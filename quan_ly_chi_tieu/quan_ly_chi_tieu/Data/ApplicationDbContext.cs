using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using quan_ly_chi_tieu.Models;

namespace quan_ly_chi_tieu.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<BankConnection> BankConnections { get; set; }

    public virtual DbSet<BillSplit> BillSplits { get; set; }

    public virtual DbSet<BillSplitParticipant> BillSplitParticipants { get; set; }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ClassificationRule> ClassificationRules { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<CurrencyRatesHistory> CurrencyRatesHistories { get; set; }

    public virtual DbSet<DailyWalletSnapshot> DailyWalletSnapshots { get; set; }

    public virtual DbSet<Debt> Debts { get; set; }

    public virtual DbSet<DebtTransaction> DebtTransactions { get; set; }

    public virtual DbSet<InvestmentAsset> InvestmentAssets { get; set; }

    public virtual DbSet<InvestmentTransaction> InvestmentTransactions { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<ReceiptItem> ReceiptItems { get; set; }

    public virtual DbSet<SavingGoal> SavingGoals { get; set; }

    public virtual DbSet<SavingTransaction> SavingTransactions { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPreference> UserPreferences { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<Workspace> Workspaces { get; set; }

    public virtual DbSet<WorkspaceMember> WorkspaceMembers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attachme__3214EC0785153E10");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FileSize).HasDefaultValue(0);
            entity.Property(e => e.FileType).HasMaxLength(50);

            entity.HasOne(d => d.Transaction).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK_Attachments_Transaction");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuditLog__3214EC077B444997");

            entity.HasIndex(e => e.RecordId, "IX_AuditLogs_RecordId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.TableName).HasMaxLength(50);
        });

        modelBuilder.Entity<BankConnection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankConn__3214EC0767C914E6");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountNo).HasMaxLength(50);
            entity.Property(e => e.BankCode).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastSyncAt).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("ACTIVE");
            entity.Property(e => e.SyncProvider).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.BankConnections)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankConn_Users");
        });

        modelBuilder.Entity<BillSplit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BillSpli__3214EC0748C29EAE");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SplitType)
                .HasMaxLength(20)
                .HasDefaultValue("EQUAL");

            entity.HasOne(d => d.Transaction).WithMany(p => p.BillSplits)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK_BillSplits_Transaction");
        });

        modelBuilder.Entity<BillSplitParticipant>(entity =>
        {
            entity.HasKey(e => new { e.BillSplitId, e.UserId }).HasName("PK__BillSpli__9B0DB8B486C6D241");

            entity.Property(e => e.OwedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaidAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("PENDING");

            entity.HasOne(d => d.BillSplit).WithMany(p => p.BillSplitParticipants)
                .HasForeignKey(d => d.BillSplitId)
                .HasConstraintName("FK_BSParticipants_Split");

            entity.HasOne(d => d.User).WithMany(p => p.BillSplitParticipants)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BSParticipants_User");
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Budgets__3214EC0763BEFCD7");

            entity.HasIndex(e => new { e.WorkspaceId, e.CategoryId }, "IX_Budgets_Workspace_Category");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AlertThreshold).HasDefaultValue(80);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRollover).HasDefaultValue(false);
            entity.Property(e => e.PeriodType).HasMaxLength(20);

            entity.HasOne(d => d.Category).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Budgets_Categories");

            entity.HasOne(d => d.Workspace).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.WorkspaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Budgets_Workspaces");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07CDACF8E0");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Color).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsSystem).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(20);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Categories_Parent");

            entity.HasOne(d => d.Workspace).WithMany(p => p.Categories)
                .HasForeignKey(d => d.WorkspaceId)
                .HasConstraintName("FK_Categories_Workspaces");
        });

        modelBuilder.Entity<ClassificationRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Classifi__3214EC075B364D01");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.SearchKeyword).HasMaxLength(100);

            entity.HasOne(d => d.TargetCategory).WithMany(p => p.ClassificationRules)
                .HasForeignKey(d => d.TargetCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rules_Categories");

            entity.HasOne(d => d.Workspace).WithMany(p => p.ClassificationRules)
                .HasForeignKey(d => d.WorkspaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rules_Workspaces");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__Currenci__A25C5AA6D95B2A40");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.ExchangeRateToVnd)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(24, 8)")
                .HasColumnName("ExchangeRateToVND");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsCrypto).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Symbol).HasMaxLength(10);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<CurrencyRatesHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Currency__3214EC07253D9DE4");

            entity.ToTable("CurrencyRatesHistory");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CurrencyCode).HasMaxLength(10);
            entity.Property(e => e.Rate).HasColumnType("decimal(24, 8)");
            entity.Property(e => e.Source).HasMaxLength(100);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.CurrencyRatesHistories)
                .HasForeignKey(d => d.CurrencyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CurrencyRates_Currencies");
        });

        modelBuilder.Entity<DailyWalletSnapshot>(entity =>
        {
            entity.HasKey(e => new { e.WalletId, e.Date }).HasName("PK__DailyWal__F3A77EDEDAB4199E");

            entity.HasIndex(e => e.Date, "IX_DailySnapshots_Date");

            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExchangeRate)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(24, 8)");

            entity.HasOne(d => d.Wallet).WithMany(p => p.DailyWalletSnapshots)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK_Snapshots_Wallets");
        });

        modelBuilder.Entity<Debt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Debts__3214EC0796ECB2C9");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CounterpartyName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DebtType).HasMaxLength(20);
            entity.Property(e => e.InitialAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InterestRate)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.RemainingAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("ACTIVE");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Debts)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Debts_Wallets");

            entity.HasOne(d => d.Workspace).WithMany(p => p.Debts)
                .HasForeignKey(d => d.WorkspaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Debts_Workspaces");
        });

        modelBuilder.Entity<DebtTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DebtTran__3214EC07565939DA");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ActionType).HasMaxLength(20);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InterestAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PrincipalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Debt).WithMany(p => p.DebtTransactions)
                .HasForeignKey(d => d.DebtId)
                .HasConstraintName("FK_DebtTrans_Debt");

            entity.HasOne(d => d.Transaction).WithMany(p => p.DebtTransactions)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DebtTrans_Transaction");
        });

        modelBuilder.Entity<InvestmentAsset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Investme__3214EC071EBB073E");

            entity.HasIndex(e => e.WorkspaceId, "IX_InvestmentAssets_Workspace");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AssetType).HasMaxLength(50);
            entity.Property(e => e.CurrentPrice)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(24, 8)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Symbol).HasMaxLength(20);

            entity.HasOne(d => d.Workspace).WithMany(p => p.InvestmentAssets)
                .HasForeignKey(d => d.WorkspaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Investments_Workspaces");
        });

        modelBuilder.Entity<InvestmentTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Investme__3214EC07AA15CAD8");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ActionType).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Fee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(24, 8)");
            entity.Property(e => e.Tax)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Asset).WithMany(p => p.InvestmentTransactions)
                .HasForeignKey(d => d.AssetId)
                .HasConstraintName("FK_InvTrans_Asset");

            entity.HasOne(d => d.Wallet).WithMany(p => p.InvestmentTransactions)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvTrans_Wallet");

            entity.HasOne(d => d.Workspace).WithMany(p => p.InvestmentTransactions)
                .HasForeignKey(d => d.WorkspaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvTrans_Workspaces");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC0732A2EF42");

            entity.HasIndex(e => new { e.UserId, e.IsRead }, "IX_Notifications_User_Unread");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Notifications_Users");
        });

        modelBuilder.Entity<ReceiptItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReceiptI__3214EC07EBB10892");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Discount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ItemName).HasMaxLength(200);
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Transaction).WithMany(p => p.ReceiptItems)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK_Receipts_Transaction");
        });

        modelBuilder.Entity<SavingGoal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SavingGo__3214EC07EA3BC1FE");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Color).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrentAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("ACTIVE");
            entity.Property(e => e.TargetAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Workspace).WithMany(p => p.SavingGoals)
                .HasForeignKey(d => d.WorkspaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SavingGoals_Workspaces");
        });

        modelBuilder.Entity<SavingTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SavingTr__3214EC07C66EF6AA");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionType).HasMaxLength(20);

            entity.HasOne(d => d.SavingGoal).WithMany(p => p.SavingTransactions)
                .HasForeignKey(d => d.SavingGoalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SavingTrans_Goal");

            entity.HasOne(d => d.Wallet).WithMany(p => p.SavingTransactions)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SavingTrans_Wallet");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC07F31884DC");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Frequency).HasMaxLength(20);
            entity.Property(e => e.Interval).HasDefaultValue(1);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsAutoInsert).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ReminderDays).HasDefaultValue(3);
            entity.Property(e => e.Type).HasMaxLength(20);

            entity.HasOne(d => d.Category).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subscriptions_Categories");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subscriptions_Wallets");

            entity.HasOne(d => d.Workspace).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.WorkspaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subscriptions_Workspaces");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tags__3214EC07CDFED3DE");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Color).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Workspace).WithMany(p => p.Tags)
                .HasForeignKey(d => d.WorkspaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tags_Workspaces");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC0749235A6B");

            entity.HasIndex(e => e.CategoryId, "IX_Transactions_Category");

            entity.HasIndex(e => e.WalletId, "IX_Transactions_Wallet");

            entity.HasIndex(e => new { e.WorkspaceId, e.TransactionDate }, "IX_Transactions_Workspace_Date");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsExcludedFromReport).HasDefaultValue(false);
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Payee).HasMaxLength(100);
            entity.Property(e => e.ReferenceId).HasMaxLength(100);
            entity.Property(e => e.Source)
                .HasMaxLength(50)
                .HasDefaultValue("MANUAL");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("COMPLETED");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_Categories");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_Users");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_Wallets");

            entity.HasOne(d => d.Workspace).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WorkspaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_Workspaces");

            entity.HasMany(d => d.Tags).WithMany(p => p.Transactions)
                .UsingEntity<Dictionary<string, object>>(
                    "TransactionTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TTags_Tag"),
                    l => l.HasOne<Transaction>().WithMany()
                        .HasForeignKey("TransactionId")
                        .HasConstraintName("FK_TTags_Transaction"),
                    j =>
                    {
                        j.HasKey("TransactionId", "TagId").HasName("PK__Transact__8314F5F14138767B");
                        j.ToTable("TransactionTags");
                    });
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transfer__3214EC07D19CB613");

            entity.HasIndex(e => new { e.WorkspaceId, e.TransferDate }, "IX_Transfers_Workspace_Date");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Fee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FromAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ToAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransferDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Transfers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfers_Users");

            entity.HasOne(d => d.FromWallet).WithMany(p => p.TransferFromWallets)
                .HasForeignKey(d => d.FromWalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfers_FromWallet");

            entity.HasOne(d => d.ToWallet).WithMany(p => p.TransferToWallets)
                .HasForeignKey(d => d.ToWalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfers_ToWallet");

            entity.HasOne(d => d.Workspace).WithMany(p => p.Transfers)
                .HasForeignKey(d => d.WorkspaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfers_Workspaces");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07DB0105D5");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534A3CEB12B").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BaseCurrencyCode)
                .HasMaxLength(10)
                .HasDefaultValue("VND");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.Language)
                .HasMaxLength(10)
                .HasDefaultValue("vi-VN");
            entity.Property(e => e.LastLoginAt).HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Theme)
                .HasMaxLength(20)
                .HasDefaultValue("LIGHT");
            entity.Property(e => e.Tier)
                .HasMaxLength(20)
                .HasDefaultValue("FREE");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.BaseCurrencyCodeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.BaseCurrencyCode)
                .HasConstraintName("FK_Users_Currencies");
        });

        modelBuilder.Entity<UserPreference>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserPref__1788CC4C04A51977");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.FirstDayOfWeek).HasDefaultValue(1);
            entity.Property(e => e.HideBalanceOnDashboard).HasDefaultValue(false);
            entity.Property(e => e.ReceiveEmailNewsletters).HasDefaultValue(true);
            entity.Property(e => e.ReceivePushNotifications).HasDefaultValue(true);

            entity.HasOne(d => d.User).WithOne(p => p.UserPreference)
                .HasForeignKey<UserPreference>(d => d.UserId)
                .HasConstraintName("FK_UserPrefs_Users");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserSess__3214EC07C57A5C69");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeviceName).HasMaxLength(100);
            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.IsRevoked).HasDefaultValue(false);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.RefreshToken).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.UserSessions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserSessions_Users");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallets__3214EC077953AC81");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Color).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreditLimit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(10)
                .HasDefaultValue("VND");
            entity.Property(e => e.CurrentBalance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.InitialBalance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsIncludedInTotal).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.WalletType).HasMaxLength(50);

            entity.HasOne(d => d.BankConnection).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.BankConnectionId)
                .HasConstraintName("FK_Wallets_BankConn");

            entity.HasOne(d => d.CurrencyCodeNavigation).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.CurrencyCode)
                .HasConstraintName("FK_Wallets_Currencies");

            entity.HasOne(d => d.Workspace).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.WorkspaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wallets_Workspaces");
        });

        modelBuilder.Entity<Workspace>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Workspac__3214EC0785F69750");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.WorkspaceType)
                .HasMaxLength(20)
                .HasDefaultValue("PERSONAL");

            entity.HasOne(d => d.Owner).WithMany(p => p.Workspaces)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Workspaces_Users");
        });

        modelBuilder.Entity<WorkspaceMember>(entity =>
        {
            entity.HasKey(e => new { e.WorkspaceId, e.UserId }).HasName("PK__Workspac__193FE915AE33FD4A");

            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("MEMBER");

            entity.HasOne(d => d.User).WithMany(p => p.WorkspaceMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WMembers_User");

            entity.HasOne(d => d.Workspace).WithMany(p => p.WorkspaceMembers)
                .HasForeignKey(d => d.WorkspaceId)
                .HasConstraintName("FK_WMembers_Workspace");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
