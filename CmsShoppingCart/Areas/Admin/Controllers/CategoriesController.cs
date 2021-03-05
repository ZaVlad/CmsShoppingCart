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
    [Route("[area]/[controller]")]
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

        [Route("/[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("/[action]")]
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
    }
}
