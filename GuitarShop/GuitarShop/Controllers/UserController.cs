using GuitarShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuitarShop.Data;
using Microsoft.AspNetCore.Identity;

namespace GuitarShop.Controllers
{
    public class UserController : Controller
    {
        private readonly GuitarShopContext _context;
        private readonly UserManager<User> _userManager;

        public UserController(GuitarShopContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Cart()
        {
            var allGuitars = (from g in _context.Guitar
                          select g).ToList();

            var user = await _userManager.GetUserAsync(User);

            var guitars = new List<Guitar>();

            foreach(var guitarName in user.Cart)
            {
                guitars.Add(allGuitars.FirstOrDefault(x => x.Name == guitarName));
            }

            return View(guitars);
        }
    }
}
