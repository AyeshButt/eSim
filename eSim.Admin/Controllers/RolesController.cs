using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eSim.EF.Entities;
using eSim.Infrastructure.Interfaces.SystemClaimRepo;
using System.Security.Claims;
using eSim.Infrastructure.DTOs.AccessControl;

namespace eSim.Admin.Controllers
{
    public class RolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ISystemClaimService _systemClaims;
        public RolesController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ISystemClaimService systemClaims)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _systemClaims = systemClaims;
        }
        [Authorize(Policy = "Roles:view")]

        #region Roles

        [HttpGet]
        public IActionResult ManageRoles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var roles = _roleManager.Roles.Where(u=>u.CreatedBy == userId).Select(a => new ManageRoleDTO { Id = a.Id, RoleName = a.Name });
            return View(model: roles.ToList());
        }
        [Authorize(Policy = "Roles:create")]

        [HttpGet]
        public IActionResult AddRole()
        {

            return View(model: new RoleDTO());
        }
        [Authorize(Policy = "Roles:create")]

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleDTO input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                var roleCreationResult = await _roleManager.CreateAsync(new ApplicationRole
                {
                    Name = input.RoleName,

                    CreatedBy = userId ?? string.Empty,
                });

                if (roleCreationResult.Succeeded)
                {

                    return RedirectToAction(nameof(ManageRoles));
                }
                else
                {
                    foreach (var error in roleCreationResult.Errors)
                    {
                        TempData["ValidationError"] = error.Description;
                        ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);
                    }
                }

            }
            return View(model: input);
        }

        [Authorize(Policy = "Roles:edit")]

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {

            RoleDTO model = new RoleDTO();

            var role = _roleManager.Roles.FirstOrDefault(a => a.Id == id);

            var existingRolesClaims = await _roleManager.GetClaimsAsync(role);

            var ListOfClaims = new List<List<ClaimItems>>();


            var groups = _systemClaims.GetClaims().OrderBy(u => u.Title).GroupBy(a => a.Title);

            foreach (var temp in groups)
            {

                foreach (var item in temp.GroupBy(u => u.ClaimType))
                {
                    var ListOfIndiviualSideMenu = new List<ClaimItems>();

                    foreach (var sample in item)
                    {
                        ListOfIndiviualSideMenu.Add(new ClaimItems
                        {
                            Type = sample.ClaimType,
                            Value = sample.ClaimValue,
                            isSelected = existingRolesClaims.Any(a => a.Value == sample.ClaimValue && a.Type == sample.ClaimType),
                            Title = sample.Title,
                        });
                    }

                    ListOfClaims.Add(ListOfIndiviualSideMenu);

                }

            }
            //foreach (var group in groups)
            //{
            //    var ListOfIndiviualSideMenu = new List<ClaimItems>();
            //    foreach (var item in group)
            //    {
            //        ListOfIndiviualSideMenu.Add(new ClaimItems
            //        {
            //            Type = item.ClaimType,
            //            Value = item.ClaimValue,
            //            isSelected = existingRolesClaims.Any(a => a.Value == item.ClaimValue && a.Type == item.ClaimType),
            //            Title = item.Title,
            //        });
            //    }

            //    ListOfClaims.Add(ListOfIndiviualSideMenu);
            //}

            if (role is not null)
            {
                model.Id = role.Id;
                model.RoleName = role.Name ?? string.Empty;
            }

            model.Claims = ListOfClaims;
            ViewBag.GroupedList = ListOfClaims
    .SelectMany(innerList => innerList) // Flatten the list of lists
    .GroupBy(item => item.Title)        // Group by the Title property
    .ToList();

            return View(model: model);
        }
        [Authorize(Policy = "Roles:edit")]

        [HttpPost]
        public async Task<IActionResult> EditRole(RoleDTO input)
        {
            if (ModelState.IsValid)
            {


                var role = _roleManager.Roles.FirstOrDefault(a => a.Id == input.Id);

                if (role is not null)
                {
                    role.Name = input.RoleName;
                }


                var roleUpdateResult = await _roleManager.UpdateAsync(role);

                var existingRolesClaims = await _roleManager.GetClaimsAsync(role);




                if (roleUpdateResult.Succeeded)
                {


                    foreach (var group in input.Claims)
                    {
                        foreach (var item in group)
                        {

                            if ((item.isSelected && existingRolesClaims.Any(a => a.Type == item.Type && a.Value == item.Value)) ||
                                !item.isSelected && !existingRolesClaims.Any(a => a.Type == item.Type && a.Value == item.Value))
                            {
                                continue;
                            }
                            else if (item.isSelected && !existingRolesClaims.Any(a => a.Type == item.Type && a.Value == item.Value))

                            {
                                /// add
                                var claim = new Claim(type: item.Type, value: item.Value);
                                await _roleManager.AddClaimAsync(role, claim);
                            }
                            else if (!item.isSelected && existingRolesClaims.Any(a => a.Type == item.Type && a.Value == item.Value))

                            {
                                /// remove
                                var claim = new Claim(type: item.Type, value: item.Value);
                                await _roleManager.RemoveClaimAsync(role, claim);
                            }
                        }
                    }


                    var userInRole = await _userManager.Users.Where(a => a.UserRoleId == role.Id).ToListAsync(); ;//
                    if (userInRole is not null)
                    {
                        foreach (var user in userInRole)
                        {
                            var claimstoRemove = await _userManager.GetClaimsAsync(user);
                            var removalResult = await _userManager.RemoveClaimsAsync(user, claimstoRemove);

                            if (removalResult.Succeeded)
                            {
                                var claimstoAdd = await _roleManager.GetClaimsAsync(role);
                                var assignmentResult = await _userManager.AddClaimsAsync(user, claimstoAdd);
                            }
                        }
                    }

                    return RedirectToAction(nameof(ManageRoles));
                }
                else
                {
                    foreach (var error in roleUpdateResult.Errors)
                    {
                        ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);
                    }
                }

            }
            return View(model: input);
        }

        #endregion

    }
}
