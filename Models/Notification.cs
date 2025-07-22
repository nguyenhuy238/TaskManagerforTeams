using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public int? TaskId { get; set; }

    public string Message { get; set; } = null!;

    public bool? IsRead { get; set; } = false;

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public virtual Task? Task { get; set; }

    public virtual User User { get; set; } = null!;

    public string RequestedStatus { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
}
