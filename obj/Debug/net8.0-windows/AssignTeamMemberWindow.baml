   M S B A M L   `   `   `  ����  TaskManager(    "TaskManager.AssignTeamMemberWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  Assign Team Member��5   	   $	��400��5	   	   $	��800��5
   	    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $"��Select Team Member to Assign��6   $��0��68   $
��Bold"�6E   $��0,0,0,10q�6W   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ TeamMembersDataGrid��5      $��1��60      AutoGenerateColumns$ False��6=      CanUserAddRows$ False��5       �� SelectedValuePath$ UserId��6*   $��0,0,0,10q�5   <    �� SelectedValue 6   �� *SelectedUserId+	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$
 	Full Name��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6U   1  +System.Windows.Controls.DataGridBoundColumn   Binding 68   �� *
FullName+6U     5      $
 Skills��6%   $	 150 6P    65   �� *Skills+6P     5      $
 Workload (Tasks)��6%   $	 100 6`    6?   �� *CurrentTasks+6`     5      $
 On-Time Rate (%)��6%   $	 120 6u    6?   �� *
OnTimeRate+ �� StringFormat$ {0:F2}��6u     5       $
 Quality Score (%)��6%   $	 120 6x    6@   �� *QualityScore+$ {0:F2}��6x     5!      $
 Projects��6%   $ 80 6X    67   �� *ProjectCount+6X     5"      $
 Assigned Status��6%   $	 100 6a    6>   �� *AssignedStatus+6a   5$      �� 5'   
    �� Orientation$ 
Horizontal=�6   $��Right=�6.   $��2��6J   $��0,10,0,0q�6W   .+�5(      �� -   5         ��Name$ 	btnCancel��5(      $��Cancel��6(   $	��100��69   $��0,0,10,0q�6E   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 5��5)      �� 5*      -   5        $ 	btnAssign��5*      $��Assign��6(   $	��100��69   $ 5��5+      5,      5-      5.      