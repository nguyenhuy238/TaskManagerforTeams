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
            // Lấy ID của người dùng hiện tại
            int currentUserId = GetCurrentUserId();

            // Lấy danh sách ProjectId mà người dùng có vai trò ProjectManager
            var managedProjectIds = _context.ProjectUserRoles
                .Where(pur => pur.UserId == currentUserId && pur.Role == "ProjectManager")
                .Select(pur => pur.ProjectId)
                .ToList();

            // Lấy các nhiệm vụ thuộc các dự án mà người dùng quản lý
            var tasks = _context.Tasks
                .Where(t => managedProjectIds.Contains(t.ProjectId))
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

            TasksDataGrid.ItemsSource = tasks; // Gán dữ liệu vào DataGrid
        }

        private void LoadProgressReport()
        {
            // Lấy ID của người dùng hiện tại
            int currentUserId = GetCurrentUserId();

            // Lấy danh sách ProjectId mà người dùng có vai trò ProjectManager
            var managedProjectIds = _context.ProjectUserRoles
                .Where(pur => pur.UserId == currentUserId && pur.Role == "ProjectManager")
                .Select(pur => pur.ProjectId)
                .ToList();

            // Tính tiến độ dựa trên các nhiệm vụ thuộc dự án của người dùng
            var totalTasks = _context.Tasks
                .Where(t => managedProjectIds.Contains(t.ProjectId))
                .Count();
            var completedTasks = _context.Tasks
                .Where(t => managedProjectIds.Contains(t.ProjectId) && t.Status == "Done")
                .Count();
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

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                CurrentUser.Instance.Clear();
                Window mainWindow = Window.GetWindow(this);
                if (mainWindow is MainWindow mw)
                {
                    mw.ShowLoginForm();
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

        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            // Mở CreateProjectWindow
            var createProjectWindow = new CreateProjectWindow();
            createProjectWindow.Owner = Window.GetWindow(this);
            createProjectWindow.ProjectCreated += (name, description, startDate, endDate) =>
            {
                try
                {
                    // Bắt đầu transaction để đảm bảo toàn vẹn dữ liệu
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        // Tạo dự án mới
                        var newProject = new Project
                        {
                            Name = name,
                            Description = description,
                            StartDate = DateOnly.FromDateTime(startDate), // Convert DateTime to DateOnly
                            EndDate = endDate.HasValue ? DateOnly.FromDateTime(endDate.Value) : null, // Handle nullable DateTime
                            CreatedBy = GetCurrentUserId(),
                            CreatedAt = DateTime.Now
                        };

                        // Lưu dự án vào bảng Projects
                        _context.Projects.Add(newProject);
                        _context.SaveChanges(); // Lưu để lấy ProjectId

                        // Gán vai trò ProjectManager cho người tạo trong ProjectUserRoles
                        var projectUserRole = new ProjectUserRole
                        {
                            ProjectId = newProject.ProjectId,
                            UserId = GetCurrentUserId(),
                            Role = "ProjectManager",
                            AssignedAt = DateTime.Now
                        };

                        _context.ProjectUserRoles.Add(projectUserRole);
                        _context.SaveChanges();

                        // Commit transaction
                        transaction.Commit();

                        MessageBox.Show("Project created and role assigned successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating project or assigning role: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            createProjectWindow.ShowDialog();
        }

        int GetCurrentUserId()
        {
            var currentUser = CurrentUser.Instance.Current;
            
                return currentUser.UserId;
            
            
        }

      
    }
}