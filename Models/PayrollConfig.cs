using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public partial class PayrollConfig
{
    public int ConfigId { get; set; }

    public string ConfigName { get; set; } = null!;

    public decimal ConfigValue { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
