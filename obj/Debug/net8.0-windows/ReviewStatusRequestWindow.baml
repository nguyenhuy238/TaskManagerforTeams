   M S B A M L   `   `   `  ����  TaskManager+    %TaskManager.ReviewStatusRequestWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  Review Status Request��5   	   $	��350��5	   	   $	��500��5
   	    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��16(�6/   $
��Bold"�6=   $��0,0,0,10q�6O   $��0��6a   ��6   �� *	TaskTitle+6a   �� 5   
   $��Request Details:��6   $
��Bold"�6,   $��0,0,0,5q�6>   $��1��6O   �� 5   
   $
��Wrap=�64   $��0,0,0,10q�6H   $��2��6Z   ��6   �� *RequestMessage+6Z   �� 5   
   $��Requested Status:��6   $
��Bold"�6-   $��0,0,0,5q�6?   $��3��6P   �� 5   
   $��0,0,0,10q�65   $��4��6G   ��6   �� *RequestedStatus+6G   �� 5   
   $0��*Response Comment (Required for rejection):��6   $
��Bold"�6F   $��0,0,0,5q�6X   $��5��6i   �� 5   
   -   5         ��Name$ ResponseCommentTextBox��5       �� AcceptsReturn$
 True��62   $��80��6G   $��0,0,0,10q�6S   $��6��6e   �� 5   
    �� Orientation$ 
Horizontal=�6   $��Right=�6.   $��5��6J   $��0,10,0,-152q�6W   $	��321��6l   .+�5       �� -   5         ��Name$ 
btnApprove��5       $��Approve��6)   $	��100��6;   $��0,0,10,0q�6G   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 5��5!      $��36��6B   �� 5"      -   5        $ 	btnReject��5"      $��Reject��6(   $	��100��69   $��0,0,10,0q�6E   $ 5��5#      $��34��6B   �� 5$      -   5        $ 	btnCancel��5$      $��Cancel��6(   $	��100��69   $ 5��5%      $��33��6B   5&      5'      5(      