using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public partial class ProjectUserRole
{
    public int ProjectId { get; set; }

    public int UserId { get; set; }

    public string Role { get; set; } = null!;

    public DateTime? AssignedAt { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
