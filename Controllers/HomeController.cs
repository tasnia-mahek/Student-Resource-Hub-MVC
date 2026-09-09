using Microsoft.AspNetCore.Mvc;
using student_resource_hub.Models;
using System.Diagnostics;

namespace student_resource_hub.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Lectures()
        {
            return RedirectToAction("Lectures", "Resource");
        }

        public IActionResult PastPapers()
        {
            return RedirectToAction("PastPapers", "Resource");
        }

        public IActionResult Notes()
        {
            return RedirectToAction("Notes", "Resource");
        }

        public IActionResult Favorites()
        {
            return View();
        }

        public IActionResult Privacy()
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
