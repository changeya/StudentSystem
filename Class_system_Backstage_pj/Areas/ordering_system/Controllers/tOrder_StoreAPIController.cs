﻿using Class_system_Backstage_pj.Areas.ordering_system.Data;
using Class_system_Backstage_pj.Areas.ordering_system.Models;
using Class_system_Backstage_pj.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TEXTpie_chart.Models;

// https://localhost:44354/tOrder_StoreAPI/storeInformation

namespace Class_system_Backstage_pj.Areas.ordering_system.Controllers
{
    public class tOrder_StoreAPIController: Controller
    {
        private readonly studentContext _myDBContext;
        private readonly IEmailSender _emailSender;
        public tOrder_StoreAPIController(studentContext myDBContext, IEmailSender emailSender)
        {
            _myDBContext = myDBContext;
            _emailSender = emailSender;
        }
        /// <summary>
        /// 呼叫所有店家資料
        /// </summary>
        /// <param name="myDBContext"></param>
        public IActionResult storeInformation()
        {
            var result = _myDBContext.T訂餐店家資料表s.
                Select(a => new 
            { 
                a.店家id,
                a.電話,
                餐廳照片 = a.餐廳照片?? "/images/user.jpg",
                a.店家名稱,
                a.電子信箱
            });
            return Json(result);
        }

        /// <summary>
        /// 依照店家id給予各菜單資訊
        /// </summary>
        /// <param name="id">指定店家</param>
        /// <returns>菜單資料回傳</returns>
        public IActionResult storeMenu(int id) 
        {
            var result = _myDBContext.T訂餐餐點資訊表s.Where(a => a.店家id == id).Select(a => new 
            {
                a.餐點名稱,
                餐點定價 = Convert.ToInt32(a.餐點定價),
                餐點照片 = a.餐點照片 ?? "/images/user.jpg",
            });
            return Json(result);
        }

        /// <summary>
        /// 傳送目前訂單數量、店家營業額加總、回傳評論平均
        /// </summary>
        /// <returns>首頁上方的方塊資料數量</returns>
        public IActionResult orderCount()
        {
            var result = _myDBContext.T訂餐訂單資訊表s.Count();
            var result1 = Convert.ToInt32(_myDBContext.T訂餐訂單詳細資訊表s.Sum(a => a.金額小記));
            var result3 = Math.Round(_myDBContext.T訂餐評論表s.Average(a => Convert.ToDouble(a.滿意度星數)), 1);
            VMstoreInformation storeInformationVM = new VMstoreInformation()
            {
                turnover = result1,
                historyorder= result,
                evaluate= result3.ToString("0.0")
            };
            return Json(storeInformationVM);
        }
        /// <summary>
        /// 表格顯示資料筆數用
        /// </summary>
        /// <param name="_search">接收前端需要顯示的資料筆數</param>
        /// <returns>依照規定回傳需要顯示的筆數</returns>
        [HttpPost]
        public IActionResult jsonsort([FromBody] StoreShow _search)
        {
            var spots = _search.storeID == 0 ? _myDBContext.T訂餐店家資料表s : _myDBContext.T訂餐店家資料表s.Where(s => s.店家id == _search.storeID);
            if (!string.IsNullOrEmpty(_search.keyword))
            {
                spots = spots.Where(s => s.店家名稱.Contains(_search.keyword) || s.電子信箱.Contains(_search.keyword)|| s.電話.Contains(_search.keyword));
            }

            int totalCount = spots.Count(); //總共幾筆
            int pagesize = _search.pageSize ?? 5; //一頁有幾筆資料
            int page = _search.page ?? 1; //目前顯示哪一頁
            int totalpage = (int)Math.Ceiling((decimal)(totalCount / pagesize)); //將小數點無條件進位，
            spots = spots.Skip((int)(page - 1) * pagesize).Take(pagesize);
            //轉換輸出格式，為了前端需求輸出
            PagingDTO sportsPagingDTO = new PagingDTO();
            sportsPagingDTO.TotalPages = totalpage;
            sportsPagingDTO.spotImagesSpots = spots.ToList();
            return Json(sportsPagingDTO);
        }
        /// <summary>
        /// 回傳評論星數數量給圓餅圖(全部不指定)
        /// </summary>
        /// <returns>圓餅圖需要的名稱與value</returns>
        public IActionResult pieComment() 
        {
            List<VMjsonpie> jsonpies = new List<VMjsonpie>();
            var result = _myDBContext.T訂餐評論表s.GroupBy(a => a.滿意度星數).Select(b => 
            new {
                滿意度星數=b.Key,
                評論數量=b.Count()
            });
            foreach ( var item in result) 
            {
                VMjsonpie vMjsonpie = new VMjsonpie()
                {
                    itemLabel= (item.滿意度星數).Trim()+"星",
                    itemValue = item.評論數量
                };
                jsonpies.Add(vMjsonpie);
            }
            return Json(jsonpies);
        }


