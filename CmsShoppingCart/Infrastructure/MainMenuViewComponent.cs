﻿using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Infrastructure
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly CmsShoppingCartDbContext _context;
        public MainMenuViewComponent(CmsShoppingCartDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();
            return View(pages);
        }

        private Task<List<Page>> GetPagesAsync()
        {
             return _context.Pages.OrderBy(s => s.Sorting).ToListAsync(); 
        }
    }
}
