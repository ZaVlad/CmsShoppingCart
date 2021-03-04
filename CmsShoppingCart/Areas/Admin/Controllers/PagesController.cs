using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly CmsShoppingCartDbContext _context;
        public PagesController(CmsShoppingCartDbContext cmsShoppingCartDbContext)
        {
            _context = cmsShoppingCartDbContext;
        }
        [Route("[area]/[controller]/{Id?}",Name = "Index")]
        // Get /admin/pages
        public async Task<IActionResult> IndexAsync()
        {
            IQueryable<Page> pages = _context.Pages.OrderBy(p=>p.Sorting);
            var pagesList = await pages.ToListAsync();

            return View(pagesList);

        }
        [Route("[area]/[controller]/[action]/{Id}")]
        public async Task<IActionResult> DetailsAsync(int Id)
        {
            Page page = await _context.Pages.FirstOrDefaultAsync(s => s.Id == Id);
            if(page == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(page);

        }
        [Route("[area]/[controller]/[action]")]
        public  IActionResult Create()
        {
            return View();

        }
        [Route("[area]/[controller]/[action]")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(Page page)
        {
            if (!ModelState.IsValid)
            {
                return View(page);
            }
            page.Slug = page.Title.ToLower().Replace(" ", "-");
            page.Sorting = 100;

            var slug = await _context.Pages.FirstOrDefaultAsync(s => s.Slug == page.Slug);
            if(slug != null)
            {
                ModelState.AddModelError("","The page already exists");
                return View(page);
            }

             _context.Add(page);
            await _context.SaveChangesAsync();
            TempData["Success"] = "The page has been Created";
            return RedirectToAction("Index");

        }

        [Route("[area]/[controller]/[action]/{Id}")]
        public async Task<IActionResult> EditAsync(int Id)
        {
            Page page = await _context.Pages.FindAsync(Id);
            if (page == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(page);

        }

       
        [Route("[area]/[controller]/[action]/{Id}")]
        [HttpPost]
        public async Task<IActionResult> EditAsync(Page page)
        {
            if (!ModelState.IsValid)
            {
                return View(page);
            }
            page.Slug = page.Id == 1 ? "home" : page.Title.ToLower().Replace(" ", "-");
            var slug = await _context.Pages.Where(s =>s.Id!=page.Id).FirstOrDefaultAsync(s => s.Slug == page.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "The page already exists");
                return View(page);
            }

            _context.Update(page);
            await _context.SaveChangesAsync();
            TempData["Success"] = "The page has been edited";
            return RedirectToAction("Edit",new { Id = page.Id});

        }

        [Route("[area]/[controller]/[action]/{Id}")]
        public async Task<IActionResult> DeleteAsync(int Id)
        {
            Page page = await _context.Pages.FindAsync(Id);
            if (page == null)
            {
                TempData["Error"] = "The page has not exists";
            }
            else
            {
                _context.Pages.Remove(page);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The page has already removed";
            }

            return RedirectToAction("Index");



        }
    }
}
