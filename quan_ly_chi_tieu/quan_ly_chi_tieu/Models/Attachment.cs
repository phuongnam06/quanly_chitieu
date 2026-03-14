using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class Attachment
{
    public Guid Id { get; set; }

    public Guid TransactionId { get; set; }

    public string FileUrl { get; set; } = null!;

    public string? FileName { get; set; }

    public string? FileType { get; set; }

    public int? FileSize { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;
}
