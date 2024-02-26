using Clothing_Store.DataAccess;
using Clothing_Store.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Clothing_Store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
           // Retrieve all products from the database
            var products = _context.tblProducts.ToList();

            // Check if products list is empty
            if (products.Count == 0)
            {
                // If products list is empty, set an error message using ViewBag
                ViewBag.ErrorMessage = "No products available at the moment.";
                return View();
            }

            // Create a list to hold the view models for all products
            List<ProductViewModel> productViewModels = new List<ProductViewModel>();

            // Loop through each product and create a view model for it
            foreach (var product in products)
            {
                ProductViewModel productModel = new ProductViewModel()
                {
                    ImageUrl = product.ImageUrl,
                    Name = product.Name,
                    Price = product.Price,
                    Rating = product.Rating,
                    Id = product.Id,
                };

                // Add the view model to the list
                productViewModels.Add(productModel);
            }

            // Pass the list of view models to the view
            return View(productViewModels);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            // Check if id is null
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the product with the specified id from the database
            var product = _context.tblProducts.FirstOrDefault(p => p.Id == id);

            // Check if product is null
            if (product == null)
            {
                return NotFound();
            }

            // Pass the product to the view for rendering
            return View(product);
        }

    }
}
