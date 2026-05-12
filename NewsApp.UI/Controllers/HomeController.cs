using Microsoft.AspNetCore.Mvc;
using NewsApp.UI.Models;
using System.Diagnostics;

namespace NewsApp.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewsDetail(int id)
        {
            ViewBag.NewsId = id; // ID'yi View'a gönderiyoruz
            return View();
        }

        public IActionResult WriteNews()
        {
            return View();
        }

        public IActionResult Profile()
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
