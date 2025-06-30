using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public partial class PayrollHistory
{
    public int HistoryId { get; set; }

    public int PayrollId { get; set; }

    public int UserId { get; set; }

    public DateOnly PayPeriod { get; set; }

    public decimal BaseSalary { get; set; }

    public decimal? Bonus { get; set; }

    public decimal? Deduction { get; set; }

    public decimal TotalSalary { get; set; }

    public string? ChangeDescription { get; set; }

    public int ChangedBy { get; set; }

    public DateTime? ChangedAt { get; set; }

    public virtual User ChangedByNavigation { get; set; } = null!;

    public virtual Payroll Payroll { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
