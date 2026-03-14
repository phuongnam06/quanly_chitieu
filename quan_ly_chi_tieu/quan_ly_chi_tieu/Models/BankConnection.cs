using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class BankConnection
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string BankCode { get; set; } = null!;

    public string? AccountNo { get; set; }

    public string? SyncProvider { get; set; }

    public string? ConnectionToken { get; set; }

    public string? Status { get; set; }

    public DateTime? LastSyncAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
