using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class Debt
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public Guid WalletId { get; set; }

    public string DebtType { get; set; } = null!;

    public string CounterpartyName { get; set; } = null!;

    public decimal InitialAmount { get; set; }

    public decimal RemainingAmount { get; set; }

    public decimal? InterestRate { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<DebtTransaction> DebtTransactions { get; set; } = new List<DebtTransaction>();

    public virtual Wallet Wallet { get; set; } = null!;

    public virtual Workspace Workspace { get; set; } = null!;
}
