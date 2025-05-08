using System.Diagnostics;
using GuitarShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuitarShop.Data;

namespace GuitarShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly GuitarShopContext _context;

        public HomeController(GuitarShopContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var bodyTypes = _context.Guitar
            .Select(g => g.Body)
            .Distinct()
            .ToList();

            return View(bodyTypes.ToList());
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Guitars()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Shops()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
