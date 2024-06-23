using Microsoft.AspNetCore.Mvc;

namespace Class_system_Backstage_pj.Areas.ordering_system.Controllers
{
    [Area("ordering_system")]
    public class ordering_systemController : Controller
    {
        public IActionResult ordering_system_index()
        {
            return View();
        }
    }
}
