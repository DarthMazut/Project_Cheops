   M S B A M L   `   `   `  ����A  =Cheops, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Cheops.UI.DialogsCheops.UI.Dialogs  5   	   '    !Cheops.UI.Dialogs.PromotionDialog   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Cheops.UI.Dialogs     5   	   -   5          � Title$  Opcje promocji��5   	   $	��350��6    $	��650��6-    � 
ResizeMode$ NoResize=�69    � Icon$$ /Cheops;component/img/icon.ico��6f    � 	Resources 5
         SelectionGradient �� EndPoint �� 
StartPoint �� RelativeTransform �� CenterY �� CenterX	 �� CenterY
 �� CenterX �� Angle �� CenterY �� CenterX � Color � Offset  NoSelectionGradient��%6  &	        &	 �    �� 5   
   $ 0.5,1&�68   $ 0.5,0&�6I    5      V� 5      .~�5      �� $	 0.5Q�6%   $	 0.5Q�63   �� 5      $		 0.5Q�6$   $	
 0.5Q�62   �� 5      $ 45Q�6&   $	 0.5Q�61   $	 0.5Q�6?   T� 5      5      .��5      � $ 	#FF2EFF00��6   $ 0Q�6-   � 5      $ 	#FF2EFF00��6   $ 1Q�6-   � 5      $ White��6   $	 0.5Q�6)   5      �� 5   
   $ 0.5,1&�6:   $ 0.5,0&�6K    5      V� 5      .~�5      �� $	 0.5Q�6%   $	 0.5Q�63   �� 5      $		 0.5Q�6$   $	
 0.5Q�62   �� 5      $ 45Q�6&   $	 0.5Q�61   $	 0.5Q�6?   T� 5      5       .��5"      � $ 	#33333300��6   $ 0Q�6-   � 5#      $ 	#33333300��6   $ 1Q�6-   � 5$      $ White��6   $	 0.5Q�6)   5%      5'      .��5*      �  � ColumnDefinitions 5+   
   �� 5,      $��1* �6   �� 5-      $��1* �6   �� 5.      $��1* �6   �� 5/      $��1* �6    � RowDefinitions 51   
   �� 52      $��5* �6   �� 53      $��2* �6   .+�56   
   �� -   5        $��xe_Knight_Border��56      $��Center=�6)   $��Center=�6F   $	��128��6a   $	��128��6m   
���   �$��3q�6�   #��[ 6�   ._�57      �� -   5        $��xe_Knight_Image��57       �� BitmapScalingMode$ HighQuality=�6+   6Y   58      �� 5:   
   -   5        $��xe_Bishop_Border��5:      $��1��6)   $��Center=�69   $��Center=�6V   $	��128��6q   $	��128��6}   
���   �$��3q�6�   #��[ 6�   ._�5;      �� -   5        $��xe_Bishop_Image��5;      $ HighQuality=�6+   6Y   5<      �� 5>   
   -   5        $��xe_Rook_Border��5>      $��2��6'   $��Center=�67   $��Center=�6T   $	��128��6o   $	��128��6{   
���   �$��3q�6�   #��[ 6�   ._�5?      �� -   5        $��xe_Rook_Image��5?      $ HighQuality=�6)   6W   5@      �� 5B   
   -   5        $��xe_Queen_Border��5B      $��3��6(   $��Center=�68   $��Center=�6U   $	��128��6p   $	��128��6|   
���   �$��3q�6�   #��[ 6�   ._�5C      �� -	   5        $��xe_Queen_Image��5C      $ HighQuality=�6+   6Y   5D      �� 5F   
   -
   5        $��xe_Button_OK��5F      $��0��6%   $��1��65   $��4��6B   $��OK��6V   $	��150��6c   $��35��6o   $��Right=�6{   $	��Top=�6�   $��0,0,40,0q�6�    �� 	IsDefault . {�.  6�   5J      5K      