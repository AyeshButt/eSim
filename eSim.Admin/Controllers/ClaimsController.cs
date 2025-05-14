using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eSim.EF.Entities;
using eSim.Infrastructure.Interfaces.SystemClaimRepo;
using eSim.Infrastructure.DTOs.AccessControl;

namespace eSim.Admin.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ISystemClaimService _systemClaims;

        public ClaimsController(ISystemClaimService systemClaims)
        {
            _systemClaims = systemClaims;
        }
        [Authorize(Policy = "Claims:view")]

        #region Claims
        public IActionResult ManageRoleClaims()
        {
            var systemClaims = _systemClaims.GetClaims();
            return View(model: systemClaims.ToList());
        }

        [Authorize(Policy = "Claims:create")]

        [HttpGet]
        public IActionResult AddRoleClaims()
        {
            BindSideMenuList();
            BindSideMenuWithParents();

            return View(model: new RoleClaimDTO());
        }
        [Authorize(Policy = "Claims:create")]

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


        [Authorize(Policy = "Claims:edit")]

        [HttpGet]
        public async Task<IActionResult> EditRoleClaims(string id)
        {

            var model = new RoleClaimDTO();

            var result = await _systemClaims.GetClaimByIdAsync(Id: id);

            BindSideMenuList();

            BindSideMenuWithParents(Convert.ToInt32(result?.ParentType));

            return View(model: result ?? model);
        }
        [Authorize(Policy = "Claims:edit")]

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
            ViewBag.RolesList = _systemClaims.GetSideMenus().Where(u => u.ParentId == null).Select(a => new SelectListItem { Text = a.Title, Value = a.Id.ToString() }).ToList();

        }
        private void BindSideMenuWithParents(int? parentId = null)
        {
            if (parentId is null)
            {
                ViewBag.SideMenuChild = _systemClaims.GetSideMenus().Where(u => u.ParentId != null).Select(a => new SelectListItem { Text = a.Title, Value = a.Title }).ToList();

            }
            ViewBag.SideMenuChild = _systemClaims.GetSideMenus().Where(u => u.ParentId != null && u.ParentId == parentId).Select(a => new SelectListItem { Text = a.Title, Value = a.Title }).ToList();

        }
        [HttpGet]
        public IActionResult GetOptions(string id)
        {
            var sideMenuId = Convert.ToInt32(id);

            var subSideMenu = _systemClaims.GetSubSideMenus(sideMenuId).ToList();

            //// Filter options based on the selected id
            var filteredOptions = subSideMenu.Select(u => u.ClaimType).ToList();

            return Json(filteredOptions);
        }
        #endregion


    }
}