        /// <summary>
        /// 依照年分、月份計算營業額為多少
        /// </summary>
        /// <param name="yaer">希望年分</param>
        /// <param name="id">店家id預設為null顯示全部，有指定時顯示指定</param>
        /// <returns>一到十二月營業額陣列</returns>
        public IActionResult barchart_monthly_revenue(string yaer="2023",int? id=null) 
        {
            int[] month=new int[12];
            var result = from item in _myDBContext.T訂餐訂單詳細資訊表s
                         join a in _myDBContext.T訂餐餐點資訊表s on item.餐點id equals a.餐點id
                         join b in _myDBContext.T訂餐訂單資訊表s on item.訂單id equals b.訂單id
                         where b.訂單狀態.Contains("完成")&& b.訂單時間.Substring(0,4)== yaer &&
                         (id==null||item.店家id== id)
                         group item.金額小記 by b.訂單時間.Substring(4, 2)
                         into grouped
                         select new
                         {
                             年分月 = grouped.Key,
                             訂單總額= grouped.Sum(item=>item.Value)
                         };
            for (int i = 1; i < 13; i++)
            {
                foreach (var item in result)
                {
                    if (item.年分月 == i.ToString("00"))
                    {
                        month[i-1] = Convert.ToInt32(item.訂單總額);
                        break;
                    }
                    else
                    {
                        month[i-1] = 0;
                    }
                }
            }
            return Json(month);
        }
        /// <summary>
        /// 回傳評論星數數量給圓餅圖(指定店家)
        /// </summary>
        /// <returns>圓餅圖需要的名稱與value</returns>
        public IActionResult pieComment_withstore(int id)
        {
            List<VMjsonpie> jsonpies = new List<VMjsonpie>();
            var result = (from item in _myDBContext.T訂餐評論表s
                         join a in _myDBContext.T訂餐訂單資訊表s on item.訂單id equals a.訂單id
                         join b in _myDBContext.T訂餐訂單詳細資訊表s on a.訂單id equals b.訂單id
                         join c in _myDBContext.T訂餐店家資料表s on b.店家id equals c.店家id
                         where c.店家id == id
                         select item).Distinct();
            var storedata= result.GroupBy(a => a.滿意度星數).Select(b =>
            new {
                滿意度星數 = b.Key,
                評論數量 = b.Count()
            });
            foreach (var item in storedata)
            {
                VMjsonpie vMjsonpie = new VMjsonpie()
                {
                    itemLabel = (item.滿意度星數).Trim() + "星",
                    itemValue = item.評論數量
                };
                jsonpies.Add(vMjsonpie);
            }
            return Json(jsonpies);
        }
        /// <summary>
        /// 依照店家ID回傳所有相關詳細資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult store_Detail_Information(int id) 
        {
            var store = _myDBContext.T訂餐店家資料表s.Where(a => a.店家id == id).Select(b=>new 
            {
                店家id=b.店家id,
                店家名稱=b.店家名稱,
                地址= b.地址,
                電話= b.電話,
                餐廳介紹= b.餐廳介紹,
                餐廳照片 = b.餐廳照片 ?? "/images/user.jpg",
                電子信箱=b.電子信箱,
                密碼= b.密碼
            }).FirstOrDefault();
            if (store != null) 
            {
                //將店家id存於Session之中
                HttpContext.Session.SetString("storeID", (store.店家id).ToString());
                return Json(store);
            }
            return NotFound();
        }
        /// <summary>
        /// 生成五位數驗證碼
        /// </summary>
        /// <returns></returns>
        private string GenerateCaptcha()
        {
            // 生成一個包含隨機數字的驗證碼
            Random random = new Random();
            string captcha = random.Next(10000, 99999).ToString();
            return captcha;
        }
        /// <summary>
        /// 依照店家資料發送驗證信，將驗證碼存在Session中
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult YourAction(string email)
        {
            string password = GenerateCaptcha();
            //將驗證碼存儲在 Session 中
            HttpContext.Session.SetString("VerificationCode", password);
            var htmlMessage = C_presentationEmailhtml.htmlMessage(password);
            _emailSender.SendEmailAsync(email, "修改會員資料驗證信", htmlMessage);
            return Ok();
        }
        /// <summary>
        /// 核對驗證碼是否正確，正確從快取中刪除驗證碼
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult check_verification(string id)
        {
            //從 Session 中讀取驗證碼
            string? verify = HttpContext.Session.GetString("VerificationCode");
            if (verify != null)
            {
                if (verify != id)
                {
                    return NotFound();
                }
                // 從 Session 中刪除驗證碼
                HttpContext.Session.Remove("VerificationCode");
                return Ok();
            }
            return BadRequest();
        }
        /// <summary>
        /// 輸入密碼驗證是否符合規定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task< IActionResult> Contact(PasswordviewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                );
                return Json(new { isValid = false, errors = errors });
            }
            //通過驗證，抓取特定店家修改密碼
            int? storeID = Convert.ToInt32(HttpContext.Session.GetString("storeID"));
            var store = _myDBContext.T訂餐店家資料表s.FirstOrDefault(a => a.店家id == storeID);
            if (store != null) 
            {
                store.密碼 = model.newPassword;
                await _myDBContext.SaveChangesAsync();
                HttpContext.Session.Remove("storeID");
            }
            return Json(new { isValid = true });
        }
        [HttpPost]
        public async Task<IActionResult> store_deatail_form(storeinformationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                );
                return Json(new { isValid = false, errors = errors });
            }
            //通過驗證，抓取特定店家修改店家資料
            int? storeID = Convert.ToInt32(HttpContext.Session.GetString("storeID"));
            var store = _myDBContext.T訂餐店家資料表s.FirstOrDefault(a => a.店家id == storeID);
            if (store != null)
            {
                store.店家名稱= model.storeName;
                store.電子信箱 = model.storeEmail;
                store.電話 = model.storePhone;
                store.地址 = model.storeAdress;
                store.餐廳介紹 = model.storeinformation;
                await _myDBContext.SaveChangesAsync();
                HttpContext.Session.Remove("storeID");
            }
            return Json(new { isValid = true });
        }
    }
}
