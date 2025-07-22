using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskManager.Models;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for TaskViewWindow.xaml
    /// </summary>
    public partial class TaskViewWindow : Window
    {
        private readonly TaskManagerDb1Context _context;
        private readonly int _projectId;
        public event Action TasksUpdated;

        public TaskViewWindow(int projectId)
        {
            InitializeComponent();
            _context = new TaskManagerDb1Context();
            _projectId = projectId;
            LoadProjectInfo();
            LoadTasks();
        }

        private void LoadProjectInfo()
        {
            var project = _context.Projects.Find(_projectId);
            if (project != null)
            {
                ProjectTitle.Text = $"Tasks for Project: {project.Name}";
                var tasks = _context.Tasks.Where(t => t.ProjectId == _projectId).ToList();
                var totalTasks = tasks.Count;
                var completedTasks = tasks.Count(t => t.Status == "Done");
                var onTimeTasks = tasks.Count(t => t.Status == "Done" && t.CompletedAt.HasValue && t.DueDate.HasValue &&
                    t.CompletedAt.Value <= t.DueDate.Value.ToDateTime(TimeOnly.MaxValue));
                var lateTasks = tasks.Count(t => t.Status == "Done" && t.CompletedAt.HasValue && t.DueDate.HasValue &&
                    t.CompletedAt.Value > t.DueDate.Value.ToDateTime(TimeOnly.MaxValue));

                var progress = totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0;
                var onTimeRate = totalTasks > 0 ? (double)onTimeTasks / completedTasks * 100 : 0;
                ProgressText.Text = $"{progress:F2}% ({completedTasks}/{totalTasks} tasks)";
                OnTimeRateText.Text = $"{onTimeRate:F2}%";
                LateTasksText.Text = lateTasks.ToString();
            }
        }

        private void LoadTasks()
        {
            var tasks = _context.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectId == _projectId)
                .Select(t => new
                {
                    t.TaskId,
                    t.Title,
                    t.Status,
                    AssignedToName = _context.Users
                        .Where(u => u.UserId == t.AssignedTo)
                        .Select(u => u.FullName)
                        .FirstOrDefault() ?? "Unassigned",
                    t.DueDate,
                    OnTimeStatus = t.Status == "Done" && t.CompletedAt.HasValue && t.DueDate.HasValue
                        ? (t.CompletedAt.Value <= t.DueDate.Value.ToDateTime(TimeOnly.MaxValue) ? "On-Time" : "Late")
                        : (t.DueDate.HasValue ? "Pending" : "N/A")
                })
                .ToList();

            TasksDataGrid.ItemsSource = tasks;
        }

        private void AssignTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as dynamic;
            if (task != null)
            {
                var userId = GetUserIdFromSelection();
                if (userId.HasValue)
                {
                    var dbTask = _context.Tasks.Find(task.TaskId);
                    if (dbTask != null)
                    {
                        int currentUserId = GetCurrentUserId();
                        var isProjectManager = _context.ProjectUserRoles
                            .Any(pur => pur.ProjectId == _projectId && pur.UserId == currentUserId && pur.Role == "ProjectManager");
                        if (!isProjectManager)
                        {
                            MessageBox.Show("You do not have permission to assign tasks.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            dbTask.AssignedTo = userId.Value;
                            var log = new ActivityLog
                            {
                                UserId = currentUserId,
                                TaskId = dbTask.TaskId,
                                Action = "AssignTask",
                                Description = $"Task '{dbTask.Title}' assigned to user {userId.Value}",
                                CreatedAt = DateTime.Now
                            };
                            _context.ActivityLogs.Add(log);

                            var notification = new Notification
                            {
                                UserId = userId.Value,
                                TaskId = dbTask.TaskId,
                                Message = $"You have been assigned to task '{dbTask.Title}' in project '{_context.Projects.Find(_projectId)?.Name ?? "Unknown"}'",
                                IsRead = false,
                                CreatedAt = DateTime.Now
                            };
                            _context.Notifications.Add(notification);

                            _context.SaveChanges();
                            transaction.Commit();

                            LoadTasks();
                            LoadProjectInfo();
                            TasksUpdated?.Invoke();
                            MessageBox.Show("Task assigned successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Task not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a user to assign the task.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void UpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as dynamic;
            if (task != null)
            {
                int currentUserId = GetCurrentUserId();
                var isProjectManager = _context.ProjectUserRoles
                    .Any(pur => pur.ProjectId == _projectId && pur.UserId == currentUserId && pur.Role == "ProjectManager");
                if (!isProjectManager)
                {
                    MessageBox.Show("You do not have permission to update task status.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newStatus = ShowStatusSelectionDialog();
                if (!string.IsNullOrEmpty(newStatus))
                {
                    using (var transaction = _context.Database.BeginTransaction()) // Declare 'transaction' here
                    {
                        try
                        {
                            var dbTask = _context.Tasks.Find(task.TaskId);
                            if (dbTask == null)
                            {
                                MessageBox.Show("Task not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            var confirm = MessageBox.Show(
                                $"Are you sure you want to change the status of '{dbTask.Title}' to {newStatus}?",
                                "Confirm",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question
                            );
                            if (confirm != MessageBoxResult.Yes)
                                return;

                            if (!new[] { "ToDo", "InProgress", "Done" }.Contains(newStatus))
                            {
                                MessageBox.Show("Invalid status selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            dbTask.Status = newStatus;
                            dbTask.CompletedAt = newStatus == "Done" ? (DateTime?)DateTime.Now : null;

                            var log = new ActivityLog
                            {
                                UserId = currentUserId,
                                TaskId = dbTask.TaskId,
                                Action = "UpdateTaskStatus",
                                Description = $"Task '{dbTask.Title}' status changed to {newStatus}",
                                CreatedAt = DateTime.Now
                            };
                            _context.ActivityLogs.Add(log);

                            if (dbTask.AssignedTo.HasValue)
                            {
                                var project = _context.Projects.Find(_projectId);
                                if (project == null)
                                {
                                    MessageBox.Show("Project not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                var notification = new Notification
                                {
                                    UserId = dbTask.AssignedTo.Value,
                                    TaskId = dbTask.TaskId,
                                    Message = $"Task '{dbTask.Title}' status updated to {newStatus} in project '{project.Name}'",
                                    IsRead = false,
                                    CreatedAt = DateTime.Now
                                };
                                _context.Notifications.Add(notification);
                            }

                            _context.SaveChanges();
                            transaction.Commit();

                            LoadTasks();
                            LoadProjectInfo();
                            TasksUpdated?.Invoke();
                            MessageBox.Show("Task status updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (DbUpdateException ex)
                        {
                            transaction.Rollback(); // 'transaction' is now in scope
                            MessageBox.Show($"Database error: {ex.InnerException?.Message ?? ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); // 'transaction' is now in scope
                            MessageBox.Show($"Error updating task status: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            var createTaskWindow = new CreateTaskWindow(_projectId);
            createTaskWindow.Owner = this;
            createTaskWindow.TaskCreated += () =>
            {
                LoadTasks();
                LoadProjectInfo();
                MessageBox.Show("Task created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            };
            createTaskWindow.ShowDialog();
        }

        private int? GetUserIdFromSelection()
        {
            var teamMembers = _context.ProjectUserRoles
                .Where(pur => pur.ProjectId == _projectId && pur.Role == "TeamMember")
                .Join(_context.Users,
                    pur => pur.UserId,
                    u => u.UserId,
                    (pur, u) => new
                    {
                        u.UserId,
                        u.FullName,
                        u.Skills
                    })
                .ToList();

            if (!teamMembers.Any())
            {
                MessageBox.Show("No Team Members available in this project.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            var teamMembersWithMetrics = teamMembers.Select(tm => new
            {
                tm.UserId,
                tm.FullName,
                tm.Skills,
                TaskCount = _context.Tasks.Count(t => t.AssignedTo == tm.UserId && t.Status == "InProgress"),
                OnTimeRate = _context.PerformanceMetrics
                    .Where(pm => pm.UserId == tm.UserId)
                    .OrderByDescending(pm => pm.PayPeriod)
                    .Select(pm => pm.OnTimeRate)
                    .FirstOrDefault(),
                QualityScore = _context.PerformanceMetrics
                    .Where(pm => pm.UserId == tm.UserId)
                    .OrderByDescending(pm => pm.PayPeriod)
                    .Select(pm => pm.QualityScore)
                    .FirstOrDefault()
            })
            .OrderByDescending(tm => tm.OnTimeRate)
            .ThenByDescending(tm => tm.QualityScore)
            .ThenBy(tm => tm.TaskCount)
            .ToList();

            var dialog = new Window
            {
                Title = "Select Team Member",
                Width = 400,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            var comboBox = new ComboBox
            {
                ItemsSource = teamMembersWithMetrics,
                Margin = new Thickness(10),
                DisplayMemberPath = "FullName"
            };
            var infoTextBlock = new TextBlock
            {
                Margin = new Thickness(10, 5, 10, 5),
                TextWrapping = TextWrapping.Wrap
            };
            comboBox.SelectionChanged += (s, args) =>
            {
                var selected = comboBox.SelectedItem;
                if (selected != null)
                {
                    var selectedMember = (dynamic)selected;
                    infoTextBlock.Text = $"Skills: {selectedMember.Skills ?? "None"}\n" +
                                         $"Tasks In Progress: {selectedMember.TaskCount}\n" +
                                         $"On-Time Rate: {selectedMember.OnTimeRate:F2}%\n" +
                                         $"Quality Score: {selectedMember.QualityScore:F2}";
                }
            };
            var btnOk = new Button
            {
                Content = "OK",
                Width = 80,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            btnOk.Click += (s, args) =>
            {
                if (comboBox.SelectedItem != null)
                {
                    dialog.Tag = ((dynamic)comboBox.SelectedItem).UserId;
                    dialog.Close();
                }
                else
                {
                    MessageBox.Show("Please select a team member.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };
            var stackPanel = new StackPanel { Margin = new Thickness(10) };
            stackPanel.Children.Add(comboBox);
            stackPanel.Children.Add(infoTextBlock);
            stackPanel.Children.Add(btnOk);
            dialog.Content = stackPanel;
            dialog.ShowDialog();
            return dialog.Tag as int?;
        }

        private string ShowStatusSelectionDialog()
        {
            var statuses = new[] { "ToDo", "InProgress", "Done" };
            var dialog = new Window
            {
                Title = "Select Status",
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            var comboBox = new ComboBox
            {
                ItemsSource = statuses,
                Margin = new Thickness(10)
            };
            var btnOk = new Button
            {
                Content = "OK",
                Width = 80,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            btnOk.Click += (s, args) =>
            {
                if (comboBox.SelectedItem != null)
                {
                    dialog.Tag = comboBox.SelectedItem.ToString();
                    dialog.Close();
                }
                else
                {
                    MessageBox.Show("Please select a status.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };
            var stackPanel = new StackPanel { Margin = new Thickness(10) };
            stackPanel.Children.Add(comboBox);
            stackPanel.Children.Add(btnOk);
            dialog.Content = stackPanel;
            dialog.ShowDialog();
            return dialog.Tag as string;
        }

        private int GetCurrentUserId()
        {
            var currentUser = CurrentUser.Instance.Current;
            return currentUser.UserId;
        }
    }
}
