   M S B A M L   `   `   `  ����  TaskManager    TaskManager.GuestView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      .��5	      �   � RowDefinitions  5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   � $��0��6   .+�5      �� $��Guest Dashboard��6   $��24(�5      $
��Bold"�5      $��Center=�5      $��Center=�5      $
��0,10q�5      �� 5      -   5         ��Name$ btnLogin��5      $��Login��5      $��Right=�5      $	��Top=�5      $��	0,10,10,0q�5      $	��100��5      $��30��5      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 5��5      5       5!      &   System.Windows.Controls.DataGrid  5$   
   -   5          Name$ ProjectsDataGrid��5$      $��1��5%         AutoGenerateColumns$ False��5&      $��10q�5'         
IsReadOnly$
 True��5(         Columns 5)      0  *System.Windows.Controls.DataGridTextColumn  5*      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Project ID��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *	ProjectId+  5+      $
 Name��6%    63   �� *Name+  5,      $ 
Start Date��6%    69   �� *	StartDate+  5-      $ End Date��6%    67   �� *	EndDate+5/        52   
   -   5        $ TasksDataGrid��52      $��2��53      $ False��54      $��10q�55      $
 True��56       57        58      $ Task ID��6%    66   �� *TaskId+  59      $ Title��6%    64   �� *Title+  5:      $ Status��6%    65   �� *Status+  5;      $ Due Date��6%    67   �� *	DueDate+5=      5>      5?      