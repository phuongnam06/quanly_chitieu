using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class BillSplitParticipant
{
    public Guid BillSplitId { get; set; }

    public Guid UserId { get; set; }

    public decimal OwedAmount { get; set; }

    public decimal? PaidAmount { get; set; }

    public string? Status { get; set; }

    public virtual BillSplit BillSplit { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
