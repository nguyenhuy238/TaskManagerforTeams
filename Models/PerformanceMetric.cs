using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public partial class PerformanceMetric
{
    public int MetricId { get; set; }

    public int UserId { get; set; }

    public DateOnly PayPeriod { get; set; }

    public int? TasksCompleted { get; set; }

    public decimal? OnTimeRate { get; set; }

    public decimal? QualityScore { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
