using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class InvestmentTransaction
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public Guid AssetId { get; set; }

    public Guid WalletId { get; set; }

    public string ActionType { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? Fee { get; set; }

    public decimal? Tax { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual InvestmentAsset Asset { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;

    public virtual Workspace Workspace { get; set; } = null!;
}
