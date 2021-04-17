using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Infrastructure
{
    public  class SmallCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            SmallCartViewModel smallCartMV;
            if(cart == null ||cart.Count==0 )
            {
                smallCartMV = null; 
            }
            else
            {
                smallCartMV = new SmallCartViewModel()
                {
                    NumberOfItem = cart.Sum(s => s.Quantity),
                    TotalAmount = cart.Sum(x => x.Quantity * x.Price)
                };
            }
            return View(smallCartMV);
        }
    }
}
