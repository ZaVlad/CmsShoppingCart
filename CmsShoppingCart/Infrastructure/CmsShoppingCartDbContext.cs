using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Infrastructure
{
    public class CmsShoppingCartDbContext : IdentityDbContext<AppUser>
    {
        public CmsShoppingCartDbContext(DbContextOptions<CmsShoppingCartDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Page> Pages {get;set;}
        public DbSet<Category> Categories {get;set;}
        public DbSet<Product> Products {get;set;}
    }
    
}
