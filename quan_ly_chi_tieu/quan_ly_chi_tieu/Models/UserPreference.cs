using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class UserPreference
{
    public Guid UserId { get; set; }

    public bool? ReceiveEmailNewsletters { get; set; }

    public bool? ReceivePushNotifications { get; set; }

    public bool? HideBalanceOnDashboard { get; set; }

    public int? FirstDayOfWeek { get; set; }

    public virtual User User { get; set; } = null!;
}
