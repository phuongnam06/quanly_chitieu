using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public Guid? WorkspaceId { get; set; }

    public Guid? ParentId { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Icon { get; set; }

    public string? Color { get; set; }

    public int? DisplayOrder { get; set; }

    public bool? IsSystem { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    public virtual ICollection<ClassificationRule> ClassificationRules { get; set; } = new List<ClassificationRule>();

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual Category? Parent { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual Workspace? Workspace { get; set; }
}
