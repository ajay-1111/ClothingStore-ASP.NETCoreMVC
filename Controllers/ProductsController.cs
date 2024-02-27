using Clothing_Store.DataAccess;
using Clothing_Store.ViewModels;
using Clothing_Store.ViewModels.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            TempData["NoProducts"] = null;
           // Retrieve all products from the database
            var products = _context.tblProducts.ToList();

            // Check if products list is empty
            if (products.Count == 0)
            {
                // If products list is empty, set an error message using ViewBag
                TempData["NoProducts"] = "No products available at the moment.";
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
        public async Task<IActionResult> Details(int productId)
        {
            TempData["NoProductFound"] = null;

            // Retrieve the product with the specified id from the database
            var product = await _context.tblProducts.FirstOrDefaultAsync(p => p.Id == productId);

            // Check if product is null
            if (product == null)
            {
                TempData["NoProductFound"] = $"Unable to find the product details for ID : {productId}";
            }
            
            ProductViewModel productModel = new ProductViewModel()
            {
                ImageUrl = product!.ImageUrl,
                Name = product.Name,
                Price = product.Price,
                Rating = product.Rating,
                Id = product.Id,
            };
            
            // Pass the product to the view for rendering
            return View(productModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            if (!string.IsNullOrWhiteSpace(category))
            {
                var categoryEnum = (ProductsEntity.ProductCategory)Enum.Parse(typeof(ProductsEntity.ProductCategory), category, true);

                var products = await _context.tblProducts
                    .Where(p => p.ProductCategoryId == categoryEnum)
                    .ToListAsync();

                if (products.Count == 0)
                {
                    TempData["NoProducts"] = $"No products available for category: {category}";
                    return RedirectToAction("Index");
                }

                var productViewModels = products.Select(product => new ProductViewModel
                {
                    ImageUrl = product.ImageUrl,
                    Name = product.Name,
                    Price = product.Price,
                    Rating = product.Rating,
                    Id = product.Id
                }).ToList();

                return View("Index", productViewModels);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var results = await _context.tblProducts
                .Where(p => p.Name.Contains(query))
                .Select(p => new { p.Name })
                .ToListAsync();

            return Json(results);
        }
    }
}
