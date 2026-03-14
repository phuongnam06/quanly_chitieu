using System.ComponentModel.DataAnnotations;

namespace quan_ly_chi_tieu.Models.ViewModels
{
    public class DebtViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại công nợ")]
        [Display(Name = "Loại công nợ")]
        public string DebtType { get; set; } = "BORROWED"; // BORROWED (Tôi nợ), LENT (Cho vay)

        [Required(ErrorMessage = "Vui lòng nhập tên đối tác")]
        [Display(Name = "Người vay / Người cho vay")]
        public string CounterpartyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số tiền")]
        [Range(1, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        [Display(Name = "Số tiền")]
        public decimal InitialAmount { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ví liên kết")]
        [Display(Name = "Ví thực hiện")]
        public Guid WalletId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu")]
        [Display(Name = "Ngày ghi nhận")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Display(Name = "Ngày đáo hạn")]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Lãi suất (%)")]
        public decimal? InterestRate { get; set; }

        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        public List<quan_ly_chi_tieu.Models.Wallet>? Wallets { get; set; }
    }

    public class RepaymentViewModel
    {
        public Guid DebtId { get; set; }
        public string? CounterpartyName { get; set; }
        public string? DebtType { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập số tiền trả")]
        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public Guid WalletId { get; set; }

        public DateTime Date { get; set; } = DateTime.Today;
        public string? Note { get; set; }
        
        public List<quan_ly_chi_tieu.Models.Wallet>? Wallets { get; set; }
    }
}
