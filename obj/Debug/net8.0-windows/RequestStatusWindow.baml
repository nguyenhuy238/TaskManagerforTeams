   M S B A M L   `   `   `  ����  TaskManager%    TaskManager.RequestStatusWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  Request Status Update��5   	   $	��300��5	   	   $	��400��5
   	    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��Select New Status:��6   $��0,0,0,5q�6.   $��0��6?   �� 5   
   -   5         ��Name$ StatusComboBox��5      $��1��6+   $��0,0,0,10q�68   .@�5      �� .��6   ToDo6!   �� 5      .��6   
InProgress6'   �� 5      .��6   Done6!   5      �� 5   
   $��Comment:��6   $��0,0,0,5q�6$   $��2��65   �� 5   
   -   5         ��Name$ CommentTextBox��5      $��3��6*   $��0,0,0,10q�67   $	��100��6I    �� AcceptsReturn$
 True��6V   �� 5   
   $��3��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��
0,0,0,-165q�6W   .+�5       �� -   5         ��Name$ 	btnCancel��5       $��Cancel��6(   $��96��69   +  %MaterialDesignThemes.Wpf.ButtonAssist	   CornerRadius$	 5��5!      $��36��6B   
 J� RenderTransformOrigin$
 0.417,0.762&�6N   �� 5"      -   5        $ 	btnSubmit��5"      $��Submit Request��6(   $	��116��6A   $	 5��5#      $��34��6B   5$      5%      5&      