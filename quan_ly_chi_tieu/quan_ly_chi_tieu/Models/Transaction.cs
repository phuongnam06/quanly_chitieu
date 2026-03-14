using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public Guid CreatedByUserId { get; set; }

    public Guid WalletId { get; set; }

    public Guid CategoryId { get; set; }

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? Note { get; set; }

    public string? Payee { get; set; }

    public string? Location { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? Status { get; set; }

    public bool? IsExcludedFromReport { get; set; }

    public string? Source { get; set; }

    public string? ReferenceId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual ICollection<BillSplit> BillSplits { get; set; } = new List<BillSplit>();

    public virtual Category Category { get; set; } = null!;

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual ICollection<DebtTransaction> DebtTransactions { get; set; } = new List<DebtTransaction>();

    public virtual ICollection<ReceiptItem> ReceiptItems { get; set; } = new List<ReceiptItem>();

    public virtual Wallet Wallet { get; set; } = null!;

    public virtual Workspace Workspace { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
