using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class DebtTransaction
{
    public Guid Id { get; set; }

    public Guid DebtId { get; set; }

    public Guid TransactionId { get; set; }

    public string? ActionType { get; set; }

    public decimal Amount { get; set; }

    public decimal PrincipalAmount { get; set; }

    public decimal InterestAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Debt Debt { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
