using Class_system_Backstage_pj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Class_system_Backstage_pj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly studentContext _studentContext;

        public HomeController(ILogger<HomeController> logger, studentContext studentContext)
        {
            _logger = logger;
            _studentContext = studentContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Privacy1()
        {
            return View();
        }
        public IActionResult Privacy2()
        {
            return View();
        }
        public JsonResult indexjson()
        {
            return Json(_studentContext.T訂餐店家資料表s);
        }
        public IActionResult Create()
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
