using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class DailyWalletSnapshot
{
    public Guid WalletId { get; set; }

    public DateOnly Date { get; set; }

    public decimal Balance { get; set; }

    public decimal? ExchangeRate { get; set; }

    public virtual Wallet Wallet { get; set; } = null!;
}
