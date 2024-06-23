using Microsoft.AspNetCore.Mvc;

namespace Class_system_Backstage_pj.Areas.class_discuss.Controllers
{
    [Area("class_discuss")]
    public class class_discussController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
