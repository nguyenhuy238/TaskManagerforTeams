   M S B A M L   `   `   `  ����  TaskManager$    TaskManager.UpdateStatusDialog   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  Update Task Status��5   	   $	��200��5   	   $	��400��5   	    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5	   	   .��5
      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��New Status:��6   $
��Bold"�6'   $��0,0,0,5q�69   �� 5   
   -   5         ��Name$ StatusTextBlock��5      $��1��6-   $��0,0,0,10q�6:   �� 5   
   $��Comment (Required):��6   $
��Bold"�6/   $��0,0,0,5q�6A   $��2��6R   �� 5   
   -   5         ��Name$ CommentTextBox��5       �� AcceptsReturn$
 True��6*   $��60��6?   $��0,0,0,10q�6K   $��3��6]   �� 5   
    �� Orientation$ 
Horizontal=�6   $��Right=�6.   $��2��6J   $��
0,10,0,-98q�6W   .+�5      �� -   5         ��Name$ 	btnCancel��5      $��Cancel��6(   $��60��69   +  %MaterialDesignThemes.Wpf.ButtonAssist	   CornerRadius$	 5��5      $��26��6B   �� 5      -   5        $ btnSave��5      $
��Save��6&   $��52��65   $	 5��5      $��26��6B   5      5      5      