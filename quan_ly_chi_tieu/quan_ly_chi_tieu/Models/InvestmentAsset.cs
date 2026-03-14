using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class InvestmentAsset
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public string AssetType { get; set; } = null!;

    public string Symbol { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal? CurrentPrice { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<InvestmentTransaction> InvestmentTransactions { get; set; } = new List<InvestmentTransaction>();

    public virtual Workspace Workspace { get; set; } = null!;
}
