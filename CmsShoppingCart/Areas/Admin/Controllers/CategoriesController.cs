using CmsShoppingCart.Infrastructure;
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
    }
}
