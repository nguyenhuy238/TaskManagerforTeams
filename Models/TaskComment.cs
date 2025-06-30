using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public partial class TaskComment
{
    public int CommentId { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string? CommentText { get; set; }

    public string? AttachmentUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Task Task { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
