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

            var userDetails = await _dbContext.tblUserRegistration.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (userDetails == null)
            {
                return NotFound();
            }

            var model = new MyAccountViewModel
            {
                Email = userDetails.Email,
                FirstName = userDetails.FirstName,
                LastName = userDetails.LastName,
                Address1 = userDetails.Address1,
                Address2 = userDetails.Address2,
                PostCode = userDetails.PostCode,
                Country = userDetails.Country,
                Telephone = userDetails.Telephone
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
                var userDetails = this._dbContext.tblUserRegistration.FirstOrDefault(u => u.Email == userEmail);

                if (userDetails != null)
                {
                    // Update user properties with the values from the view model
                    userDetails.FirstName = model.FirstName;
                    userDetails.LastName = model.LastName;
                    userDetails.Address1 = model.Address1;
                    userDetails.Address2 = model.Address2;
                    userDetails.PostCode = model.PostCode;
                    userDetails.Country = model.Country;
                    userDetails.Telephone = model.Telephone;
                    userDetails.County = model.County;

                    // Update user in the database
                    var result = _dbContext.tblUserRegistration.Update(userDetails);
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
