using Clothing_Store.DataAccess;
using Clothing_Store.ViewModels;
using Clothing_Store.ViewModels.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Clothing_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly UserManager<RegisterUserEntity> _userManager;

        private readonly SignInManager<RegisterUserEntity> _signInManager;

        public CartController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<RegisterUserEntity> userManager, SignInManager<RegisterUserEntity> signInManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cartItems = await GetCartItemsForCurrentUser();
            ViewBag.CartItemCount = await GetCartItemCount();
            return View(cartItems);
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart(int id)
        {
            TempData["CartItemCount"] = null;

            // Retrieve product details from the database
            var product = await _context.tblProducts.FirstOrDefaultAsync(p => p.Id == id);

            var checkIfUserSignedInOrNot = _signInManager.IsSignedIn(User);

            if (checkIfUserSignedInOrNot)
            {
                var user = _userManager.GetUserId(User);

                if (user != null)
                {
                    // Check if the item is already in the cart.
                    var getTheQuantity = await _context.tblUserCartEntities.FirstOrDefaultAsync(p => p.productId == id);
                    if (getTheQuantity != null)
                    {
                        // If the item is already in the cart then increase the quantity.
                        getTheQuantity.Quantity += 1;

                        _context.Update(getTheQuantity);
                    }
                    else
                    {
                        // user has no cart but adding a new item to the existing cart.

                        if (product != null)
                        {
                            UserCartEntity newUserCartEntity = new UserCartEntity()
                            {
                                productId = product.Id,
                                userId = user,
                                Quantity = 1,
                                Price = product.Price
                            };

                            await _context.tblUserCartEntities.AddAsync(newUserCartEntity);
                        }
                    }
                }
                else
                {
                    // user has no cart. Adding a brand new cart for the user.

                    UserCartEntity newUserCartEntity = new UserCartEntity()
                    {
                        productId = product!.Id,
                        userId = user!,
                        Quantity = 1,
                        Price = product.Price
                    };

                    await _context.tblUserCartEntities.AddAsync(newUserCartEntity);
                }

                await _context.SaveChangesAsync();
            }


            // Set the cart item count in TempData
            int cartItemCount = await GetCartItemCount();
            TempData["CartItemCount"] = cartItemCount;

            return RedirectToAction("Index", "Products");
        }
        private async Task<int> GetCartItemCount()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    return await _context.tblUserCartEntities
                        .Where(u => u.userId == user.Id)
                        .SumAsync(u => u.Quantity);
                }
            }
            return 0;
        }
        private async Task<List<CartItemViewModel>> GetCartItemsForCurrentUser()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var cartItems = await (from uc in _context.tblUserCartEntities
                        join p in _context.tblProducts on uc.productId equals p.Id
                        where uc.userId == user.Id
                        select new CartItemViewModel
                        {
                            ProductId = p.Id,
                            ImageUrl = p.ImageUrl,
                            ProductName = p.Name,
                            Price = p.Price,
                            Quantity = uc.Quantity
                        }).ToListAsync();

                    return cartItems;
                }
            }
            return new List<CartItemViewModel>();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            // Retrieve the current user's ID from the HttpContext
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                // Handle the case where the user is not authenticated
                return Unauthorized();
            }

            // Find the item in the user's cart
            var cartItem = await _context.tblUserCartEntities.FirstOrDefaultAsync(c => c.productId == productId && c.userId == userId);

            if (cartItem != null)
            {
                _context.tblUserCartEntities.Remove(cartItem);
                await _context.SaveChangesAsync();

                // Return a success response
                return Json(new { success = true });
            }

            // Return an error response if the item was not found in the cart
            return Json(new { success = false });
        }

    }
}
