   M S B A M L   `   `   `  ����A  =Cheops, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Cheops.UI.DialogsCheops.UI.Dialogs  5   	   G(clr-namespace:Cheops.ChessBoardComponentCheops.ChessBoardComponent  5   	       Cheops.UI.Dialogs.Analyse   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Cheops.UI.Dialogs     5   	   HChessBoardComponent(clr-namespace:Cheops.ChessBoardComponent     5   	     � Title$  Analyse��5	   	   $	��450��6   $	��700��6&    � Icon$  /Cheops;component/icon.ico��62    � 
ResizeMode$ NoResize=�6T   .��5
      �  � RowDefinitions 5   
   �� 5      .+�5   
   +   %Cheops.ChessBoardComponent.SmartBoard  -   5          Name$ xe_Board��5   )   $
��Left=�6;   $	��250��6V   $��
432,87,0,0q�6c   $	��Top=�6w   $	��250��6�   $��2��6�   �� 5   
   -   5         ��Name$ 	textBlock��5      $��Center=�6'   $��0,10,0,0q�6D   $
��Wrap=�6V   $��Znajdź najelpszy ruch��6j   $	��Top=�6�   $��28(�6�   $��Cambria)�6�   �� 5   
   
���   �$��0,0,0,1q�6%   $��Center=�6?   $��33��6\   $��
10,15,10,0q�6h   $	��Top=�6|   $	��672��6�   �� 5   
   -   5         ��Name$ 	xe_DDList��5      $
��Left=�6&   $��
10,113,0,0q�6A   $	��Top=�6U   $	��410��6m   6y   �� 5   
   -   5         ��Name$ label��5      $��Algorytm eksplorujący:��6   $
��Left=�6@   $��	10,87,0,0q�6[   $	��Top=�6n   $��0,5,5,5q�6�   �� 5   
   -   5         ��Name$ xe_checkBox��5      $��Iteracyjne pogłębianie��6(   $
��Left=�6I   $��
24,145,0,0q�6d   $	��Top=�6x   $��20��6�   	 J� RenderTransformOrigin$	 0.5,0.5&�6�   {�.  z�5      V� 5      .~�5      �� 
 �� ScaleX$	
 1.2Q�6%    �� ScaleY$	 1.2Q�62   �� 5      �� 5      T� 5      5      5      �� 5   
   -   5         ��Name$ xe_Depth_TextBox��5      $
��Left=�6,   $��23��6G   $��
82,185,0,0q�6S    �� TextWrapping$
 Wrap=�6g   $��1��6{   $	��Top=�6�   $��40��6�   $��Center=�6�   $��Center=�6�   6�   �� 5   
   -   5        $ label1��5      $��Głębokość:��6    $
��Left=�65   $��
11,182,0,0q�6P   $	��Top=�6d   �� 5    
   -   5         ��Name$ xe_Button_OK��5       $��Rozpocznij analizę��6'   $��Center=�6D   $��
10,0,10,10q�6a   $��Bottom=�6u   $	��650��6�   $��40��6�   $��20(�6�   {�.  6�   5"      5#      