using Clothing_Store.DataAccess;
using Clothing_Store.ViewModels;
using Clothing_Store.ViewModels.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clothing_Store.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext dbContext;

       public AccountController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the email exists in the database
                    var existingUser = await dbContext.tblUserRegistration
                        .FirstOrDefaultAsync(u => u.Email == model.Email);

                    if (existingUser == null || existingUser.Password != model.Password)
                    {
                        // If the user with the same email doesn't exist or password doesn't match, show error message
                        ViewBag.ErrorMessage = "Oops! Username or Password is incorrect. Please try again.";
                        return View(model);
                    }

                    // HttpContext.Session.SetString("UserEmail", model.Email);

                    // Redirect to the home page or the requested returnUrl after successful login
                    return RedirectToAction("Index", "Products");
                }
                catch (Exception)
                {
                    // Handle exceptions
                    ViewBag.ErrorMessage = "An error occurred while processing your request. Please try again later.";
                    // Log the exception
                    return View(model);
                }
            }

            // If ModelState is not valid, return the view with the model data and errors
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the email or telephone number already exists in the database
                    var existingUser = await dbContext.tblUserRegistration
                        .FirstOrDefaultAsync(u => u.Email == model.Email || u.Telephone == model.Telephone);

                    if (existingUser != null)
                    {
                        // If the user with the same email or telephone number already exists, show error message
                        ViewBag.ErrorMessage = $"The user with email: {model.Email} or telephone number: {model.Telephone} already exists.";
                        return View(model);
                    }

                    // If the user does not exist, proceed with user registration
                    var userRegistration = new RegisterUserEntity()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Telephone = model.Telephone,
                        Password = model.Password,
                    };

                    // Add the user to the database
                    await dbContext.tblUserRegistration.AddAsync(userRegistration);
                    await dbContext.SaveChangesAsync();

                    HttpContext.Session.SetString("UserEmail", model.Email);

                    // Redirect the user to the home page after successful registration
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    // Handle any exceptions
                    ModelState.AddModelError(string.Empty, e.Message);
                    return RedirectToAction("Index", "Home");
                }
            }

            // If ModelState is not valid, return the view with the model data and errors
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Logout()
        {
            return Task.FromResult<IActionResult>(RedirectToAction("Index", "Home"));
        }

    }
}
