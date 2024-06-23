using Class_system_Backstage_pj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Class_system_Backstage_pj.Areas.video_management.Controllers
{
    [Area("video_management")]
    public class T影片VideoController : Controller
    {
        private readonly ILogger<T影片VideoController> _logger;
        private readonly studentContext _studentContext;
        public T影片VideoController(ILogger<T影片VideoController> logger, studentContext studentContext)
        {
            _logger = logger;
            _studentContext = studentContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult video()
        { return View(); }
        public IActionResult test() 
        { return View(); }
    

        [HttpPost]
        public JsonResult DeleteVideo(int id)
        {
            try
            {
                // 根據 id 刪除影片，這裡是一個示例，您需要根據實際情況修改
                var videoToDelete = _studentContext.T影片Videos.Find(id);
                if (videoToDelete != null)
                {
                    _studentContext.T影片Videos.Remove(videoToDelete);
                    _studentContext.SaveChanges();
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "影片不存在" });
            }
            catch (Exception ex)
            {
                // 處理異常情況，這裡只是簡單示例，應該進行更完整的錯誤處理
                return Json(new { success = false, message = ex.Message });
            }
        }
        public JsonResult DetailsJson()
        {
            return Json(_studentContext.T影片Videos);
        }

        [HttpPost]
        public JsonResult Create(T影片Video newVideo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // 將新影片資料添加到資料庫
                    _studentContext.T影片Videos.Add(newVideo);
                    _studentContext.SaveChanges();

                    // 返回成功的 JSON 回應
                    return Json(new { success = true, message = "影片新增成功" });
                }

                // 如果模型驗證失敗，返回失敗的 JSON 回應
                return Json(new { success = false, message = "請檢查輸入的資料" });
            }
            catch (Exception ex)
            {
                // 返回例外錯誤的 JSON 回應
                return Json(new { success = false, message = $"新增影片時發生錯誤: {ex.Message}" });
            }
        }

        public JsonResult UpdateJson()
        {
            
            return Json(_studentContext.T影片Videos);
        }


        public JsonResult Indexjson()
        {
            return Json(_studentContext.T影片Videos);
        }
        
    }
    
}
