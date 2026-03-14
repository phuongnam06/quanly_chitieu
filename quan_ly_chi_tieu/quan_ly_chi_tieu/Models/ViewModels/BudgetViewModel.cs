using System.ComponentModel.DataAnnotations;

namespace quan_ly_chi_tieu.Models.ViewModels
{
    public class BudgetViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số tiền ngân sách")]
        [Range(1, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        [Display(Name = "Hạn mức ngân sách (đ)")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Chu kỳ")]
        public string PeriodType { get; set; } = "MONTHLY"; // DAILY, WEEKLY, MONTHLY, YEARLY

        [Display(Name = "Ngưỡng cảnh báo (%)")]
        [Range(1, 100)]
        public int AlertThreshold { get; set; } = 80;

        [Display(Name = "Tự động tái tạo")]
        public bool IsRollover { get; set; } = false;

        // Populated values for display
        public decimal SpentAmount { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryIcon { get; set; }
        public string? CategoryColor { get; set; }

        public List<quan_ly_chi_tieu.Models.Category>? Categories { get; set; }
    }
}
