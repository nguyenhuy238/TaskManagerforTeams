   M S B A M L   `   `   `  ����  TaskManager    TaskManager.AdminView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      .��5	      �   � RowDefinitions  5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��Admin Dashboard��6   $��24(�5      $
��Bold"�5      $��Center=�5      $��Center=�5      $
��0,10q�5      �� 5   
   $��1��6   $��10q�6"   .@�5      �� $��User Management��6   .��5      �   5      �� 5      $��* �6(   �� 5      $
��Auto �6(   .+�5       &   System.Windows.Controls.DataGrid  -   5          Name$ UsersDataGrid��5          AutoGenerateColumns$ False��5!      $��0,0,0,10q�5"         Columns 5#      0  *System.Windows.Controls.DataGridTextColumn  5$      ,  &System.Windows.Controls.DataGridColumn   Header$ User ID��61   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   �� *UserId+  5%      $ Username��61    6C   �� *
Username+  5&      $ 	Full Name��61    6D   �� *
FullName+  5'      $
 Role��61    6?   �� *Role+4  .System.Windows.Controls.DataGridTemplateColumn  5(         CellTemplate 5)   "   �� 5*   &   .R�5+   *   ��  �� Orientation$ 
Horizontal=�65   .+�5,   .   �� -   5        $
��Edit��5,   5   $	��5,0q�6[   �� 5-   .   -   5        $��Delete��5-   5   $	��5,0q�6_   5.   +   5/   '   51      53      �� 54      $��1��6!   $ 
Horizontal=�6.   .+�55      �� -   5        $��Add User��55   !   $��0,0,10,0q�6J   56      57      58      �� 59      $��Project Overview��6   .��5:        -   5        $ ProjectsDataGrid��5:      $ False��5;      $��10q�5<       5=        5>      $ 
Project ID��6-    6A   �� *	ProjectId+  5?      $
 Name��6-    6;   �� *Name+  5@      $ 
Start Date��6-    6A   �� *	StartDate+  5A      $ End Date��6-    6?   �� *	EndDate+5C      5D      5E      �� 5H   
   $��2��6   $��10q�6"   .+�5I      �� $��Payroll Approval��6   $
��Bold"�60   $��0,0,0,5q�6B     5J      -   5        $ PayrollDataGrid��5J      $ False��5K      $��0,0,0,10q�5L       5M        5N      $ 
Payroll ID��6)    6=   �� *	PayrollId+  5O      $ User ID��6)    6:   �� *UserId+  5P      $ Total Salary��6)    6?   �� *TotalSalary+  5Q       5R      �� 5S      .R�5T   "   �� -   5        $��Approve��5T   )   6;   5U      5W      5Y      �� 5Z      -   5         ��Name$ ReportTextBlock��5Z      $��Loading report...��61   $��0,0,0,5q�6J   5[      5\      5]      