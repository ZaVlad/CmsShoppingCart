using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Infrastructure
{
    public class CategoriesViewComponent :ViewComponent
    {
        CmsShoppingCartDbContext _context;

        public CategoriesViewComponent(CmsShoppingCartDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await GetCategories(); 
            return View(categories);
        }

        private  Task<List<Category>> GetCategories()
        {
            return _context.Categories.OrderBy(c => c.Name).ToListAsync();
        }
    }
}
