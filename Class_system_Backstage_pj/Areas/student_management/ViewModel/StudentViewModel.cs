using System.ComponentModel.DataAnnotations;

namespace Class_system_Backstage_pj.Areas.student_management.ViewModel
{
    public class StudentViewModel
    {
        [Display(Name = "姓名")]
        public string? 姓名 { get; set; }

        [Display(Name = "性別")]
        public string? 性別 { get; set; }

        [Display(Name = "信箱")]
        public string? 信箱 { get; set; }

        [Display(Name = "手機")]
        public string? 手機 { get; set; }

        [Display(Name = "活躍狀態")]
        public string? 狀態 { get; set; }

        [Display(Name = "鎖定狀態")]
        public string? 鎖定 { get; set; }
    }
}

