using System.ComponentModel.DataAnnotations;

namespace quan_ly_chi_tieu.Models.ViewModels
{
    public class SubscriptionViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên dịch vụ/giao dịch")]
        [Display(Name = "Tên dịch vụ / Giao dịch")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số tiền")]
        [Range(1, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        [Display(Name = "Số tiền")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại")]
        [Display(Name = "Loại giao dịch")]
        public string Type { get; set; } = "EXPENSE";

        [Required(ErrorMessage = "Vui lòng chọn ví")]
        [Display(Name = "Ví mặc định")]
        public Guid WalletId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn tần suất")]
        [Display(Name = "Tần suất")]
        public string Frequency { get; set; } = "MONTHLY"; // WEEKLY, MONTHLY, YEARLY

        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Display(Name = "Tự động ghi nhận")]
        public bool IsAutoInsert { get; set; } = true;

        public List<quan_ly_chi_tieu.Models.Category>? Categories { get; set; }
        public List<quan_ly_chi_tieu.Models.Wallet>? Wallets { get; set; }
    }
}
