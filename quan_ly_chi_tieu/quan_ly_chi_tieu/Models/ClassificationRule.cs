using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models;

public partial class ClassificationRule
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public string SearchKeyword { get; set; } = null!;

    public Guid TargetCategoryId { get; set; }

    public Guid? TargetTagId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category TargetCategory { get; set; } = null!;

    public virtual Workspace Workspace { get; set; } = null!;
}
