using Clothing_Store.DataAccess;
using Clothing_Store.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Clothing_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Retrieve cart items from storage (e.g., session, database)
            var cartItems = GetCartItems();
            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            // Retrieve product details from the database
            var product = _context.tblProducts.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            // Add the product to the cart
            var cartItems = GetCartItems();

            cartItems.Add(new CartItemViewModel()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1,
                ImageUrl = product.ImageUrl,
            });

            // Store the updated cart items
            SaveCartItems(cartItems);

            return RedirectToAction("Index", "Cart");
        }

        private List<CartItemViewModel> GetCartItems()
        {
            // Retrieve cart items from session storage
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var cartItemsJson = session.GetString("CartItems");
                if (cartItemsJson == null)
                {
                    // If cart items don't exist, return an empty list
                    return new List<CartItemViewModel>();
                }

                // Deserialize the JSON string to a list of CartItemViewModel objects
                return JsonConvert.DeserializeObject<List<CartItemViewModel>>(cartItemsJson) ?? throw new InvalidOperationException();
            }

            // If session is null, return an empty list
            return new List<CartItemViewModel>();
        }


        private void SaveCartItems(List<CartItemViewModel> cartItems)
        {
            var cartItemsJson = JsonConvert.SerializeObject(cartItems);
            // Store the JSON string in session storage
            _httpContextAccessor.HttpContext?.Session.SetString("CartItems", cartItemsJson);

        }
    }
}
