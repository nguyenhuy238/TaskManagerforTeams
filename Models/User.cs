using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public decimal BaseSalary { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Payroll> PayrollApprovedByNavigations { get; set; } = new List<Payroll>();

    public virtual ICollection<PayrollHistory> PayrollHistoryChangedByNavigations { get; set; } = new List<PayrollHistory>();

    public virtual ICollection<PayrollHistory> PayrollHistoryUsers { get; set; } = new List<PayrollHistory>();

    public virtual ICollection<Payroll> PayrollUsers { get; set; } = new List<Payroll>();

    public virtual ICollection<PerformanceMetric> PerformanceMetrics { get; set; } = new List<PerformanceMetric>();

    public virtual ICollection<ProjectUserRole> ProjectUserRoles { get; set; } = new List<ProjectUserRole>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<Task> TaskAssignedToNavigations { get; set; } = new List<Task>();

    public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();

    public virtual ICollection<Task> TaskCreatedByNavigations { get; set; } = new List<Task>();
}
