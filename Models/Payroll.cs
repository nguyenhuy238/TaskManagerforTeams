using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public partial class Payroll
{
    public int PayrollId { get; set; }

    public int UserId { get; set; }

    public DateOnly PayPeriod { get; set; }

    public decimal BaseSalary { get; set; }

    public decimal? Bonus { get; set; }

    public decimal? Deduction { get; set; }

    public decimal TotalSalary { get; set; }

    public bool? Approved { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual ICollection<PayrollHistory> PayrollHistories { get; set; } = new List<PayrollHistory>();

    public virtual User User { get; set; } = null!;
}
