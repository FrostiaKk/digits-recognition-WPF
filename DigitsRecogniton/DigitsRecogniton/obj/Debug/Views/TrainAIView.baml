   M S B A M L   `   `   `  ����K  GDigitsRecogniton, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:DigitsRecogniton.ViewsDigitsRecogniton.Views  5      (    "DigitsRecogniton.Views.TrainAIView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:DigitsRecogniton.Views     5      ���#F0F0F0$��16(�5   4   .��5	      �   � ColumnDefinitions  5
   
   �� 5      $��20 �6   �� 5      $��* �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $��20 �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��20 �6   .+�5    
   �� $��1��6   $��2��6!   $��4��61   $��Center=�5!      $��0,0,0,20q�5"      	Draw here61   �� 5%   
   $��2q�6   $	��565��6%   $	��455��62   
���   �$��2��5&      $��2��6   $��4��6.   ._�5'      �� $��0q�6   $	��560��6)   $	��450��66   
���   �$��2��5(      $��2��6   $��4��6.   ._�5)      �� -   5         ��Name$ myCanvas��5)      $��2��6.   $��2��6;   $��4��6K   $	��560��5*      $	��450��6!   $��0,0,0,0q�6-   6@   5+      5,      �� 50   
   -   5         ��Name$ Digits��52      $��3��50      $��3��6    $��2��60   $��0,20,0,0q�51      $	��200��52   #   $��30��62    �� SelectedItem 51   %   �� *SelectedDigit+��53      �� 54      .R�55      �� $��	DigitName��6    ��61   �� *Name+57      59      �� 5<   
   $��4��6   $��2��6   $��2��6.   $	��200��5=      $��30��6    $��	0,20,10,0q�60   ��5>      ��  �� Path$ SavePatternCommand���5?   	   ��  �� ElementName$ myCanvas��.��6;   Train6B   �� 5@   
   $��4��6   $��4��6   $��2��6.   $	��200��5A      $��30��6    $��	10,20,0,0q�60   ��5B      �� $ ClearCanvasCommand���5C   	   �� $ myCanvas��.��6;   Clear6B   5F      5G      