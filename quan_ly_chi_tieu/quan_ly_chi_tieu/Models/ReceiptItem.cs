using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class ReceiptItem
{
    public Guid Id { get; set; }

    public Guid TransactionId { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal? Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal? Discount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;
}
