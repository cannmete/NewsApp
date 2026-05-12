
using Microsoft.AspNetCore.Mvc;

namespace NewsApp.UI.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult RoleManagement()
        {
            return View();
        }

        public IActionResult Categories()
        {
            return View();
        }

        public IActionResult News()
        {
            return View();
        }

        public IActionResult Comments() 
        { 
            return View(); 
        }
        public IActionResult Logs()
        {
            return View();
        }
    }
}