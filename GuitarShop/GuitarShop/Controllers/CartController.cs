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
        return View();
    }

    // Add a guitar to the cart
    public async Task<IActionResult> AddToCart(string guitarName)
    {
        return RedirectToAction("Index");
    }

    // Remove a guitar from the cart
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        return RedirectToAction("Index");
    }
}