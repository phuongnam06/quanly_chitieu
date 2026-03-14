using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class BillSplit
{
    public Guid Id { get; set; }

    public Guid TransactionId { get; set; }

    public string? SplitType { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BillSplitParticipant> BillSplitParticipants { get; set; } = new List<BillSplitParticipant>();

    public virtual Transaction Transaction { get; set; } = null!;
}
