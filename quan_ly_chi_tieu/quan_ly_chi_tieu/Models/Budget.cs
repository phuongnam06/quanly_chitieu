using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class Budget
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public Guid? CategoryId { get; set; }

    public decimal Amount { get; set; }

    public string PeriodType { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int? AlertThreshold { get; set; }

    public bool? IsRollover { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Workspace Workspace { get; set; } = null!;
}
