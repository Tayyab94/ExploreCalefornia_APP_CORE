using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCalefornia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExploreCalefornia.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        // GET: /<controller>/
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl=null)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var result = await signInManager.PasswordSignInAsync(model.EmailAddress, model.Password, model.RememberMe, false);

            if(!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Login Error!");
                return View();
            }

            if(String.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(returnUrl);
        }

        [HttpPost]

        public  async Task<IActionResult> Logout(string returnUrl=null)
        {
            await signInManager.SignOutAsync();

            if(string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            return Redirect(returnUrl);
        }


        [HttpGet]

        public IActionResult Register()
        {
            return View(new RegistrationViewModel());
        }

        [HttpPost]

        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var newUser = new IdentityUser
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress,

            };

            var result = await userManager.CreateAsync(newUser, model.Password);
            if(!result.Succeeded)
            {
                foreach (var item in result.Errors.Select(s=>s.Description))
                {
                    ModelState.AddModelError("", item);
                }
                return View();
            }

            return RedirectToAction("Login");
        }

    }
}
