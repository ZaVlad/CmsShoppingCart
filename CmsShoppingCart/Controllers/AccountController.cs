using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signManager, IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _signManager = signManager;
            _passwordHasher = passwordHasher;
        }
        [Route("~/[controller]")]
        [AllowAnonymous]
        //GET /account/register
        public IActionResult Register() => View();

        [Route("~/[controller]")]
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //GET /account/register
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {


                AppUser appUser = new AppUser()
                {
                    UserName = user.UserName,
                    Email = user.Email
                };
                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(user);
        }

        [AllowAnonymous]
        [Route("{returnUrl?}")]
        //GET /account/register
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login()
            {
                ReturnUrl = returnUrl
            };
            return View(login);
        }

        [Route("{returnUrl?}")]
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //GET /account/register
        public async Task<IActionResult> Login(Login login)
        {

            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByEmailAsync(login.Email);
                if(appUser != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signManager.PasswordSignInAsync(appUser, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(login.ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError("", "Login failed, wrong credentials.");
            }
            
            return View(login);
        }

        [Route("{returnUrl?}")]
        //GET /account/register
        public async Task <IActionResult> Logout(string returnUrl)
        {
            await _signManager.SignOutAsync();
            return Redirect("/");
        }

        public async Task<IActionResult> Edit()
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            UserEdit user = new UserEdit(appUser);
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //GET /account/register
        public async Task<IActionResult> Edit(UserEdit user)
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (ModelState.IsValid)
            {
                appUser.Email = user.Email;
                if(user.Password != null)
                {
                    appUser.PasswordHash = _passwordHasher.HashPassword(appUser,user.Password);
                }
            }
            IdentityResult result = await _userManager.UpdateAsync(appUser);
            if(result.Succeeded)
            {
                TempData["Success"] = "Your information has been edited!";
            }

            return View();
        }

    }


}
