using Class_system_Backstage_pj.Areas.student_management.Models.DTO;
using Class_system_Backstage_pj.Areas.student_management.ViewModel;
using Class_system_Backstage_pj.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;


namespace Class_system_Backstage_pj.Areas.student_management.Controllers
{
    [Area("student_management")]
    public class MemberController : Controller
    {
        private readonly ILogger<MemberController> _logger;
        private readonly studentContext _studentContext;

        public MemberController(ILogger<MemberController> logger, studentContext studentContext)
        {
            _logger = logger;
            _studentContext = studentContext;
        }


        public IActionResult Index()
        {
            return View();
        }




        // GET: api/Member
        [HttpGet]
        public IActionResult GetAllStudent()
        {
            //return Ok(members);
            var members = _studentContext.T會員學生s.ToList();
            return View(members);
        }

        public IActionResult GetAllStudentJson()
        {
            var members = _studentContext.T會員學生s;
            return Json(members);

        }


        public IActionResult CheckStudentEmail(T會員學生 user)
        {
            bool isDuplicate = _studentContext.T會員學生s.Any(p => p.信箱 == user.信箱);
            return Content($"{isDuplicate}", "text/plain", Encoding.UTF8);
        }


        // GET: api/Member/5
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var member = _studentContext.T會員學生s.Find(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }


        public IActionResult RegisterStudent()
        {
            return View();
        }

        // POST: api/Member
        [HttpPost]
        public IActionResult RegisterStudent(MemberRegisterDTO memberRegisterinfo)
        {

            //if (!ModelState.IsValid)
            //{
            //    return View(memberRegisterinfo);
            //}

            byte[]? _filebyte = null;

            if (memberRegisterinfo.圖片 != null && memberRegisterinfo.圖片.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    // 將圖片數據讀取到內存流中
                    memberRegisterinfo.圖片.CopyTo(memoryStream);

                    // 將內存流中的數據轉換為 byte 陣列
                    _filebyte = memoryStream.ToArray();
                }
            }

            string _salt = GenerateSalt();
            string _hashPassword = HashPassword(memberRegisterinfo.密碼, _salt);

            var memberEntity = new T會員學生
            {
                姓名 = memberRegisterinfo.姓名,
                性別 = memberRegisterinfo.性別,
                身分證字號 = memberRegisterinfo.身分證字號,
                信箱 = memberRegisterinfo.信箱,
                手機 = memberRegisterinfo.手機,
                地址 = memberRegisterinfo.地址,
                生日 = memberRegisterinfo.生日,
                學校 = memberRegisterinfo.學校,
                科系 = memberRegisterinfo.科系,
                學位 = memberRegisterinfo.學位,
                畢肄 = memberRegisterinfo.畢肄,
                圖片 = _filebyte,
                密碼 = _hashPassword,
                Salt = _salt,
                註冊日期 = DateTime.Now,
                修改日期 = DateTime.Now,
            };



            try
            {
                _studentContext.T會員學生s.Add(memberEntity);
                _studentContext.SaveChanges();

                return Ok(new { message = "註冊成功", name = memberRegisterinfo.姓名 });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "註冊失敗", error = ex.Message, name = memberRegisterinfo.姓名 });
            }
        }

        //創造鹽 (亂碼)
        private string GenerateSalt()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);
                return Convert.ToBase64String(salt);//轉成base64字串
            }
        }

        //創造Hash
        private string HashPassword(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())// SHA-256 雜湊
            {
                byte[] combined = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashedBytes = sha256.ComputeHash(combined);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // PUT: api/Member/5
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, T會員學生 updatedMember)
        {
            if (id != updatedMember.學生id)
            {
                return BadRequest();
            }

            var existingMember = _studentContext.T會員學生s.Find(id);
            if (existingMember == null)
            {
                return NotFound();
            }

            existingMember = updatedMember;
            _studentContext.SaveChanges();

            return NoContent();
        }


        // DELETE: api/Member/5 先封印這方法
        //[HttpDelete("{id}")]
        //public IActionResult DeleteMember(int id)
        //{
        //    var member = _studentContext.T會員學生s.Find(id);
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }

        //    _studentContext.T會員學生s.Remove(member);
        //    _studentContext.SaveChanges();

        //    return NoContent();
        //}



    }
}
