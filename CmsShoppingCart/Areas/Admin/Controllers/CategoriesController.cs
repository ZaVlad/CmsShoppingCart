using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    public class CategoriesController : Controller
    {
        private readonly CmsShoppingCartDbContext _context;
        
        public CategoriesController(CmsShoppingCartDbContext cmsShoppingCartDbContext)
        {
            _context = cmsShoppingCartDbContext;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _context.Categories.OrderBy(s=>s.Sorting).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            category.Slug = category.Name.ToLower().Replace(' ', '-');
            category.Sorting = 100;

            var slug = await _context.Categories.FirstOrDefaultAsync(s =>s.Slug == category.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "This category is already exist");
                return View(category);
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Category created succes";
        
            return RedirectToAction("Index");
        }

        [Route("/{Id}")]
        public async Task<IActionResult> EditAsync(int Id)
        {
            var category = await _context.Categories.FindAsync(Id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [Route("/{Id}")]
        [HttpPost]
        public async Task<IActionResult> EditAsync(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            category.Slug = category.Name.ToLower().Replace(' ','-');
            var name =  await _context.Categories.Where(s =>s.Id !=category.Id).FirstOrDefaultAsync(c =>c.Name == category.Name);

            if(name != null)
            {
                ModelState.AddModelError("", "Its category is already exist");
                return View(category);
            }
            TempData["Success"] = "Edited is accepted";
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { Id = category.Id });

        }
        public async Task<IActionResult> DeleteAsync(int Id)
        {
            var category = await _context.Categories.FindAsync(Id);

            if(category == null)
            {
                TempData["Error"] = "Its category isnt avaible to delete, its not exist";
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Success deleted";
            return RedirectToAction("Index");
        }

       [HttpPost]
        public async Task<IActionResult>Reorder(int[] id)
        {
            int count = 1;
            foreach (var categoryId in id)
            {
                Category category = await _context.Categories.FindAsync(categoryId);
                category.Sorting = count;
                _context.Update(category);
                await _context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }

    }
}
