using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    public class ProductsController : Controller
    {
        private readonly CmsShoppingCartDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductsController(CmsShoppingCartDbContext cmsShoppingCartDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _context = cmsShoppingCartDbContext;
            this.webHostEnvironment = webHostEnvironment;
        }
        [ActionName("Index")]
        public  async Task<IActionResult> IndexAsync(int p=1)
        {
            int pageSize = 5;
            var products = _context.Products.OrderBy(s => s.Name)
                                                .Include(s => s.Category)
                                                .Skip((p - 1)*pageSize).Take(pageSize);
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count()/pageSize);

            return View(await products.ToListAsync());
        }
        //Get /admin/products/create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(s => s.Sorting), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Product product)
        {
           ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(s => s.Sorting), "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            product.Slug = product.Name.Trim().ToLower().Replace(" ", "-");
            var slug = await _context.Products.FirstOrDefaultAsync(s =>s.Slug == product.Slug);
            if(slug != null)
            {
                ModelState.AddModelError("","Some product with the same name is exist");
                return View(product);
            }

            string imageName = "noimage.png";
            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, @"media/products");
                imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);
                FileStream fs = new FileStream(filePath,FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close(); 
            }
            product.Image = imageName;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "The product has been created!";
            return RedirectToAction("Index");
        }

        [Route("{id}")]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            Product product = await _context.Products.Include(s =>s.Category).FirstOrDefaultAsync(s => s.Id == id);
            if(product == null)
            {
                return NotFound();
            }

            return View(product);

        }

        [Route("{id}")]
        public async Task<IActionResult> EditAsync(int id)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId =  new SelectList(_context.Categories.OrderBy(s=>s.Sorting), "Id", "Name",product.CategoryId);
            return View(product);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("{id}")]
        public async Task<IActionResult> EditAsync(int id,Product product)
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(s => s.Sorting), "Id", "Name", product.CategoryId);
            if(ModelState.IsValid)
            { 
            product.Slug = product.Name.ToLower().Replace(' ', '-');
            var slug = await _context.Products.Where(x =>x.Id!=product.Id).FirstOrDefaultAsync(x => x.Slug == product.Slug);
            if(slug != null)
            {
                ModelState.AddModelError("", "The productis already exists");
                return View(product);
            }
            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                if (!string.Equals(product.Image, "noimage.png"))
                {
                    string oldImagePath = Path.Combine(uploadsDir, product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();
                product.Image = imageName;

            }
            _context.Update(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "The product has been Updated!";
            return RedirectToAction("Index");
            }
            return View(product);
        }
         [Route("{Id}")]
         public async Task<IActionResult> DeleteAsync (int id)
            {
            Product product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                TempData["Error"] = "This product doesnt exist";
            }

            if (!string.Equals(product.Image, "noimage.png"))
            {
                string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                string oldImagePath = Path.Combine(uploadsDir, product.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            _context.Remove(product);
            TempData["Success"] = "Succesfull delete";
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
            }
    }


}
