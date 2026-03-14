using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace quan_ly_chi_tieu.Models.ViewModels
{
    public class TransactionViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số tiền")]
        [Range(1, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        [Display(Name = "Số tiền")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ví")]
        [Display(Name = "Ví")]
        public Guid WalletId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày")]
        [Display(Name = "Ngày giao dịch")]
        public DateTime TransactionDate { get; set; } = DateTime.Today;

        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        [Display(Name = "Người thụ hưởng / Nguồn")]
        public string? Payee { get; set; }

        public string Type { get; set; } = "EXPENSE"; // EXPENSE | INCOME

        [Display(Name = "Ảnh hóa đơn / Đính kèm")]
        public IFormFile? AttachmentFile { get; set; }

        public List<quan_ly_chi_tieu.Models.Attachment>? Attachments { get; set; }
        
        // For dropdown lists
        public List<quan_ly_chi_tieu.Models.Category>? Categories { get; set; }
        public List<quan_ly_chi_tieu.Models.Wallet>? Wallets { get; set; }
    }

    public class TransactionFilterViewModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? WalletId { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Type { get; set; } // EXPENSE | INCOME | all
        public string? Keyword { get; set; }
    }
}
