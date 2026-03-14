using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class CurrencyRatesHistory
{
    public Guid Id { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public decimal Rate { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? Source { get; set; }

    public virtual Currency CurrencyCodeNavigation { get; set; } = null!;
}
