   M S B A M L   `   `   `  ����A  =Cheops, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Cheops.ChessBoardComponentCheops.ChessBoardComponent  5      +    %Cheops.ChessBoardComponent.ChessBoard   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:Cheops.ChessBoardComponent     5      -   5        $
��self��5   7   ��5	      �� *ActualHeight+  �� ElementName$
  self�� � 	Resources 5         
arrowStyle �� 
TargetType&    Cheops.ChessBoardComponent.Arrow �� Property �� Value �� Triggers P� Property P� Value��%3  &	        �� 5   
   "  6#   .�5      ��  � �� 6'   �� *
ArrowColor+$
  self���� 5       � �� 60   �� *ArrowThickness+$
  self�� 5      P� 5       �@J�IsMouseOver . . �5      ��  � �� 6/   �� *ArrowHoverColor+$
  self��5      5      5      .��5      �� ��6   �� *OuterBorderColor+$
  self����6H   �� *OuterBorderThickness+$
  self��._�5   
   � -   5         �Name$ 	OuterGrid��5      ��6<   �� *MarginColor+$
  self��	 � ColumnDefinitions	 5      �� 5      $��* �6#   �� 5      $��4* �6#   �� 5      $��4* �6#   �� 5      $��4* �6#   �� 5       $��4* �6#   �� 5!      $��4* �6#   �� 5"      $��4* �6#   �� 5#      $��4* �6#   �� 5$      $��4* �6#   �� 5%      $��* �6#   
 � RowDefinitions
 5'      �� 5(      $��* �6    �� 5)      $��4* �6    �� 5*      $��4* �6    �� 5+      $��4* �6    �� 5,      $��4* �6    �� 5-      $��4* �6    �� 5.      $��4* �6    �� 5/      $��4* �6    �� 50      $��4* �6    �� 51      $��* �6    .+�54      �� $��1��6   $��1��6%   $��8��62   $��8��6F   ��6W   �� *InnerBorderThickness+$
  self����6�   �� *InnerBorderColor+$
  self��._�55      � -   5        $ 	InnerGrid��55      	 56      �� 57      $��* �6+   �� 58      $��* �6+   �� 59      $��* �6+   �� 5:      $��* �6+   �� 5;      $��* �6+   �� 5<      $��* �6+   �� 5=      $��* �6+   �� 5>      $��* �6+   
 5@      �� 5A      $��* �6(   �� 5B      $��* �6(   �� 5C      $��* �6(   �� 5D      $��* �6(   �� 5E      $��* �6(   �� 5F      $��* �6(   �� 5G      $��* �6(   �� 5H      $��* �6(   5J      5K      5L      5M      5N      