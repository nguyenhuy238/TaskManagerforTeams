   M S B A M L   `   `   `  ����F  BTaskManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:TaskManagerTaskManager  5           TaskManager.TeamMemberView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      %localclr-namespace:TaskManager  5        � 	Resources  5
      )   #TaskManager.BooleanToColorConverter   BooleanToColorConverter.   (TaskManager.BooleanToVisibilityConverter   BooleanToVisibilityConverter��%:   &	        &	        5   
   6(     5   
   6-   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   .+�5      �� $��Team Member Dashboard��6   $��24(�5      $
��Bold"�5      $��Center=�5      $��Center=�5      $
��0,10q�5      �� 5      -   5         ��Name$ 	btnLogout��5      $��Logout��5      $��Right=�5      $	��Top=�5       $��	0,10,10,0q�5!      $	��100��5"      $��30��5#      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 5��5$      5%      5&      &   System.Windows.Controls.DataGrid  5)   
   -   5          Name$ AssignedTasksDataGrid��5)      $��1��5*         AutoGenerateColumns$ False��5+      $��10q�5,         CanUserAddRows$ False��5-         Columns 5.      0  *System.Windows.Controls.DataGridTextColumn  5/      ,  &System.Windows.Controls.DataGridColumn   Header$ Task ID��6%   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 66   �� *TaskId+  50      $ Title��6%   	 64   �� *Title+  51      $ Status��6%   	 65   �� *Status+  52      $ Priority��6%   	 67   �� *
Priority+  53      $ Due Date��6%   	 67   �� *	DueDate+4  .System.Windows.Controls.DataGridTemplateColumn  54      $ Change Status��6)   
   CellTemplate
 55      �� 56      .R�57      ��  �� Orientation$ 
Horizontal=�6)   .+�58   "   ��  ��Name$ StatusComboBox��6+   $	��100��59       �� SelectedValuePath$ Content��5;       �� DisplayMemberPath$ Content��5<       �� SelectedValue 5:      �� *Status+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�.@�5=   &   �� $
��ToDo��63   �� 5>   &   $��
InProgress��63   �� 5?   &   $
��Done��63   5@   #   �� 5A   "   -   5        $��Update��5A   )   $��5,0,0,0q�6U   $ 5��5B      5C      5D      5F        5G      $ Details��6)   
 5H      �� 5I      .R�5J      �� -   5        $��View Details��5J   %   $ 5��5K   %   5L      5N      5P      �� 5S   
   $��2��6   $��10q�6"   .+�5T      �� $��Notifications��6   $
��Bold"�6-   $��0,0,0,5q�6?   �� 5U      -   5         ��Name$ NotificationsListBox��5U      $	��120��64   $
��Auto=�6A   ��5V      �� 5W      .R�5X      �� $ 
Horizontal=�6%   .+�5Y      �� $��0,0,10,0q�5[   (   ��5Y   (   �� *DisplayText+��5Z   (   �� *IsRead+ �� 	Converter# [  5[   (   �� 5\      -   5        $��Mark as Read��5\   %   $ 5��5^   %   y�5]   %   �� *IsRead+# [ 5^   %   5_      5`      5b      5c      5d      5e      