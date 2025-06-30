using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<ProjectUserRole> ProjectUserRoles { get; set; } = new List<ProjectUserRole>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
