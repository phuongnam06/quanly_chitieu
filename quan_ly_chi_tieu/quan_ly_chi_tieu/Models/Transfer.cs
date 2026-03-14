using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class Transfer
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public Guid CreatedByUserId { get; set; }

    public Guid FromWalletId { get; set; }

    public Guid ToWalletId { get; set; }

    public decimal Amount { get; set; }

    public decimal? Fee { get; set; }

    public decimal? FromAmount { get; set; }

    public decimal? ToAmount { get; set; }

    public DateTime TransferDate { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual Wallet FromWallet { get; set; } = null!;

    public virtual Wallet ToWallet { get; set; } = null!;

    public virtual Workspace Workspace { get; set; } = null!;
}
