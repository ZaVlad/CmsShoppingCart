using CmsShoppingCart.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class ProductsController : Controller
    {
        readonly CmsShoppingCartDbContext _context;
        public ProductsController(CmsShoppingCartDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> IndexAsync(int p =1)
        {
            int pageSize = 6;
            var products = _context.Products.OrderBy(s => s.Name)
                .Skip((p - 1) * pageSize)
                .Take(pageSize);
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);
            return View(await products.ToListAsync());
        }
        [HttpGet("{categorySlug}")]
        public async Task<IActionResult> ProductsByCategoryAsync(string categorySlug,int p = 1)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(s => s.Slug == categorySlug);
            if(category == null)
            {
                return RedirectToAction("Index");
            }

            int pageSize = 6;
            var products = _context.Products.Where(s => s.CategoryId == category.Id).OrderBy(s => s.Name)
                .Skip((p - 1) * pageSize)
                .Take(pageSize);
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Where(s => s.CategoryId == category.Id).Count() / pageSize);
            ViewBag.CategoryName = category.Name;
            ViewBag.CategorySlug = categorySlug;

            return View(await products.ToListAsync());
        }

    }
}
