using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eSim.Common;
using eSim.Infrastructure.DTOs;
using eSim.EF.Entities;
using eSim.Infrastructure.Interfaces.SystemClaimRepo;
using System.Data;
using System.Security.Claims;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.AccessControl;

namespace eSim.Admin.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ISystemClaimService _systemClaims;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ISystemClaimService systemClaims)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _systemClaims = systemClaims;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region Users
        public async Task<IActionResult> ManageUsers()
        {


            var roles = _roleManager.Roles.Select(a => new SelectListItem { Value = a.Id, Text = a.Name }).ToList();
            var user = _userManager.Users.Select(a => new UserDTO { Username = a.UserName, Email = a.Email, Id = a.Id });

            return View(model: user.ToList());
        }

        [HttpGet]
        public IActionResult EditUser(string id)
        {

            BindRoleList();
            var user = _userManager.Users.First(a => a.Id == id);
            if (user is null)
            {
                return View(viewName: "NotFound");
            }

            ManagerUserDTO model = new ManagerUserDTO
            {

                Email = user.Email,
                Id = user.Id,
                Username = user.UserName,
                Role = user.UserRoleId



            };
            return View(model: model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(ManagerUserDTO input)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Users.First(a => a.Id == input.Id);
                if (user is null)
                {
                    return View(viewName: "NotFound");
                }


                if (user.UserRoleId == input.Role)
                {
                    // we don't need to change the claims against this user
                }
                else
                {
                    var old_role = _roleManager.Roles.FirstOrDefault(a => a.Id == user.UserRoleId);
                    var new_role = _roleManager.Roles.First(a => a.Id == input.Role);
                    if (old_role is not null)
                    {
                        await _userManager.RemoveClaimsAsync(user, await _roleManager.GetClaimsAsync(old_role));
                    }


                    if (new_role is not null)
                    {
                        await _userManager.AddClaimsAsync(user, await _roleManager.GetClaimsAsync(new_role));
                        user.UserRoleId = new_role.Id;
                    }



                }

                user.Email = input.Email;
                user.UserName = input.Username;

                var updateUserResult = await _userManager.UpdateAsync(user);
                if (updateUserResult.Succeeded)
                {
                    return RedirectToAction(nameof(ManageUsers));
                }
            }
            BindRoleList();
            return View(model: input);
        }

        #endregion


        #region Roles

        [HttpGet]
        public IActionResult ManageRoles()
        {
            var roles = _roleManager.Roles.Select(a => new ManageRoleDTO { Id = a.Id, RoleName = a.Name });
            return View(model: roles.ToList());
        }

        [HttpGet]
        public IActionResult AddRole()
        {

            return View(model: new RoleDTO());
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleDTO input)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
                        ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);
                    }
                }

            }
            return View(model: input);
        }


        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {

            RoleDTO model = new RoleDTO();

            var role = _roleManager.Roles.FirstOrDefault(a => a.Id == id);

            var existingRolesClaims = await _roleManager.GetClaimsAsync(role);


            var ListOfClaims = new List<List<ClaimItems>>();

            var groups = _systemClaims.GetClaims().GroupBy(a => a.ClaimType);

            foreach (var group in groups)
            {
                var ListOfIndiviualSideMenu = new List<ClaimItems>();
                foreach (var item in group)
                {
                    ListOfIndiviualSideMenu.Add(new ClaimItems
                    {
                        Type = item.ClaimType,
                        Value = item.ClaimValue,
                        isSelected = existingRolesClaims.Any(a => a.Value == item.ClaimValue && a.Type == item.ClaimType)
                    });
                }
                ListOfClaims.Add(ListOfIndiviualSideMenu);
            }

            if (role is not null)
            {
                model.Id = role.Id;
                model.RoleName = role.Name ?? string.Empty;
            }

            model.Claims = ListOfClaims;
            return View(model: model);
        }
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


        #region Claims
        public IActionResult ManageRoleClaims()
        {
            var systemClaims = _systemClaims.GetClaims();
            return View(model: systemClaims.ToList());
        }


        [HttpGet]
        public IActionResult AddRoleClaims()
        {
            BindSideMenuList();
            return View(model: new RoleClaimDTO());
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleClaims(RoleClaimDTO input)
        {
            if (ModelState.IsValid)
            {

                var addSystemClaimsResult = await _systemClaims.AddClaimAsync(input);

                if (addSystemClaimsResult)
                {
                    return RedirectToAction(nameof(ManageRoleClaims));
                }
            }

            BindSideMenuList();
            return View(model: input);
        }



        [HttpGet]
        public async Task<IActionResult> EditRoleClaims(string id)
        {

            var model = new RoleClaimDTO();

            var result = await _systemClaims.GetClaimByIdAsync(Id: id);

            BindSideMenuList();
            return View(model: result ?? model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRoleClaims(RoleClaimDTO input)
        {
            if (ModelState.IsValid)
            {

                var editResult = await _systemClaims.EditClaimAsync(input);

                if (editResult)
                {
                    return RedirectToAction(nameof(ManageRoleClaims));
                }

            }

            BindSideMenuList();
            return View(model: input);
        }


        private void BindSideMenuList()
        {
            ViewBag.RolesList = _systemClaims.GetSideMenus().Select(a => new SelectListItem { Text = a.Title, Value = a.Title }).ToList();
        }


        #endregion


        #region Manage User 



        private void BindRoleList()
        {
            ViewBag.RolesList = _roleManager.Roles.Select(a => new SelectListItem { Value = a.Id, Text = a.Name }).ToList();
        }


        [HttpGet]
        public IActionResult AddUser()
        {


            BindRoleList();

            return View(model: new ManagerUserDTO());
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(ManagerUserDTO input)
        {



            var role = _roleManager.Roles.FirstOrDefault(a => a.Id == input.Role);

            ApplicationUser user = new ApplicationUser
            {

                UserName = input.Username,
                Email = input.Email,
                UserRoleId = role?.Id ?? null

            };
            var userCreationResult = await _userManager.CreateAsync(user, BusinessManager.DefaultPassword);




            if (userCreationResult.Succeeded)
            {

                if (role is not null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    await _userManager.AddClaimsAsync(user, roleClaims);
                }


                return RedirectToAction(nameof(ManageUsers));
            }


            BindRoleList();
            return View(model: input);
        }

        #endregion


    }
}
