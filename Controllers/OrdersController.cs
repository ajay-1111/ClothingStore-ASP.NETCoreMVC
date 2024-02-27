using Clothing_Store.DataAccess;
using Clothing_Store.ViewModels.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clothing_Store.Controllers;

public class OrdersController : Controller
{
    private readonly ApplicationDbContext _context;

    private readonly UserManager<RegisterUserEntity> _userManager;

    private readonly SignInManager<RegisterUserEntity> _signInManager;


    public OrdersController(ApplicationDbContext context, SignInManager<RegisterUserEntity> signInManager, UserManager<RegisterUserEntity> userManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var cartItems = _context.tblUserCartEntities.Where(c => c.userId == user.Id).ToList();

                if (cartItems.Any())
                {
                    // Create an order
                    var order = new OrderEntity
                    {
                        UserId = user.Id,
                        OrderDate = DateTime.Now
                    };

                    _context.tblOrderEntities.Add(order);
                    await _context.SaveChangesAsync();

                    // Create order items for each cart item
                    foreach (var cartItem in cartItems)
                    {
                        var orderItem = new OrderItemEntity
                        {
                            OrderId = order.Id,
                            ProductId = cartItem.productId,
                            Quantity = cartItem.Quantity,
                            Price = cartItem.Price
                        };

                        _context.tblOrderItemEntities.Add(orderItem);
                    }

                    // Remove the cart items associated with this user
                    _context.tblUserCartEntities.RemoveRange(cartItems);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Order placed successfully!" });
                }

                return Json(new { success = false, message = "Your cart is empty. Please add items before placing an order." });
            }

            return Json(new { success = false, message = "User not found. Please sign in to place an order." });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "Failed to place order. Please try again later." });
        }
    }

}
