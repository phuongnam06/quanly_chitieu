using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class Workspace
{
    public Guid Id { get; set; }

    public Guid OwnerId { get; set; }

    public string Name { get; set; } = null!;

    public string? WorkspaceType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<ClassificationRule> ClassificationRules { get; set; } = new List<ClassificationRule>();

    public virtual ICollection<Debt> Debts { get; set; } = new List<Debt>();

    public virtual ICollection<InvestmentAsset> InvestmentAssets { get; set; } = new List<InvestmentAsset>();

    public virtual ICollection<InvestmentTransaction> InvestmentTransactions { get; set; } = new List<InvestmentTransaction>();

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<SavingGoal> SavingGoals { get; set; } = new List<SavingGoal>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();

    public virtual ICollection<WorkspaceMember> WorkspaceMembers { get; set; } = new List<WorkspaceMember>();
}
