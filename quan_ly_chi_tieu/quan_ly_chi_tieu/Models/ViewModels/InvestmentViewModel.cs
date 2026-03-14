using System.ComponentModel.DataAnnotations;

namespace quan_ly_chi_tieu.Models.ViewModels
{
    public class AssetViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại tài sản")]
        public string AssetType { get; set; } = "STOCK"; // STOCK, CRYPTO, GOLD, REAL_ESTATE

        [Required(ErrorMessage = "Vui lòng nhập mã (Symbol)")]
        public string Symbol { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập tên tài sản")]
        public string Name { get; set; } = string.Empty;

        public decimal? CurrentPrice { get; set; }
    }

    public class InvestmentTxViewModel
    {
        [Required]
        public Guid AssetId { get; set; }

        [Required]
        public string TransactionType { get; set; } = "BUY"; // BUY, SELL, DIVIDEND

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá thực hiện")]
        public decimal Price { get; set; }

        [Required]
        public Guid WalletId { get; set; }

        public DateTime Date { get; set; } = DateTime.Today;

        public List<quan_ly_chi_tieu.Models.Wallet>? Wallets { get; set; }
    }
}
