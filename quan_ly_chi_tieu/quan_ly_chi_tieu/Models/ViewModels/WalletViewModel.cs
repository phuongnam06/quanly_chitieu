using System.ComponentModel.DataAnnotations;

namespace quan_ly_chi_tieu.Models.ViewModels
{
    public class WalletViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên ví")]
        [Display(Name = "Tên ví")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn loại ví")]
        [Display(Name = "Loại ví")]
        public string WalletType { get; set; } = "CASH";

        [Display(Name = "Số dư ban đầu")]
        [Range(0, double.MaxValue, ErrorMessage = "Số dư không được âm")]
        public decimal InitialBalance { get; set; } = 0;

        [Display(Name = "Biểu tượng")]
        public string Icon { get; set; } = "💰";

        [Display(Name = "Màu sắc")]
        public string Color { get; set; } = "#4CAF50";

        [Display(Name = "Bao gồm trong tổng tài sản")]
        public bool IsIncludedInTotal { get; set; } = true;
    }
}
