using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eSim.Common;
using eSim.Infrastructure.DTOs;
using eSim.EF.Entities;
using eSim.Common.StaticClasses;
using eSim.Infrastructure.DTOs.AccessControl;

namespace eSim.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Authorize(Policy = "Users:view")]

        #region Users
        public async Task<IActionResult> ManageUsers()
        {
            var roles = _roleManager.Roles.Select(a => new SelectListItem { Value = a.Id, Text = a.Name }).ToList();

            var user = _userManager.Users.Select(a => new UserDTO { Username = a.UserName, Email = a.Email, Id = a.Id });

            return View(model: await user.ToListAsync());
        }
        [Authorize(Policy = "Users:edit")]

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
        [Authorize(Policy = "Users:edit")]

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
        [Authorize(Policy = "Users:create")]

        [HttpGet]
        public IActionResult AddUser()
        {
            BindRoleList();

            return View(model: new ManagerUserDTO());
        }
        //[Authorize(Policy = "Users:create")]

        [HttpPost]
        public async Task<IActionResult> AddUser(ManagerUserDTO input)
        {
            var role = _roleManager.Roles.FirstOrDefault(a => a.Id == input.Role);

            ApplicationUser user = new ApplicationUser
            {

                UserName = input.Username,
                Email = input.Email,
                UserRoleId = role?.Id ?? null,
            };
            var userCreationResult = await _userManager.CreateAsync(user, BusinessManager.DefaultPassword);
            
            foreach(var item in userCreationResult.Errors)
            {
                TempData["ValidationError"] = item.Description;

                return View(model: input);
            }



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
        [Authorize(Policy = "Users:disable")]

        [HttpGet]
        public async Task<IActionResult> DisableUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            user.LockoutEnabled = true; 
            user.LockoutEnd = DateTime.UtcNow.AddMonths(3600);
            await _userManager.UpdateAsync(user);
            return View();
        }
        [Authorize(Policy = "Users:delete")]

        [HttpGet]
        public async Task<IActionResult> DeleteUser()
        {
            return View();
        }
        private void BindRoleList()
        {
            ViewBag.RolesList = _roleManager.Roles.Select(a => new SelectListItem { Value = a.Id, Text = a.Name }).ToList();
        }
    }
}
