   M S B A M L   `   `   `  ����c  _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity  5      K GDigitsRecogniton, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:DigitsRecogniton.ViewsDigitsRecogniton.Views 5      +   %DigitsRecogniton.Views.CheckDigitView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:DigitsRecogniton.Views     5      ���#F0F0F0$��16(�5   4   .��5      �   � ColumnDefinitions  5   
   �� 5      $��20 �6   �� 5      $��* �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $��20 �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��20 �6   .+�5"   
   �� $��1��6   $��2��6!   $��4��61   $��Center=�5#      $��0,0,0,30q�5$      	Draw here61   �� 5(   
   $��2q�6   $	��565��6%   $	��455��62   
���   �$��2��5)      $��2��6   $��4��6.   ._�5*      �� $��0q�6   $	��560��6)   $	��450��66   
���   �$��2��5+      $��2��6   $��4��6.   ._�5,      �� -   5         ��Name$ myCanvas��5,      $��2��6.   $��2��6;   $��4��6K   $	��560��5-      $	��450��6!   $��0,0,0,0q�6-   6@   5.      5/      �� 51   
   $��3��6   $��3��6!   $��right=�52      $��0,20,0,0q�60   Recognized digit:6U   �� 53   
   -   5         ��Name$ 	NameDigit��53      $��3��6'   $��4��64   $��0,20,0,0q�6D   $��Center=�54      63   �� 57   
   $��4��6   $��2��6   $��2��6.   $	��200��58      $��30��6    $��	0,20,10,0q�59      $��Check��6$   ��5:      ��  �� Path$ CheckDigitCommand���5;   	   ��  �� ElementName$ myCanvas��5<      �� 5=   
   $��4��6   $��4��6   $��2��6.   $	��200��5>      $��30��6    $��	10,20,0,0q�5?      $��Clear��6$   ��5@      �� $  ClearCanvasAndDigitCommand���5A   	   �� $ myCanvas��6=   5C      5D      