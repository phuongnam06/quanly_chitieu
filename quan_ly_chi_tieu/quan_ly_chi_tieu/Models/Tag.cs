using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class Tag
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public string Name { get; set; } = null!;

    public string? Color { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Workspace Workspace { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
