using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class Currency
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Symbol { get; set; } = null!;

    public decimal? ExchangeRateToVnd { get; set; }

    public bool? IsCrypto { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CurrencyRatesHistory> CurrencyRatesHistories { get; set; } = new List<CurrencyRatesHistory>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
