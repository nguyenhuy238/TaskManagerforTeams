   M S B A M L   `   `   `  ����  TaskManager    TaskManager.AdminView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      .��5	      �   � RowDefinitions  5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   .+�5      �� $��Admin Dashboard��6   $��24(�5      $
��Bold"�5      $��Center=�5      $��Center=�5      $
��0,10q�5      �� 5       �� Orientation$ 
Horizontal=�6   $��Right=�62   $��	0,10,10,0q�6N   .+�5      �� -   5         ��Name$ 	btnLogout��5      $��Logout��5      $	��100��5      $��30��5      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 5��5      5      5      5       �� 5#   
   $��1��6   $��10q�6"   .@�5$      �� $��User Management��6   .��5%      �   5&      �� 5'      $��* �6(   �� 5(      $
��Auto �6(   .+�5*      &   System.Windows.Controls.DataGrid  -   5          Name$ UsersDataGrid��5*         AutoGenerateColumns$ False��5+      $��0,0,0,10q�5,         Columns 5-      0  *System.Windows.Controls.DataGridTextColumn  5.      ,  &System.Windows.Controls.DataGridColumn   Header$ User ID��61   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   �� *UserId+  5/      $ Username��61    6C   �� *
Username+  50      $ 	Full Name��61    6D   �� *
FullName+  51      $
 Role��61    6?   �� *Role+4  .System.Windows.Controls.DataGridTemplateColumn  52      	   CellTemplate	 53   "   �� 54   &   .R�55   *   �� $ 
Horizontal=�65   .+�56   .   �� -   5        $
��Edit��56   5   $	��5,0q�6[   �� 57   .   -   5        $��Delete��57   5   $	��5,0q�6_   58   +   59   '   5;      5=      �� 5>      $��1��6!   $ 
Horizontal=�6.   .+�5?      �� -   5        $��Add User��5?   !   $��0,0,10,0q�6J   �� 5@      -   5        $��Assign User to Project��5@   !   $��0,0,10,0q�6d   5A      5B      5C      �� 5D      $��Project Overview��6   .��5E      � 
 � ColumnDefinitions
 5F      �� 5G      $	��58* �6+   �� 5H      $	��87* �6+   �� 5I      $
��629* �6+     5K      �� 5L      $
��Auto �6(   �� 5M      $��* �6(   �� 5N      $
��Auto �6(   .+�5P      �� -   5        $ btnCreateProject��5P      $��Create Project��5Q      $	��120��5R      $��30��5S      $ 5��5T      $��	10,0,0,10q�5V      $
��Left=�5W      $��2��65   �� 5X      $��0��6!   $��1��6.   $ 
Horizontal=�6>   $
��Left=�6W   $��
140,0,0,10q�6r   .+�5Y      �� -   5        $��Edit Project��5Y   !   $��0,0,10,0q�6R   �� 5Z      -	   5        $��Delete Project��5Z   !   $��0,0,10,0q�6V   5[        5\      -
   5        $ ProjectsDataGrid��5\      $��1��5]      $ False��5^      $��10,10,10,10q�5_      $��3��61    5`        5a      $ 
Project ID��61    6E   �� *	ProjectId+  5b      $
 Name��61    6?   �� *Name+  5c      $ 
Start Date��61    6E   �� *	StartDate+  5d      $ End Date��61    6C   �� *	EndDate+5f      5g      5h      �� 5i      $��Payroll Config��6   .��5j      �� $��10q�6   .+�5k      �� $"��Payroll Bonus/Penalty Config��6    $
��Bold"�6D   $��0,0,0,10q�6V   �� 5l      -   5        $��Update Payroll Config��5l      $	��180��6d   $��0,0,0,10q�6p   5m      5n      �� 5o      $��Activity Logs��6   .��5p      �� $��10q�6   .+�5q      �� -   5        $��Load Activity Logs��5q      $	��160��6S   $��0,0,0,10q�6_     5r      -   5        $ ActivityLogsDataGrid��5r      $ False��6=   $	��200��6Y    5s        5t      $ Log ID��61    6A   �� *LogId+  5u      $ User ID��61    6B   �� *UserId+  5v      $ Action��61    6A   �� *Action+  5w      $ Description��61    6F   �� *Description+  5x      $ 
Created At��61    6E   �� *	CreatedAt+5z      5{      5|      �� 5}      $��Performance Report��6   .��5~      �� $��10q�6   .+�5      �� -   5        $��Load Performance Report��5      $	��180��6]   $��0,0,0,10q�6i     5�      -   5        $ PerformanceReportDataGrid��5�      $ False��6B   $	��200��6^    5�        5�      $
 User��61    6?   �� *User+  5�      $ Quality Score��61    6H   �� *QualityScore+  5�      $ Evaluated At��61    6G   �� *EvaluatedAt+5�      5�      5�      5�      �� 5�   
   $��2��6   $��10q�6"   .+�5�      �� $��Payroll Approval��6   $
��Bold"�60   $��0,0,0,5q�6B     5�      -   5        $ PayrollDataGrid��5�      $ False��5�      $��0,0,0,10q�5�       5�        5�      $ 
Payroll ID��6)    6=   �� *	PayrollId+  5�      $ User ID��6)    6:   �� *UserId+  5�      $ Total Salary��6)    6?   �� *TotalSalary+  5�      	 5�      �� 5�      .R�5�   "   �� -   5        $��Approve��5�   )   6;   5�      5�      5�      �� 5�      -   5         ��Name$ ReportTextBlock��5�      $��Loading report...��61   $��0,0,0,5q�6J   5�      5�      5�      