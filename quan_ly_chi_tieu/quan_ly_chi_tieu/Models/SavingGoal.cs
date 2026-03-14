using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class SavingGoal
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public string Name { get; set; } = null!;

    public decimal TargetAmount { get; set; }

    public decimal? CurrentAmount { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? TargetDate { get; set; }

    public string? Icon { get; set; }

    public string? Color { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<SavingTransaction> SavingTransactions { get; set; } = new List<SavingTransaction>();

    public virtual Workspace Workspace { get; set; } = null!;
}
