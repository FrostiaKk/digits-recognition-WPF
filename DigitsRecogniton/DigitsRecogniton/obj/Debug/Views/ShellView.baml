   M S B A M L   `   `   `  ����K  GDigitsRecogniton, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:DigitsRecogniton.ViewsDigitsRecogniton.Views  5   	   &     DigitsRecogniton.Views.ShellView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:DigitsRecogniton.Views     5   	     � WindowStartupLocation$  CenterScreen=�5       � Title$ 	ShellView��5   	   $	��500��6   $	��700��6+   $
��1100��6:   ���#F0F0F0$��16(�5	      .��5      �  � ColumnDefinitions 5   
   �� 5      $��20 �6   �� 5      $��* �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $��20 �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��20 �6   .+�5!   
   �� -   5         ��Name$ LoadCheckDigit��5!      $��1��6)   $��2��66   $��	0,0,10,10q�5"      $	��200��6$   $��30��63   $��Right=�5#      .��6-   Check Digit6:   �� 5$   
   -   5        $ LoadTrainAI��5$      $��1��6&   $��3��63   $��	10,0,0,10q�5%      $
��Left=�6$   $	��200��5&      $��30��6    .��60   
Train AI6:   �� 5)   
   -   5         ��Name$ 
ActiveItem��5*   -   $��2��5)      $��2��6&   $��2��5*      $��Center=�5+      5-      5.      