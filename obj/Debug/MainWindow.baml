   M S B A M L   `   `   `  ����A  =Cheops, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:CheopsCheops  5   	   G(clr-namespace:Cheops.ChessBoardComponentCheops.ChessBoardComponent  5   	       Cheops.MainWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Cheops     5   	   :chess(clr-namespace:Cheops.ChessBoardComponent     5   	   -   5          � Title$  Project Cheops��5	   	   $	��350��6    $	��525��6-    � Icon$ img/icon.ico��5
   	    � WindowState$ 	Maximized=�5        � 	Resources 5         myStyle �� 
TargetType'   !Cheops.ChessBoardComponent.Square �� Triggers P� Property P� Value �� Property	 �� Value  myArrowStyle&    Cheops.ChessBoardComponent.Arrow  FENBoxStyle
 N� EnterActions g� Duration	 W� To �� TargetProperty N� ExitActions  FENBoxStyleZakotwiczony  LegalMoveGradient � Color � Offset��%�  &	        &	 �     &	 ^    &	 �    &	 3    �� 5   
   "  6     5      P� 5       �@J�IsMouseOver . . �5      ��  � ��
	 �  ��63   5      5      �� 5   
   "  6%   .�5      ��  � ��
	 �  ��6'   �� 5       � ��$	 5��60    5      P� 5       �@J�IsMouseOver . . �5      ��  � ��
	 �����6/   5      5      �� 5!   
   " ��6$   .�5"      ��  � ��$	 15��6'    5#      P� 5$       �@J�IsMouseOver . 
 5%      �� 5&      .��5'      �� .��5(   "   W� $ 0:0:0.25D�63   $ 55X�6G   $ Height�6O   5)      5*       5,      �� 5-      .��5.      �� .��5/   "   W� $ 0:0:0.25D�63   $ 15X�6G   $ Height�6O   50      51      53      55      �� 58   
   " ��60   .�59      ��  � ��$	 55��6'   5:      
� 5<   
   .��5=      � $ 	#FFFAE150��6   $
 0.50Q�6-   � 5>      $ 1Q�6   $ 	#00FAE150��6&   5?      5B       J� CommandBindings 5F      )  #System.Windows.Input.CommandBinding  5G   
   -   5           Command$  local:Commands.RotateBoard��5H      5J        5M   
   -   5        $  local:Commands.ChangeColor��5N      5P        5S   
   -   5        $ local:Commands.NewBoard��5T      5V        5Y   
   -   5        $ local:Commands.UndoMove��5Z      5\        5_   
   -   5        $ local:Commands.FenImport��5`      5b        5e   
   -   5        $% local:Commands.EvaluatePosition��5f      5h        5l   
   -   5        $# local:Commands.PerformAnalyse��5m      5o        5r   
   -	   5        $ local:Commands.Info��5s      5u      .��5z      ]� .+�5}   
   +  %System.Windows.Controls.Ribbon.Ribbon  $	��Top=�6    �� BitmapScalingMode$ HighQuality=�6&      ApplicationMenu 5~      :  4System.Windows.Controls.Ribbon.RibbonApplicationMenu  5      $y�	Collapsed=�6(   .@�5�      .  (System.Windows.Controls.Ribbon.RibbonTab  $��Narzędzia główne��6   .@�5�      0  *System.Windows.Controls.Ribbon.RibbonGroup  $��Szachownica��6   .@�5�      1  +System.Windows.Controls.Ribbon.RibbonButton     Label$ Od nowa��6#      LargeImageSource$ img/Commands/board.png��63   $��local:Commands.NewBoard��6]     5�      $ Cofnij ruch��6#   $ img/Commands/back.png��67   $��local:Commands.UndoMove��6`     5�      $ Kolory��6#   $ img/Commands/colours.png��62   $ ��local:Commands.ChangeColor��6^     5�      $ Obróć szachownicę��6#   $ img/Commands/rotate.png��6=   $ ��local:Commands.RotateBoard��6h     5�      $ Importuj pozycje��6#   $ img/Commands/fen.png��6<   $��local:Commands.FenImport��6d   5�      5�        5�      $��Silnik��6   .@�5�        $��Silnik��6   .@�5�        $ Analizuj��6#   $ img/Commands/gear.png��64   $#��local:Commands.PerformAnalyse��6]     5�      $ Ewaluacja pozycji��6#   $ img/Commands/engine.png��6=   $%��local:Commands.EvaluatePosition��6h   3	  -System.Windows.Controls.Ribbon.RibbonCheckBox	  5�      -
   5        $#��xe_AdvanceEvaluation_CheckBox��5�   %    	  Label$ Pokaż szczegóły��6J    d� 	IsChecked$
 TrueY�6b   5�      5�        5�      $
��Info��6   .@�5�        $��
Informacje��6   .@�5�        $ Informacje o aplikacji��6#   $ img/Commands/info.png��6B   $��local:Commands.Info��5�   #   5�      5�      5�      � 5�   
   -   5        $��xe_gr��5�      
������� � RowDefinitions 5�      �� 5�      $��* �6    �� 5�      $
��Auto �6    .+�5�      +
   %Cheops.ChessBoardComponent.SmartBoard
  -   5         
 Name$ Board��5�      +   %Cheops.ChessBoardComponent.SmartBoard!   SquareLeftMouseButtonClick$ em_OnSquareClick��5�      "   SquareRightMouseButtonClick$ em_OnRightSquareClick��5�      $��20q�5�   .   $
��Auto��6:   $	��300��5�      $	��300��6/   +   %Cheops.ChessBoardComponent.ChessBoard   InCheckColor 5�      
� 5�      .��5�      � $	 Red��6'   $
 0.75Q�63   � 5�      $ 1Q�6'   $ 	#00FF0000��62   5�      5�      �� 5�      -   5        $��xe_StackPanelBottom��5�      ���#222222$��1��6I   #��[ 6V   .+�5�      � .+�5�      �� -   5        $��xe_FENTextBox��5�       �� 
IsReadOnly . $��Center=�6E   $��Center=�6b   $��25��6}   $��
10,15,10,0q�6�   $	��500��6�   $��3q�6�   $��Hello Text World!��6�   ��6�   �� *ActualWidth+ �� ElementName$ xe_StackPanelBottom��6�   �� 5�      -   5        $��xe_PinImage��5�      $��img/Commands/pin.png��6/   $��Right=�6M   $��Center=�6i   $��	0,10,20,0q�6�   $��32��6�   $��32��6�     J� RenderTransformOrigin$  0.5,0.5&�6�   z�5�      V� 5�      .~�5�   "   �� �� 5�   "   �� 5�   "   ! �� Angle$! 90Q�62   T� 5�   "   5�      5�      5�      5�      5�      5�      5�      