   M S B A M L   `   `   `  ����  TaskManager    TaskManager.AdminView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      .��5	      �   � RowDefinitions  5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   .+�5      �� $��Admin Dashboard��6   $��24(�5      $
��Bold"�5      $��Center=�5      $��Center=�5      $
��0,10q�5      �� 5       �� Orientation$ 
Horizontal=�6   $��Right=�62   $��	0,10,10,0q�6N   .+�5      �� -   5         ��Name$ 	btnLogout��5      $��Logout��5      $	��100��5      $��30��5      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 5��5      5      5      5       �� 5#   
   $��1��6   $��10q�6"   .@�5$      �� $��User Management��6   .��5%      �   5&      �� 5'      $��* �6(   �� 5(      $
��Auto �6(   .+�5*      &   System.Windows.Controls.DataGrid  -   5          Name$ UsersDataGrid��5*         AutoGenerateColumns$ False��5+      $��0,0,0,10q�5,         Columns 5-      0  *System.Windows.Controls.DataGridTextColumn  5.      ,  &System.Windows.Controls.DataGridColumn   Header$ User ID��61   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   �� *UserId+  5/      $ Username��61    6C   �� *
Username+  50      $ 	Full Name��61    6D   �� *
FullName+  51      $
 Role��61    6?   �� *Role+4  .System.Windows.Controls.DataGridTemplateColumn  52      	   CellTemplate	 53   "   �� 54   &   .R�55   *   �� $ 
Horizontal=�65   .+�56   .   �� -   5        $
��Edit��56   5   $	��5,0q�6[   �� 57   .   -   5        $��Delete��57   5   $	��5,0q�6_   58   +   59   '   5;      5=      �� 5>      $��1��6!   $ 
Horizontal=�6.   .+�5?      �� -   5        $��Add User��5?   !   $��0,0,10,0q�6J   5@      5A      5B      �� 5C      $��Project Overview��6   .��5D      � 
 � ColumnDefinitions
 5E      �� 5F      $	��58* �6+   �� 5G      $	��87* �6+   �� 5H      $
��629* �6+     5J      �� 5K      $
��Auto �6(   �� 5L      $��* �6(   .+�5N      �� -   5        $ btnCreateProject��5N      $��Create Project��5O      $	��120��5P      $��30��5Q      $ 5��5R      $��	10,0,0,10q�5T      $
��Left=�5U      $��2��68     5V      -   5        $ ProjectsDataGrid��5V      $��1��5W      $ False��5X      $��10,10,10,10q�5Y      $��3��64    5Z        5[      $ 
Project ID��61    6E   �� *	ProjectId+  5\      $
 Name��61    6?   �� *Name+  5]      $ 
Start Date��61    6E   �� *	StartDate+  5^      $ End Date��61    6C   �� *	EndDate+5`      5a      5b      5c      �� 5f   
   $��2��6   $��10q�6"   .+�5g      �� $��Payroll Approval��6   $
��Bold"�60   $��0,0,0,5q�6B     5h      -   5        $ PayrollDataGrid��5h      $ False��5i      $��0,0,0,10q�5j       5k        5l      $ 
Payroll ID��6)    6=   �� *	PayrollId+  5m      $ User ID��6)    6:   �� *UserId+  5n      $ Total Salary��6)    6?   �� *TotalSalary+  5o      	 5p      �� 5q      .R�5r   "   �� -	   5        $��Approve��5r   )   6;   5s      5u      5w      �� 5x      -
   5         ��Name$ ReportTextBlock��5x      $��Loading report...��61   $��0,0,0,5q�6J   5y      5z      5{      