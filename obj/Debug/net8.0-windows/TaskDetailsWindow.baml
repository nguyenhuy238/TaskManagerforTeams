   M S B A M L   `   `   `  ����  TaskManager#    TaskManager.TaskDetailsWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  Task Details��5   	   $	��400��5	   	   $	��600��5
   	    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ 	CanResize=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ 	TaskTitle��5      $��20(�6'   $
��Bold"�65   $��0,0,0,10q�6G   �� 5   
   $��Description:��6   $
��Bold"�6(   $��0,0,0,5q�6:   $��1��6K   �� 5   
   -   5         ��Name$ TaskDescription��5       �� 
IsReadOnly$
 True��6+    �� AcceptsReturn$
 True��6=   $��80��6R   $��0,0,0,10q�6^   $��2��6p   �� 5   
   $��	Comments:��6   $
��Bold"�6%   $��0,0,0,5q�67   $��3��6H   �� 5   
   -   5         ��Name$ CommentsListBox��5      $��0,0,0,10q�6+   $��3��6=   ��5       �� 5!      .R�5"      �� .+�5#      �� $
��Wrap=�6A   ��6$   �� *CommentText+6A   �� 5$      $��Italic$�6|   
���������6$   �� *CreatedByName+	 �� StringFormat$.	 (Posted by {0} at {1:yyyy-MM-dd HH:mm:ss}��6�   5%      5&      5(      �� 5+   
   -   5        
 ��Name$
 btnClose��5+      $��Close��6#   $	��100��63   $��Right=�6?   $��4��6[   $��0,10,0,0q�6h   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 5��5,      6>   5-      5.      