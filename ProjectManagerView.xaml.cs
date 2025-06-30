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
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for ProjectManagerView.xaml
    /// </summary>
    public partial class ProjectManagerView : UserControl
    {
        private readonly TaskManagerDb1Context _context;

        public ProjectManagerView()
        {
            InitializeComponent();
            _context = new TaskManagerDb1Context();
            LoadTasks();
            LoadProgressReport();
        }

        private void LoadTasks()
        {
            var tasks = _context.Tasks
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
                }).ToList();
            TasksDataGrid.ItemsSource = tasks; // Gán dữ liệu vào DataGrid
        }

        private void LoadProgressReport()
        {
            var totalTasks = _context.Tasks.Count();
            var completedTasks = _context.Tasks.Count(t => t.Status == "Done");
            var progress = totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0;
            ProgressReportText.Text = $"{progress:F2}% completed ({completedTasks}/{totalTasks} tasks)";
        }

        private void AssignTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as dynamic;
            if (task != null)
            {
                var userId = GetUserIdFromSelection(); // Logic để chọn người nhận nhiệm vụ (cần triển khai thêm)
                if (userId.HasValue)
                {
                    var dbTask = _context.Tasks.Find(task.TaskId);
                    if (dbTask != null)
                    {
                        dbTask.AssignedTo = userId.Value;
                        _context.SaveChanges();
                        LoadTasks();
                        MessageBox.Show("Task assigned successfully!");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a user to assign the task.");
                }
            }
        }

        private void UpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as dynamic;
            if (task != null)
            {
                var newStatus = ShowStatusSelectionDialog(); // Hiển thị dialog để chọn trạng thái mới
                if (!string.IsNullOrEmpty(newStatus))
                {
                    var dbTask = _context.Tasks.Find(task.TaskId);
                    if (dbTask != null)
                    {
                        dbTask.Status = newStatus;
                        _context.SaveChanges();
                        LoadTasks();
                        MessageBox.Show("Task status updated successfully!");
                    }
                }
            }
        }

        private int? GetUserIdFromSelection()
        {
            // Placeholder: Thay bằng logic thực tế (ví dụ: ComboBox để chọn người dùng)
            // Ví dụ: Lấy từ danh sách người dùng trong cơ sở dữ liệu
            var users = _context.Users.ToList();
            // Cần thêm giao diện UI (ComboBox) để chọn người dùng
            return users.Any() ? users.First().UserId : (int?)null; // Trả về UserId đầu tiên nếu có
        }

        private string ShowStatusSelectionDialog()
        {
            // Placeholder: Hiển thị dialog để chọn trạng thái (ToDo, InProgress, Done)
            var statuses = new[] { "ToDo", "InProgress", "Done" };
            // Cần thêm giao diện UI (ComboBox hoặc dialog) để chọn trạng thái
            return statuses[0]; // Giả lập trả về "ToDo"
        }
    }
}