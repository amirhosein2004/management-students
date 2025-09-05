using Microsoft.AspNetCore.Mvc;

namespace StudentManagementAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "سیستم مدیریت دانشجویان";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "صفحه تماس با ما";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}