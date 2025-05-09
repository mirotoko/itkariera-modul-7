using GuitarShop.Data;
using GuitarShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CartController : Controller
{
    private readonly GuitarShopContext _guitarShopContext;
    private readonly UserManager<Users> _userManager;

    public CartController(GuitarShopContext guitarShopContext, UserManager<Users> userManager)
    {
        _guitarShopContext = guitarShopContext;
        _userManager = userManager;
    }

    // View the current user's cart
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var cartItems = _guitarShopContext.Carts
            .Include(c => c.Guitar)
            .Where(c => c.UserId == user.Id)
            .ToList();

        return View(cartItems);
    }

    // Add a guitar to the cart
    public async Task<IActionResult> AddToCart(string guitarName)
    {
        var user = await _userManager.GetUserAsync(User);
        var existing = _guitarShopContext.Carts.FirstOrDefault(c => c.UserId == user.Id && c.GuitarName == guitarName);

        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            _guitarShopContext.Carts.Add(new Cart
            {
                UserId = user.Id,
                GuitarName = guitarName,
                Quantity = 1
            });
        }

        await _guitarShopContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    // Remove a guitar from the cart
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        var cartItem = await _guitarShopContext.Carts.FindAsync(id);
        if (cartItem != null)
        {
            _guitarShopContext.Carts.Remove(cartItem);
            await _guitarShopContext.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}