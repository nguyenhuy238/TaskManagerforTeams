   M S B A M L   `   `   `  ����  TaskManager"    TaskManager.CreateTaskWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  Create New Task��5   	   $	��450��5	   	   $	��500��5
   	    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��Task Title:��6   $��0,0,0,5q�6'   $��0��68   �� 5   
   -   5         ��Name$ txtTitle��5      $��0,0,0,10q�6$   $��1��66   �� 5!   
   $��Description:��6   $��0,0,0,5q�6(   $��2��69   �� 5"   
   -   5        $ txtDescription��5"       �� AcceptsReturn$
 True��6*   $��80��6?   $��0,0,0,10q�6K   $��3��6]   �� 5%   
   $��Status:��6   $��0,0,0,5q�6#   $��4��64   �� 5&   
   -   5         ��Name$ 	cmbStatus��5&      $��0,0,0,10q�6&   $��5��68   .@�5'      �� $
��ToDo��6   �� 5(      $��
InProgress��6   �� 5)      $
��Done��6   5*      �� 5-   
   $��	Priority:��6   $��0,0,0,5q�6%   $��6��66   �� 5.   
   -   5        $ cmbPriority��5.      $��0,0,0,10q�6(   $��7��6:   .@�5/      �� $
��High��6   �� 50      $��Medium��6   �� 51      $	��Low��6   52      �� 55   
   $��Assigned To (Optional):��6   $��0,0,0,5q�63   $��8��6D   �� 56   
   -   5        $ cmbAssignedTo��56      $��0,0,0,10q�6*   $��9��6<    �� DisplayMemberPath$ FullName��6I   �� 59   
   $��Due Date (Optional):��6   $��0,0,0,5q�60   $��10��6A   (  "System.Windows.Controls.DatePicker  5:   
   -   5          Name$ 	dpDueDate��5:      $��0,0,0,10q�6(   $��11��6:   �� 5=   
   	 �� Orientation$	 
Horizontal=�6   $��Right=�6.   $��12��6J   $��0,10,0,0q�6X   .+�5>      �� -   5        
 ��Name$
 	btnCancel��5>      $��Cancel��6(   $	��100��69   $��0,0,10,0q�6E   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 5��5?      �� 5@      -   5        $
 	btnCreate��5@      $��Create��6(   $	��100��69   $ 5��5A      5B      5C      5D      