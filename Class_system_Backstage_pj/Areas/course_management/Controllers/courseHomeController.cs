using Microsoft.AspNetCore.Mvc;

namespace Class_system_Backstage_pj.Areas.course_management.Controllers
{
    [Area("course_management")]
    public class courseHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        
    }
}
