using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eSim.EF.Context;
using eSim.Infrastructure.DTOs;
using eSim.EF.Entities;

namespace eSim.Admin.Controllers
{
    public class SideMenusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SideMenusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SideMenus
        public async Task<IActionResult> Index()
        {


            return View(await _context.SideMenu.ToListAsync());
          
        }

        [Authorize(Policy = "SideMenus:view")]

        // GET: SideMenus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sideMenu = await _context.SideMenu
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sideMenu == null)
            {
                return NotFound();
            }

            return View(sideMenu);
        }
        [Authorize(Policy = "SideMenus:create")]

        // GET: SideMenus/Create
        public IActionResult Create()
        {
            BindSideMenusList();

            return View();
        }
        [Authorize(Policy = "SideMenus:create")]

        // POST: SideMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ControllerName,ActionName,Title,ParentId")] SideMenu sideMenu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sideMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sideMenu);
        }
        [Authorize(Policy = "SideMenus:edit")]

        // GET: SideMenus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sideMenu = await _context.SideMenu.FindAsync(id);
            
            if (sideMenu == null)
            {
                return NotFound();
            }
            
            BindSideMenusList();

            return View(sideMenu);
        }
        [Authorize(Policy = "SideMenus:edit")]

        // POST: SideMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ControllerName,ActionName,Title,ParentId")] SideMenu sideMenu)
        {
            if (id != sideMenu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sideMenu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SideMenuExists(sideMenu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sideMenu);
        }
        [Authorize(Policy = "SideMenus:delete")]

        // GET: SideMenus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sideMenu = await _context.SideMenu
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sideMenu == null)
            {
                return NotFound();
            }

            return View(sideMenu);
        }
        [Authorize(Policy = "SideMenus:delete")]

        // POST: SideMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sideMenu = await _context.SideMenu.FindAsync(id);
            if (sideMenu != null)
            {
                _context.SideMenu.Remove(sideMenu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SideMenuExists(int id)
        {
            return _context.SideMenu.Any(e => e.Id == id);
        }
        private void BindSideMenusList()
        {
            ViewBag.SideMenuList = _context.SideMenu.Where(u => u.ParentId == null).Select(u => new SelectListItem() { Text = u.Title, Value = u.Id.ToString() });
        }
    }
}

