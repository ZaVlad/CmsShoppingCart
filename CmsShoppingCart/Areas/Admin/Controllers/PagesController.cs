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
    public class PagesController : Controller
    {
        private readonly CmsShoppingCartDbContext _context;
        public PagesController(CmsShoppingCartDbContext cmsShoppingCartDbContext)
        {
            _context = cmsShoppingCartDbContext;
        }
        [Route("[area]/[controller]")]
        public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = _context.Pages.OrderBy(p=>p.Sorting);
            var pagesList = await pages.ToListAsync();

            return View(pagesList);

        }
    }
}
