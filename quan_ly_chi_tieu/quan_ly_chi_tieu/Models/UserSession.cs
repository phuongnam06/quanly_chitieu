using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class UserSession
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string RefreshToken { get; set; } = null!;

    public string? DeviceName { get; set; }

    public string? IpAddress { get; set; }

    public string? Location { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool? IsRevoked { get; set; }

    public virtual User User { get; set; } = null!;
}
