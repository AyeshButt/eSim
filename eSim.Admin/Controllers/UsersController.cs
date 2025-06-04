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
using eSim.Common.Enums;
using eSim.Infrastructure.Interfaces.Admin.Account;
using System.Security.Claims;
using eSim.Infrastructure.Interfaces.Admin.Email;
using eSim.Infrastructure.Interfaces.Admin.Client;
using eSim.Infrastructure.DTOs.Client;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace eSim.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailService _email;
        private readonly IAccountService _account;
        public UsersController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IAccountService account, IEmailService email)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _account = account;
            _email = email;
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
                Role = user.UserRoleId,
                UserType = user.UserType

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
                user.UserType = input.UserType;

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
        public async Task<IActionResult> AddUser()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is not null)
                BindRoleList(user.Id);

            return View(model: new ManagerUserDTO());
        }
        [Authorize(Policy = "Users:create")]

        [HttpPost]
        public async Task<IActionResult> AddUser(ManagerUserDTO input)
        {
            string token = string.Empty;
            string encodedToken = string.Empty;

            var role = _roleManager.Roles.FirstOrDefault(a => a.Id == input.Role);

            var loggedUser = await _userManager.GetUserAsync(User);

            ApplicationUser user = new ApplicationUser
            {

                UserName = input.Username,
                Email = input.Email,
                UserRoleId = role?.Id ?? null,
                ParentId = loggedUser?.Id,
            };

            #region Mapping appropriate user type
            switch (loggedUser?.UserType)
            {
                case 1:
                    user.UserType = (int)AspNetUsersTypeEnum.Superadmin;
                    break;
                case 2:
                case 3:
                    user.UserType = (int)AspNetUsersTypeEnum.Subadmin;
                    break;
                case 4:
                case 5:
                    user.UserType = (int)AspNetUsersTypeEnum.Subclient;
                    break;
                default:

                    break;
            }
            #endregion

            var userCreationResult = await _userManager.CreateAsync(user, BusinessManager.DefaultPassword);
            TempData["UserCreated"] = BusinessManager.UserCreated;

            foreach (var item in userCreationResult.Errors)
            {
                TempData["ValidationError"] = item.Description;

                return View(model: input);
            }

            if (!userCreationResult.Succeeded)
            {
                BindRoleList();
                return View(model: input);
            }

            #region Adding claims to a role
            if (role != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                await _userManager.AddClaimsAsync(user, roleClaims);
            }
            #endregion

            var model = new ClientUserDTO
            {
                Password = BusinessManager.DefaultPassword,
                UserId = user.Id,
            };
            ///
            var findUser = await _userManager.FindByIdAsync(user.Id);
                
            if (findUser is null)
            {
                TempData["EmailNotSent"] = BusinessManager.EmailNotSent;

                return RedirectToAction("ManageUsers");
            }
           
            token = await _userManager.GenerateEmailConfirmationTokenAsync(findUser);
            encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            
            model.Token = encodedToken;

            #region Sending verification and password email to the user

            var confirmationEmail = _email.SendConfirmationEmail(input.Email, model);
            var passwordEmail = _email.SendPasswordEmail(input.Email, model);

            TempData[confirmationEmail && passwordEmail ? "EmailReceived" : "EmailNotReceived"] = confirmationEmail && passwordEmail ? BusinessManager.EmailReceived : BusinessManager.EmailNotReceived;
            #endregion

            return RedirectToAction(nameof(ManageUsers));
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
        private void BindRoleList(string id = "")
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ViewBag.RolesList = _roleManager.Roles.Select(a => new SelectListItem { Value = a.Id, Text = a.Name }).ToList();
            }
            else
            {
                ViewBag.RolesList = _roleManager.Roles.Where(u => u.CreatedBy == id).Select(a => new SelectListItem { Value = a.Id, Text = a.Name }).ToList();
            }
        }

    }
}
