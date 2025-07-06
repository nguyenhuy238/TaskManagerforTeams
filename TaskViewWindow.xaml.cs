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
        public event Action TasksUpdated; // Thêm sự kiện TasksUpdated
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
            }
        }

        private void LoadTasks()
        {
            var tasks = _context.Tasks
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
                    t.DueDate
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
                        dbTask.AssignedTo = userId.Value;
                        _context.SaveChanges();
                        LoadTasks();
                        TasksUpdated?.Invoke(); // Kích hoạt sự kiện
                        MessageBox.Show("Task assigned successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                var newStatus = ShowStatusSelectionDialog();
                if (!string.IsNullOrEmpty(newStatus))
                {
                    var dbTask = _context.Tasks.Find(task.TaskId);
                    if (dbTask != null)
                    {
                        dbTask.Status = newStatus;
                        _context.SaveChanges();
                        LoadTasks();
                        MessageBox.Show("Task status updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                LoadTasks(); // Làm mới danh sách Task sau khi tạo
                MessageBox.Show("Task created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            };
            createTaskWindow.ShowDialog();
        }
        private int? GetUserIdFromSelection()
        {
            // Fetch the list of TeamMembers for the project
            var teamMembers = _context.ProjectUserRoles
                .Where(pur => pur.ProjectId == _projectId && pur.Role == "TeamMember")
                .Join(_context.Users,
                    pur => pur.UserId,
                    u => u.UserId,
                    (pur, u) => new { u.UserId, u.FullName })
                .ToList();

            if (!teamMembers.Any())
            {
                MessageBox.Show("No Team Members available in this project.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            // Fetch performance metrics and task counts
            var teamMembersWithMetrics = teamMembers.Select(tm => new
            {
                tm.UserId,
                tm.FullName,
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
            .OrderByDescending(tm => tm.OnTimeRate) // Prioritize high OnTimeRate
            .ThenByDescending(tm => tm.QualityScore) // Then QualityScore
            .ThenBy(tm => tm.TaskCount) // Finally, fewer InProgress tasks
            .ToList();

            // Create dialog to select TeamMember
            var dialog = new Window
            {
                Title = "Select Team Member",
                Width = 400,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            var comboBox = new ComboBox
            {
                ItemsSource = teamMembersWithMetrics,
                Margin = new Thickness(10),
                DisplayMemberPath = "FullName"
            };
            // Display additional information when selecting
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
                    var selectedMember = (dynamic)selected; // Explicit cast to dynamic
                    infoTextBlock.Text = $"Tasks In Progress: {selectedMember.TaskCount}\n" +
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
                    dialog.Tag = ((dynamic)comboBox.SelectedItem).UserId; // Explicit cast to dynamic
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
            // Placeholder: Cần triển khai giao diện để chọn trạng thái
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
    }
}
