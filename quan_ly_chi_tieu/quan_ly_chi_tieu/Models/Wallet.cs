using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class Wallet
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public string Name { get; set; } = null!;

    public string WalletType { get; set; } = null!;

    public Guid? BankConnectionId { get; set; }

    public decimal? InitialBalance { get; set; }

    public decimal? CurrentBalance { get; set; }

    public string? CurrencyCode { get; set; }

    public decimal? CreditLimit { get; set; }

    public int? StatementDate { get; set; }

    public int? PaymentDueDate { get; set; }

    public string? Icon { get; set; }

    public string? Color { get; set; }

    public bool? IsIncludedInTotal { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual BankConnection? BankConnection { get; set; }

    public virtual Currency? CurrencyCodeNavigation { get; set; }

    public virtual ICollection<DailyWalletSnapshot> DailyWalletSnapshots { get; set; } = new List<DailyWalletSnapshot>();

    public virtual ICollection<Debt> Debts { get; set; } = new List<Debt>();

    public virtual ICollection<InvestmentTransaction> InvestmentTransactions { get; set; } = new List<InvestmentTransaction>();

    public virtual ICollection<SavingTransaction> SavingTransactions { get; set; } = new List<SavingTransaction>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Transfer> TransferFromWallets { get; set; } = new List<Transfer>();

    public virtual ICollection<Transfer> TransferToWallets { get; set; } = new List<Transfer>();

    public virtual Workspace Workspace { get; set; } = null!;
}
