using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class SavingTransaction
{
    public Guid Id { get; set; }

    public Guid SavingGoalId { get; set; }

    public Guid WalletId { get; set; }

    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = null!;

    public DateTime TransactionDate { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual SavingGoal SavingGoal { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}
