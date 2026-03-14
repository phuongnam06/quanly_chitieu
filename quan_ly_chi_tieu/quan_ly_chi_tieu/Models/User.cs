using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string? FullName { get; set; }

    public string? AvatarUrl { get; set; }

    public string? BaseCurrencyCode { get; set; }

    public string? Language { get; set; }

    public string? Theme { get; set; }

    public string? Tier { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsVerified { get; set; }

    public virtual ICollection<BankConnection> BankConnections { get; set; } = new List<BankConnection>();

    public virtual Currency? BaseCurrencyCodeNavigation { get; set; }

    public virtual ICollection<BillSplitParticipant> BillSplitParticipants { get; set; } = new List<BillSplitParticipant>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();

    public virtual UserPreference? UserPreference { get; set; }

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();

    public virtual ICollection<WorkspaceMember> WorkspaceMembers { get; set; } = new List<WorkspaceMember>();

    public virtual ICollection<Workspace> Workspaces { get; set; } = new List<Workspace>();
}
