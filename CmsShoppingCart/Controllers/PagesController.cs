using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Controllers
{
    [Route("[controller]/[action]")]
    public class PagesController : Controller
    {
        private readonly CmsShoppingCartDbContext _context;
        public PagesController(CmsShoppingCartDbContext context)
        {
            _context = context;
        }
        [Route("{slug?}")]
        [Route("~/")]
        public async Task<IActionResult> PageAsync(string slug)
        {
            if(slug == null)
            {
                return View(await _context.Pages.Where(x => x.Slug =="home").FirstOrDefaultAsync());
            }
            Page page = await _context.Pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();

            if(page == null)
            {
                return NotFound();
            }

            return View(page);
        }

    }
}
