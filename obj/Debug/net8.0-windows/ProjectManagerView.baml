   M S B A M L   `   `   `  ����  TaskManager$    TaskManager.ProjectManagerView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      .��5	      �   � RowDefinitions  5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   .+�5      �� $��Project Manager Dashboard��6   $��24(�5      $
��Bold"�5      $��Center=�5      $��Center=�5      $
��0,10q�5      �� 5       �� Orientation$ 
Horizontal=�6   $��Right=�62   $��	0,10,10,0q�6N   .+�5      �� -   5         ��Name$ btnCreateProject��5      $��Create Project��5      $	��120��5      $��30��5      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 5��5      $��0,0,10,0q�5      �� 5       -   5        $ btnViewNotifications��5       $��View Notifications��5!      $	��140��5"      $��30��5#      $ 5��5$      $��0,0,10,0q�5&      �� 5'      -   5        $ 	btnLogout��5'      $��Logout��5(      $	��100��5)      $��30��5*      $ 5��5+      5,      5-      5.      &   System.Windows.Controls.DataGrid  51   
   -   5          Name$ ProjectsDataGrid��51      $��1��52         AutoGenerateColumns$ False��53      $��10q�54         CanUserAddRows$ False��55         Columns 56      0  *System.Windows.Controls.DataGridTextColumn  57      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Project ID��6%   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 69   �� *	ProjectId+  58      $
 Name��6%   	 63   �� *Name+  59      $ Description��6%   	 6:   �� *Description+  5:      $ 
Start Date��6%   	 69   �� *	StartDate+  5;      $ End Date��6%   	 67   �� *	EndDate+  5<      $ Progress��6%   	 67   �� *
Progress+
 �� StringFormat$
 {0:F2}%��4  .System.Windows.Controls.DataGridTemplateColumn  5=         CellTemplate 5>      �� 5?      .R�5@      �� -   5        $��
View Tasks��5@   %   $ 5��5A   %   5B      5D        5E       5F      �� 5G      .R�5H      �� -   5        $��Edit Project��5H   %   $ 5��5I   %   5J      5L        5M       5N      �� 5O      .R�5P      �� -   5        $��Assign Team Member��5P   %   $ 5��5Q   %   5R      5T      5V      �� 5Y   
   $��2��6   $ 
Horizontal=�6"   $��Center=�6;   $��10q�6X   .+�5Z      �� $��Overall Progress: ��6   $
��Bold"�62   $��Center=�6D   �� 5[      -   5         ��Name$ ProgressReportText��5[      $��
Loading...��64   $	��5,0q�6F   $��Center=�6S   5\      5]      5^      