using Clothing_Store.DataAccess;
using Clothing_Store.ViewModels;
using Clothing_Store.ViewModels.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clothing_Store.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        private readonly SignInManager<RegisterUserEntity> signInManager;

        private readonly UserManager<RegisterUserEntity> userManager;

        public AccountController(ApplicationDbContext dbContext, SignInManager<RegisterUserEntity> signInManager, UserManager<RegisterUserEntity> userManager)
        {
            this.dbContext = dbContext;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            TempData["CartItemCount"] = null;
            TempData["LoginError"] = null;

            if (ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password,
                    isPersistent: false,
                    lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    // Set the cart item count in TempData
                    return RedirectToAction("Index", "Products");
                }

                TempData["LoginError"] = "Incorrect username/password. Register here if you are new.";
            }

            // If ModelState is not valid, return the view with the model data and errors
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            TempData["RegisterationMessage"] = null;
            TempData["RegisterationErrorMessage"] = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var newUser = new RegisterUserEntity()
                    {
                        UserName = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Telephone = model.Telephone,
                        Password = model.Password,
                    };

                    var registration = await this.userManager.CreateAsync(newUser, model.Password);

                    if (registration.Succeeded)
                    {
                        await this.signInManager.SignInAsync(newUser, isPersistent: false);
                        TempData["RegisterationMessage"] = "Hurry!.. Successfully register.Please login to continue.";
                    }
                    
                    return View(model);
                }
                catch (Exception ex)
                {
                    TempData["RegisterationErrorMessage"] = ex.Message;
                    return View(model);
                }
            }

            // If ModelState is not valid, return the view with the model data and errors
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
