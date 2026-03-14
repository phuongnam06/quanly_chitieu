using System.ComponentModel.DataAnnotations;

namespace quan_ly_chi_tieu.Models.ViewModels
{
    public class SavingGoalViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên mục tiêu")]
        [Display(Name = "Tên mục tiêu")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số tiền mục tiêu")]
        [Range(1, double.MaxValue)]
        [Display(Name = "Số tiền cần tiết kiệm (đ)")]
        public decimal TargetAmount { get; set; }

        [Display(Name = "Đã tiết kiệm được (đ)")]
        public decimal CurrentAmount { get; set; } = 0;

        [Display(Name = "Ngày đặt mục tiêu")]
        public DateTime? Deadline { get; set; }

        [Display(Name = "Biểu tượng")]
        public string Icon { get; set; } = "🎯";

        [Display(Name = "Màu sắc")]
        public string Color { get; set; } = "#9C27B0";
    }
}
