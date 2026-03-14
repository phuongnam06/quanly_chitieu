using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class Subscription
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public Guid WalletId { get; set; }

    public Guid CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Frequency { get; set; } = null!;

    public int? Interval { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public DateOnly NextExecutionDate { get; set; }

    public int? ReminderDays { get; set; }

    public bool? IsAutoInsert { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;

    public virtual Workspace Workspace { get; set; } = null!;
}
