using Clothing_Store.DataAccess;
using Clothing_Store.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clothing_Store.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public MyAccountController(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> MyAccount()
        {
            string? userEmail = "ajayreddy_gopu@gmail.com";

            var userDetails = await _dbContext.AspNetUsers.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (userDetails == null)
            {
                return NotFound();
            }

            var model = new MyAccountViewModel
            {
                Email = userDetails.Email,
                FirstName = userDetails.FirstName,
                LastName = userDetails.LastName,
                Telephone = userDetails.Telephone,
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount(MyAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve user's current email from session
                string? userEmail = "ajayreddy_gopu@gmail.com";

                // Retrieve user entity from the database based on the email
                var userDetails = this._dbContext.AspNetUsers.FirstOrDefault(u => u.Email == userEmail);

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

                // User not found
                ModelState.AddModelError(string.Empty, "User not found.");
            }

            // If ModelState is not valid, return the view with the model data and errors
            return RedirectToAction("Index", "Home");
        }

    }
}
