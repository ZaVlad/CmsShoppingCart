using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("[area]/[controller]/[action]")]
    public class UsersController : Controller
    {
            private readonly UserManager<AppUser> _userManager;
            public UsersController(UserManager<AppUser> userManager)
            {
                _userManager = userManager;
            }
            public IActionResult Index()
            {
            return View(_userManager.Users);
            }
    }
}
