using Clothing_Store.DataAccess;
using Clothing_Store.ViewModels;
using Clothing_Store.ViewModels.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clothing_Store.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly UserManager<RegisterUserEntity> _userManager;

        private readonly SignInManager<RegisterUserEntity> _signInManager;

        public MyAccountController(ApplicationDbContext _dbContext, SignInManager<RegisterUserEntity> signInManager, UserManager<RegisterUserEntity> userManager)
        {
            this._dbContext = _dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> MyAccount()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound();
                }

                var model = new MyAccountViewModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Telephone = user.Telephone,
                };

                return View(model);
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount(MyAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_signInManager.IsSignedIn(User))
                {
                    var user = await _userManager.GetUserAsync(User);

                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Retrieve user entity from the database based on the email
                    var userDetails = this._dbContext.AspNetUsers.FirstOrDefault(u => u.Email == user.Email);

                    if (userDetails != null)
                    {
                        // Update user properties with the values from the view model
                        userDetails.FirstName = model.FirstName;
                        userDetails.LastName = model.LastName;
                        userDetails.Telephone = model.Telephone;

                        // Update user in the database
                        var result = _dbContext.AspNetUsers.Update(userDetails);
                        await _dbContext.SaveChangesAsync();

                        // Update successful
                        return RedirectToAction("Index", "Products");
                    }
                }

                // User not found
                ModelState.AddModelError(string.Empty, "User not found.");
            }

            // If ModelState is not valid, return the view with the model data and errors
            return RedirectToAction("Index", "Home");
        }

    }
}
