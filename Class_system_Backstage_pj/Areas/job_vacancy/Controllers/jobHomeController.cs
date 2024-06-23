using Class_system_Backstage_pj.Models;
using Microsoft.AspNetCore.Mvc;

namespace Class_system_Backstage_pj.Areas.job_vacancy.Controllers
{
    [Area("job_vacancy")]
    public class jobHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Resume()
        {
            return View();
        }
        public IActionResult Company()
        {
            return View();
        }
        public IActionResult Vacancy()
        {
            return View();
        }
    }
}
