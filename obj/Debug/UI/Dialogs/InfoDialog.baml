   M S B A M L   `   `   `  ����A  =Cheops, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Cheops.DialogsCheops.Dialogs  5   	       Cheops.Dialogs.InfoDialog   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Cheops.Dialogs     5   	   -   5          �Name$  window��5   	    � Title$ 
InfoDialog��5   	   $	��320��6   $	��580��6)    � WindowStyle$
 None=�65    � 
ResizeMode$ NoResize=�6H    � 	Resources 5
         
exitHoover �� 
TargetType �� Triggers P� Property P� Value	 N� EnterActions
 g� Duration	 �� To �� TargetProperty N� ExitActions��%�  &	        �� 5   
   " �6#    5      P� 5       �@J�IsMouseOver . 	 5      �� 5      .��5      �� .��5   "   �� $
 0:0:0.3D�61   $ PaleVioletRedX�6D   $/ )(Grid.Background).(SolidColorBrush.Color)�6W   5      5       5      �� 5      .��5      �� .��5   "   �� $
 0:0:0.3D�61   $/ )(Grid.Background).(SolidColorBrush.Color)�6D   5      5      5      5      5      .��5#      �� $��1q�6   ��6!   �� *TitleBarColor+ �� ElementName$ window��._�5$   
   �� .+�5%      � -   5        $��24��5%      ��5&      �� 5'       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;    �� RelativeTransform 5(      V� 5)      .~�5*   "   ��  �� Angle$	 270Q�62    �� CenterY$	 0.5Q�6>    �� CenterX$	 0.5Q�6L   5+      .��5-      �  � Color 6'   �� *TitleBarColor+$ window��� 5.      $ White��6'    � Offset$	 0.9Q�65   5/       � ColumnDefinitions 53      �� 54      $��* �6'   �� 55      $
��Auto �6'   .+�58      �� $��Center=�6   $	��5,0q�64   ._�59      �� $��14(�6K   ��6    �� *Title+$ window��6K   5:      � 5<      -   5        $��1��5<      $��24��6'   
�������#��[  6z   .+�5=      �� $��Center=�6   $��5,0,5,0q�68   ._�5>      �� $��Zamknij��6$   $��12(�63   5?      5@      5B      � 5D      $	��250��6    5E      �� 5F      $��4* �6'   �� 5G      $��6* �6'   .+�5J      �� $��Center=�6u   $��Center=�6�   ��6   �� *
Icon.Width+$ window����6I   �� *Icon+$ window��6�   �� 5L      $��1��6   $��Center=�6/   $��Center=�6J   $	��300��6g   $	��210��6s   $
��20,0q�6�   $
��Auto=�6�   .��5M      �� $��Center=�6    $��12(�6l   ��6;   �� *MessageText+$ window��6l   5N      5P      � 5R      $��Right=�6   $��0,0,30,0q�64   .+�5S      �� $	��100��6   $��25��6%   $��
10,0,10,20q�6h   ��61   �� *MainButtonText+$ window��6h   �� 5T      $	��100��6   $��25��6%   $��
10,0,10,20q�6h   ��61   �� *SideButtonText+$ window��6h   5U      5W      5X      5Y      