   M S B A M L   `   `   `  ����  TaskManager%    TaskManager.CreateProjectWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   $	��348��5	   	   $	��465��5
   	     � WindowStartupLocation$  CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title 5   	   �� *WindowTitle+.��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��Project Name:��6   $��0,0,0,5q�6)   $��0��6:   �� 5   
   -   5         ��Name$ txtName��5      $��0,0,0,10q�6#   $��1��65   ��6B   �� *ProjectName+�� 5   
   $��Description:��6   $��0,0,0,5q�6(   $��2��69   �� 5    
   -   5        $ txtDescription��5        �� AcceptsReturn$
 True��6*   $��60��6?   $��0,0,0,10q�6K   $��3��6]   ��6j   �� *ProjectDescription+�� 5#   
   $��Start Date:��6   $��0,0,0,5q�6'   $��4��68   (  "System.Windows.Controls.DatePicker  5$   
   -   5          Name$ dpStartDate��5$      $��0,0,0,10q�6*   $��5��6<      SelectedDate 6I   �� *	StartDate+�� 5'   
   $��End Date (Optional):��6   $��0,0,0,5q�60   $��6��6A     5(   
   -   5        $ 	dpEndDate��5(      $��0,0,0,10q�6(   $��7��6:    6G   �� *	EndDate+�� 5+   
    �� Orientation$ 
Horizontal=�6   $��Right=�6.   $��8��6J   $��0,10,0,0q�6W   .+�5,      �� -   5        	 ��Name$	 	btnCancel��5,      $��Cancel��6(   $	��100��69   $��0,0,10,0q�6E   +  %MaterialDesignThemes.Wpf.ButtonAssist
   CornerRadius$
 5��5-      �� 5.      -   5        $	 btnSave��5.      $	��100��6I   $
 5��5/      ��5.   &   �� *SaveButtonText+5/      50      51      52      