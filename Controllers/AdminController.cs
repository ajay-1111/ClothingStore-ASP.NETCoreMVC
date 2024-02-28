using Microsoft.AspNetCore.Mvc;
using Clothing_Store.DataAccess;
using Clothing_Store.ViewModels;
using Clothing_Store.ViewModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clothing_Store.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var products = _context.tblProducts.OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalProducts = _context.tblProducts.Count();

            var model = new PaginatedList<ProductsEntity>(products, totalProducts, page, pageSize);

            return View(model);
        }

        // Action method to display form for adding a new product
        public IActionResult Create()
        {
            TempData["AddSuccess"] = null;
            TempData["AddError"] = null;
            TempData["UpdateSuccess"] = null;
            TempData["UpdateError"] = null;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductsEntity product, IFormFile? productImage)
        {
            TempData["AddSuccess"] = null;
            TempData["AddError"] = null;

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if a file is uploaded
                    if (productImage is { Length: > 0 })
                    {
                        // Generate a unique filename for the image
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(productImage.FileName);

                        // Get the path of the wwwroot/img folder where images will be stored
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                        // Combine the unique filename with the path to store the image
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Copy the uploaded file to the specified path
                        await using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await productImage.CopyToAsync(stream);
                        }

                        // Update the ImageUrl property of the product with the new filename
                        product.ImageUrl = uniqueFileName;
                    }

                    // Set CreateDateTime and ModifieDateTime
                    product.CreateDateTime = DateTime.Now;
                    product.ModifieDateTime = DateTime.Now;

                    // Add the product to the database
                    _context.tblProducts.Add(product);
                    await _context.SaveChangesAsync();

                    TempData["AddSuccess"] = $"ProductID {product.Id} has been added successfully.";

                    // Redirect to the product list page after successful creation
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log or handle the exception appropriately
                    TempData["AddError"] = $"Exception while adding the new product : {ex.Message}";
                }
            }

            // If the model state is not valid, return the view with the model data and errors
            return View(product);
        }


        // Action method to display form for updating product details
        public IActionResult Edit(int id)
        {
            TempData["AddSuccess"] = null;
            TempData["AddError"] = null;
            TempData["UpdateSuccess"] = null;
            TempData["UpdateError"] = null;
            var product = _context.tblProducts.Find(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductsEntity product)
        {
            TempData["UpdateSuccess"] = null;
            TempData["UpdateError"] = null;

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update other properties of the product as usual
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["UpdateSuccess"] = $"ProductID {product.Id} has been updated successfully.";
                }
                catch (Exception ex)
                {
                    TempData["UpdateError"] = $"Exception updating the product : {ex.Message}.";
                }
            }
            return RedirectToAction("Index");
        }


        // Action method to delete a product
        public IActionResult Delete(int id)
        {
            TempData["AddSuccess"] = null;
            TempData["AddError"] = null;
            TempData["UpdateSuccess"] = null;
            TempData["UpdateError"] = null;

            var product = _context.tblProducts.Find(id);
            if (product != null) _context.tblProducts.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
