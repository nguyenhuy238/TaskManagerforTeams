using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models;

public partial class TaskManagerDb1Context : DbContext
{
    public TaskManagerDb1Context()
    {
    }

    public TaskManagerDb1Context(DbContextOptions<TaskManagerDb1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payroll> Payrolls { get; set; }

    public virtual DbSet<PayrollConfig> PayrollConfigs { get; set; }

    public virtual DbSet<PayrollHistory> PayrollHistories { get; set; }

    public virtual DbSet<PerformanceMetric> PerformanceMetrics { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectUserRole> ProjectUserRoles { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskComment> TaskComments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=TaskManagerDB1;UId=sa;pwd=123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Activity__5E54864823C50553");

            entity.HasIndex(e => e.UserId, "IX_ActivityLogs_UserId");

            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasOne(d => d.Task).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__ActivityL__TaskI__5BE2A6F2");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ActivityL__UserI__5AEE82B9");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E12CCAD5CE5");

            entity.HasIndex(e => e.UserId, "IX_Notifications_UserId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Message).HasMaxLength(500);

            entity.HasOne(d => d.Task).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK__Notificat__TaskI__571DF1D5");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__5629CD9C");
        });

        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.HasKey(e => e.PayrollId).HasName("PK__Payroll__99DFC672A794FA78");

            entity.ToTable("Payroll");

            entity.HasIndex(e => e.ApprovedBy, "IX_Payroll_ApprovedBy");

            entity.HasIndex(e => new { e.UserId, e.PayPeriod }, "IX_Payroll_UserId_PayPeriod");

            entity.HasIndex(e => new { e.UserId, e.PayPeriod }, "UQ_Payroll_UserId_PayPeriod").IsUnique();

            entity.Property(e => e.Approved).HasDefaultValue(false);
            entity.Property(e => e.ApprovedAt).HasColumnType("datetime");
            entity.Property(e => e.BaseSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Bonus)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deduction)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.PayrollApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__Payroll__Approve__6E01572D");

            entity.HasOne(d => d.User).WithMany(p => p.PayrollUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payroll__UserId__6D0D32F4");
        });

        modelBuilder.Entity<PayrollConfig>(entity =>
        {
            entity.HasKey(e => e.ConfigId).HasName("PK__PayrollC__C3BC335C0C3C342B");

            entity.ToTable("PayrollConfig");

            entity.HasIndex(e => e.ConfigName, "UQ__PayrollC__A89A7DB291AFEAF7").IsUnique();

            entity.Property(e => e.ConfigName).HasMaxLength(50);
            entity.Property(e => e.ConfigValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<PayrollHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__PayrollH__4D7B4ABDDB48ADCF");

            entity.ToTable("PayrollHistory");

            entity.HasIndex(e => e.PayrollId, "IX_PayrollHistory_PayrollId");

            entity.Property(e => e.BaseSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Bonus)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ChangeDescription).HasMaxLength(500);
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deduction)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalSalary).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.PayrollHistoryChangedByNavigations)
                .HasForeignKey(d => d.ChangedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PayrollHi__Chang__75A278F5");

            entity.HasOne(d => d.Payroll).WithMany(p => p.PayrollHistories)
                .HasForeignKey(d => d.PayrollId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PayrollHi__Payro__73BA3083");

            entity.HasOne(d => d.User).WithMany(p => p.PayrollHistoryUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PayrollHi__UserI__74AE54BC");
        });

        modelBuilder.Entity<PerformanceMetric>(entity =>
        {
            entity.HasKey(e => e.MetricId).HasName("PK__Performa__561056A54830A0F2");

            entity.HasIndex(e => new { e.UserId, e.PayPeriod }, "IX_PerformanceMetrics_UserId_PayPeriod");

            entity.HasIndex(e => new { e.UserId, e.PayPeriod }, "UQ_PerformanceMetrics_UserId_PayPeriod").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OnTimeRate)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.QualityScore)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TasksCompleted).HasDefaultValue(0);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.PerformanceMetrics)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Performan__UserI__6477ECF3");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Projects__761ABEF06BDB1F24");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Projects__Create__3F466844");
        });

        modelBuilder.Entity<ProjectUserRole>(entity =>
        {
            entity.HasKey(e => new { e.ProjectId, e.UserId }).HasName("PK__ProjectU__A76232348984444C");

            entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Role).HasMaxLength(20);

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectUserRoles)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectUs__Proje__440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectUserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectUs__UserI__44FF419A");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tasks__7C6949B1A6236C42");

            entity.HasIndex(e => e.AssignedTo, "IX_Tasks_AssignedTo");

            entity.HasIndex(e => e.ProjectId, "IX_Tasks_ProjectId");

            entity.Property(e => e.CompletedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Priority).HasMaxLength(10);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.TaskAssignedToNavigations)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK__Tasks__AssignedT__4BAC3F29");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tasks__CreatedBy__4CA06362");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tasks__ProjectId__4AB81AF0");
        });

        modelBuilder.Entity<TaskComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__TaskComm__C3B4DFCA217C8DB9");

            entity.HasIndex(e => e.TaskId, "IX_TaskComments_TaskId");

            entity.Property(e => e.AttachmentUrl).HasMaxLength(255);
            entity.Property(e => e.CommentText).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskComments)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaskComme__TaskI__5070F446");

            entity.HasOne(d => d.User).WithMany(p => p.TaskComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaskComme__UserI__5165187F");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C7F6FA7A4");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E49DE34769").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105341DFDD39F").IsUnique();

            entity.Property(e => e.BaseSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
