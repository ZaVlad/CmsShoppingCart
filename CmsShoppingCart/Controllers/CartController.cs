using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Controllers
{
    [Route("[controller]/[action]")]
    public class CartController : Controller
    {
        CmsShoppingCartDbContext _context;
        public CartController(CmsShoppingCartDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new CartViewModel { CartItems = cart, GrandTotal = cart.Sum(s => s.Price * s.Quantity) };
            return View(cartVM);
        }

        [Route("{id}")]
        public async Task<IActionResult> AddAsync(int id)
        {
            Product product = await _context.Products.FindAsync(id);

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity++;
            }

            HttpContext.Session.SetJson("Cart", cart);

            if (HttpContext.Request.Headers["X-Requested-With"] !="XMLHttpRequest")
            {
                return RedirectToAction("Index");
            }

            return ViewComponent("SmallCart");
            
        }
        [Route("{id}")]
        public async Task<IActionResult> DecreaseAsync(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartItem cartitem = cart.Where(c => c.ProductId == id).FirstOrDefault();
            if (cartitem.Quantity > 1)
            {
                --cartitem.Quantity;
            }
            else
            {
                cart.RemoveAll(s => s.ProductId == id);
            }
            HttpContext.Session.SetJson("Cart", cart);
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            return RedirectToAction("Index");
        }
        [Route("{id}")]
        public IActionResult Remove(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            cart.RemoveAll(s => s.ProductId == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");


            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return Redirect(HttpContext.Request.Headers["Referer"].ToString());

            return Ok(); 
        }

    }
}
