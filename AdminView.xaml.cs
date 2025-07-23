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
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : UserControl, IDisposable
    {
        private readonly TaskManagerDb1Context _context;
        private bool _disposed;

        public AdminView()
        {
            _context = new TaskManagerDb1Context();
            InitializeComponent();
            LoadUsers();
            LoadProjects();
            LoadPayroll();
            LoadActivityLogs();
            LoadPerformanceReport();
            GenerateReport();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _context?.Dispose();
                _disposed = true;
            }
        }

        private void LoadUsers()
        {
            try
            {
                var users = _context.Users.ToList();
                UsersDataGrid.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadProjects()
        {
            try
            {
                var projects = _context.Projects.ToList();
                ProjectsDataGrid.ItemsSource = projects;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading projects: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPayroll()
        {
            try
            {
                var payroll = _context.Payrolls.Where(p => (bool)!p.Approved).ToList();
                PayrollDataGrid.ItemsSource = payroll;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payroll: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadActivityLogs(object sender = null, RoutedEventArgs e = null)
        {
            try
            {
                var logs = _context.ActivityLogs
                    .OrderByDescending(l => l.CreatedAt)
                    .Take(100)
                    .ToList();
                ActivityLogsDataGrid.ItemsSource = logs;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading activity logs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPerformanceReport(object sender = null, RoutedEventArgs e = null)
        {
            try
            {
                var metrics = _context.PerformanceMetrics
                    .Include(pm => pm.User)
                    .ToList();

                var report = metrics.Select(m => new
                {
                    User = m.User?.FullName ?? "Unknown",
                    QualityScore = m.QualityScore,
                    EvaluatedAt = m.CreatedAt
                }).ToList();

                PerformanceReportDataGrid.ItemsSource = report;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading performance report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateReport()
        {
            try
            {
                var totalUsers = _context.Users.Count();
                var totalTasks = _context.Tasks.Count();
                var completedTasks = _context.Tasks.Count(t => t.Status == "Done");
                var progress = totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0;
                ReportTextBlock.Text = $"Report (as of {DateTime.Now}): {totalUsers} users, {totalTasks} tasks, {progress:F2}% completed";
            }
            catch (Exception ex)
            {
                ReportTextBlock.Text = $"Error generating report: {ex.Message}";
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newUser = new User
                {
                    Username = "newuser",
                    PasswordHash = "hashedpassword", // Cần mã hóa thực tế
                    Email = "newuser@example.com",
                    FullName = "New User",
                    Role = "TeamMember",
                    BaseSalary = 1000.00m
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                LoadUsers();
                MessageBox.Show("User added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = UsersDataGrid.SelectedItem as User;
                if (user != null)
                {
                    // Giả lập chỉnh sửa (thực tế cần dialog hoặc form)
                    user.FullName = user.FullName; // Cần dialog để nhập dữ liệu mới
                    _context.SaveChanges();
                    LoadUsers();
                    MessageBox.Show("User edited successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please select a user to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = UsersDataGrid.SelectedItem as User;
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    LoadUsers();
                    MessageBox.Show("User deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please select a user to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApprovePayroll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var payroll = button?.DataContext as Payroll;
                if (payroll != null)
                {
                    payroll.Approved = true;
                    payroll.ApprovedBy = GetCurrentUserId();
                    payroll.ApprovedAt = DateTime.Now;
                    _context.SaveChanges();
                    LoadPayroll();
                    MessageBox.Show("Payroll approved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error approving payroll: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var createProjectWindow = new CreateProjectWindow();
                createProjectWindow.Owner = Window.GetWindow(this);
                createProjectWindow.ProjectCreated += (name, description, startDate, endDate) =>
                {
                    try
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            var newProject = new Project
                            {
                                Name = name,
                                Description = description,
                                StartDate = DateOnly.FromDateTime(startDate),
                                EndDate = endDate.HasValue ? DateOnly.FromDateTime(endDate.Value) : null,
                                CreatedBy = GetCurrentUserId(),
                                CreatedAt = DateTime.Now
                            };

                            _context.Projects.Add(newProject);
                            _context.SaveChanges();

                            var projectUserRole = new ProjectUserRole
                            {
                                ProjectId = newProject.ProjectId,
                                UserId = GetCurrentUserId(),
                                Role = "ProjectManager",
                                AssignedAt = DateTime.Now
                            };
                            _context.ProjectUserRoles.Add(projectUserRole);

                            var log = new ActivityLog
                            {
                                UserId = GetCurrentUserId(),
                                TaskId = null,
                                Action = "Create Project",
                                Description = $"Created project {newProject.Name}",
                                CreatedAt = DateTime.Now
                            };
                            _context.ActivityLogs.Add(log);

                            var notification = new Notification
                            {
                                UserId = GetCurrentUserId(),
                                TaskId = null,
                                Message = $"You have created project {newProject.Name} and been assigned as ProjectManager.",
                                IsRead = false,
                                CreatedAt = DateTime.Now
                            };
                            _context.Notifications.Add(notification);

                            _context.SaveChanges();
                            transaction.Commit();

                            LoadProjects();
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening create project window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentUser.Instance.Clear();
                Window mainWindow = Window.GetWindow(this);
                if (mainWindow is MainWindow mw)
                {
                    mw.ShowLoginForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetCurrentUserId()
        {
            var currentUser = CurrentUser.Instance.Current;
            return currentUser.UserId;
        }

        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var project = ProjectsDataGrid.SelectedItem as Project;
                if (project != null)
                {
                    project.Name = "Edited " + project.Name; // Cần dialog để nhập dữ liệu mới
                    _context.SaveChanges();
                    LoadProjects();
                    MessageBox.Show("Project edited successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please select a project to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var project = ProjectsDataGrid.SelectedItem as Project;
                if (project != null)
                {
                    _context.Projects.Remove(project);
                    _context.SaveChanges();
                    LoadProjects();
                    MessageBox.Show("Project deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please select a project to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AssignUserToProject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = UsersDataGrid.SelectedItem as User;
                var project = ProjectsDataGrid.SelectedItem as Project;
                if (user != null && project != null)
                {
                    var role = "TeamMember"; // Có thể lấy từ ComboBox hoặc dialog
                    var projectUserRole = new ProjectUserRole
                    {
                        ProjectId = project.ProjectId,
                        UserId = user.UserId,
                        Role = role,
                        AssignedAt = DateTime.Now
                    };
                    _context.ProjectUserRoles.Add(projectUserRole);
                    _context.SaveChanges();
                    MessageBox.Show($"Assigned {user.FullName} to project {project.Name} as {role}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please select both a user and a project.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error assigning user to project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddOrUpdatePayrollConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var config = _context.PayrollConfigs.FirstOrDefault();
                if (config == null)
                {
                    config = new PayrollConfig
                    {
                        ConfigName = "DefaultConfig",
                        ConfigValue = 0.1m,
                        Description = "Default payroll configuration",
                        CreatedAt = DateTime.Now
                    };
                    _context.PayrollConfigs.Add(config);
                }
                else
                {
                    config.ConfigValue = 0.15m;
                    config.UpdatedAt = DateTime.Now;
                }
                _context.SaveChanges();
                MessageBox.Show("Payroll config updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating payroll config: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}