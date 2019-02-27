using System;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using Cheops.ChessBoardComponent;
using System.Diagnostics;
using Cheops.UI.Dialogs;
using System.Windows.Media;
using System.Linq;

namespace Cheops
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {

        #region ======================================= VARIABLES =========================================================

        Process _stockfish;
        Coords _str;
        Coords _end;

        Side _sideToMove = Side.White;

        #endregion

        #region ======================================= PROPERTIES ========================================================

        /// <summary>
        /// Determines whether bottom panel with FEN-string TextBox is pinned.
        /// </summary>
        public bool IsFenBoxPined { get; set; } = false;

        #endregion

        #region ================================== DEPENDENCY PROPERTIES ==================================================

        #endregion

        #region ==================================== EVENTS & DELEGATES ===================================================

        #endregion

        #region ================================ CONSTRUCTOR / DESTRUCTOR =================================================

        public MainWindow()
        {
            InitializeComponent();

            AI.SearchTree.InitializeHistoryTables();
            AI.SearchTree.InitializeHashValues();
            Board.PieceSet = new PieceSet("../img/PieceSet/Neo");
            Board.LogicalBoard.GenerateHashValue();
            Board.LogicalBoard.GenerateBoards();

            for (ulong i = 0; i < AI.SearchTree.TTRecordNumber; i++)
            {
                AI.SearchTree.TranspositionTable[i] = new AI.TTRecord();
            }

            Helper.MainWindowHandle = this;
        }

        #endregion

        #region ====================================== PUBLIC METHODS =====================================================

        #endregion

        #region ====================================== PRIVATE METHODS ====================================================

        void PlugStockfish()
        {
            ProcessStartInfo si = new ProcessStartInfo()
            {
                FileName = @"C:\Users\Alien\Desktop\C++\visual MS\Aplikacje\Project Cheops\Cheops\stockfish_9_x64.exe",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            _stockfish = new Process();
            _stockfish.StartInfo = si;
            try
            {
                // throws an exception on win98
                _stockfish.PriorityClass = ProcessPriorityClass.BelowNormal;
            }
            catch { }

            _stockfish.OutputDataReceived += new DataReceivedEventHandler(stockfish_OutputDataReceived);

            _stockfish.Start();
            _stockfish.BeginErrorReadLine();
            _stockfish.BeginOutputReadLine();

            SendLine("uci");
            SendLine("isready");
            SendLine("ucinewgame");
            SendLine($"position fen rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQqk - 0 1");
            SendLine("go depth 2");
        }

        void SendLine(string command)
        {
            _stockfish.StandardInput.WriteLine(command);
            _stockfish.StandardInput.Flush();
        }

        void stockfish_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data.Contains("bestmove"))
            {
                string[] slowa = e.Data.Split(' ');

                string start = slowa[1].Substring(0, 2);
                string stop = slowa[1].Substring(2, 2);

                _str = (Coords)Enum.Parse(typeof(Coords), start.ToUpper());
                _end = (Coords)Enum.Parse(typeof(Coords), stop.ToUpper());

            }
        }

        public void HandleGameEndings()
        {

            if (Board.LogicalBoard._possibleBoardsList.Count == 0 && Board.LogicalBoard.IsKingInCheck)
            {
                string msg = $"{Helper.SwitchSide(Board.LogicalBoard.SideToMove)} wins!";
                MessageBox.Show(msg);
            }
            else if (Board.LogicalBoard._possibleBoardsList.Count == 0 && Board.LogicalBoard.IsKingInCheck == false)
            {
                MessageBox.Show("Stand down! It's a draw!");
            }
            else if (Board.LogicalBoard.WhitePiecesList.Count == 1 && Board.LogicalBoard.BlackPiecesList.Count == 1)
            {
                MessageBox.Show("Stand down! It's a draw!");
            }
            else if(Board.LogicalBoard.NoCaptureOrPawnMove >= 50)
            {
                MessageBox.Show("Stand down! It's a draw!");
            }

            int repetitions = 0;

            foreach (AI.Board board in Board.LogicalBoard.PreviousBoardList)
            {

                if (Enumerable.SequenceEqual(board.PieceArrangement, Board.LogicalBoard.PieceArrangement))
                {
                    repetitions++;
                }
            }
            if (repetitions >= 2)
                MessageBox.Show("Stand down! It's a draw.");
        }

        public void DisplayArrows()
        {
            if (Board.LogicalBoard.StartSquare != null && Board.LogicalBoard.FinishSquare != null)
            {
                Board.RemoveArrow();
                Board.DrawArrow(Helper.ExtandCoordsToCoords((ExtandCoords)Board.LogicalBoard.StartSquare),
                            Helper.ExtandCoordsToCoords((ExtandCoords)Board.LogicalBoard.FinishSquare));
            }
        }

        #endregion

        #region ======================================= EVENT METHODS =====================================================

        private void em_OnSquareClick(object sender, ChessBoardClickEventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if(Board.IsDrawingArrow)
                    Board.SubmitArrow(e.Square);
                else
                    Board.InvokeArrow(e.Square);

            }
            else
            {
                Board.RemoveBorder();

                if (Board.SelectedSquare == null && e.Square.Piece != null)
                {
                    if (e.Square.Piece.Side == Board.LogicalBoard.SideToMove)
                    {
                        Board.SelectSquare(e.Square.Coords);

                        foreach (AI.Board board in Board.LogicalBoard._possibleBoardsList)
                        {
                            if (Helper.ExtandCoordsToCoords((ExtandCoords)board.StartSquare) == e.Square.Coords)
                            {
                                Brush legMovBr = (Brush)FindResource("LegalMoveGradient");
                                Board.HightlightSquare(Helper.ExtandCoordsToCoords((ExtandCoords)board.FinishSquare), legMovBr);
                            }
                        }
                    }
                }
                else if (Board.SelectedSquare != null)
                {
                    char znakProm = '\0';
                    Piece selectedPiece = Board.GetSelectedPiece();

                    if (selectedPiece != null)
                    {
                        if (selectedPiece is Pawn && Board.LogicalBoard.SideToMove == Side.White)
                        {
                            if (e.Square.Coords.ToString().EndsWith("8") && Board.SelectedSquare.Coords.ToString().EndsWith("7"))
                            {
                                PromotionDialog promotionDialog = new PromotionDialog(Board.LogicalBoard.SideToMove, Board.PieceSet);
                                if (promotionDialog.ShowDialog() == true)
                                {
                                    znakProm = promotionDialog.ChoosenSign;
                                }

                            }
                        }
                        else if (selectedPiece is Pawn && Board.LogicalBoard.SideToMove == Side.Black)
                        {
                            if (e.Square.Coords.ToString().EndsWith("1") && Board.SelectedSquare.Coords.ToString().EndsWith("2"))
                            {
                                PromotionDialog promotionDialog = new PromotionDialog(Board.LogicalBoard.SideToMove, Board.PieceSet);
                                if (promotionDialog.ShowDialog() == true)
                                {
                                    znakProm = promotionDialog.ChoosenSign;
                                }
                            }
                        }
                    }

                    Board.LogicalBoard.MakeMove(Board.SelectedSquare.Coords, e.Square.Coords, znakProm);

                    Board.Deselect();
                    Board.DisplayPositionFormLogicalBoard();
                    DisplayArrows();

                    if (Board.LogicalBoard.IsKingInCheck)
                        Board.SetCheck(Board.LogicalBoard.SideToMove);

                    HandleGameEndings();
                }
            }
        }

        private void em_OnRightSquareClick(object sender, ChessBoardClickEventArgs e)
        {
            Board.Deselect();
            Board.DrawBorder(e.Square.Coords, Brushes.PaleVioletRed);
        }

        private void em_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Board.RemoveArrow(Board.GetHoveredArrow());
            }
            else if(e.Key == Key.Enter)
            {

                //AI.BestMoveInfo bmi = AI.SearchTree.MinMaxSearch(Board.LogicalBoard, 3);
                //AI.BestMoveInfo bmi = AI.SearchTree.AlphaBetaSearch(Board.LogicalBoard, 4);
                //AI.BestMoveInfo bmi = AI.SearchTree.KillerSearch(Board.LogicalBoard, 4);
                //AI.BestMoveInfo bmi = AI.SearchTree.KillerWithSortSearch(Board.LogicalBoard, 5);
                //AI.BestMoveInfo bmi = AI.SearchTree.HistorySearch(Board.LogicalBoard, 4);
                //AI.BestMoveInfo bmi = AI.SearchTree.NullMoveSearch(Board.LogicalBoard, 5);
                //AI.BestMoveInfo bmi = AI.SearchTree.TTSearch(Board.LogicalBoard, 5);

                //ulong zRuchow = Board.LogicalBoard.HashValue;
                //Board.LogicalBoard.GenerateHashValue();
                //ulong zPozycji = Board.LogicalBoard.HashValue;


                ////MessageBox.Show((zRuchow == zPozycji).ToString());
                //MessageBox.Show($"Z ruchów\n{Convert.ToString((long)zRuchow, 2)}\nZ pozycji:\n{Convert.ToString((long)zPozycji,2)}");

                //AI.BestMoveInfo bmi = new AI.BestMoveInfo();
                //while (Board.LogicalBoard._possibleBoardsList.Count != 0)
                //{
                //    if(_sideToMove == Side.White)
                //    {
                //        bmi = AI.SearchTree.KillerWithSortSearch(Board.LogicalBoard, 4);
                //    }
                //    else
                //    {
                //        Title = "Obliczam...";
                //        bmi = AI.SearchTree.NullMoveSearch(Board.LogicalBoard, 4);
                //    }

                //    Board.LogicalBoard.MakeMove(bmi.StartSquare, bmi.FinishSquare, bmi.PromotionSign);
                //    Board.Deselect();
                //    Board.DisplayPositionFormLogicalBoard();
                //    DisplayArrows();
                //    if (Board.LogicalBoard.IsKingInCheck)
                //        Board.SetCheck(Board.LogicalBoard.SideToMove);
                //    HandleGameEndings();
                //    System.Windows.Forms.Application.DoEvents();
                //}

                //AI.BestMoveInfo bmi = new AI.BestMoveInfo();
                //for (int i = 1; i < 7; i++)
                //{
                //    bmi = AI.SearchTree.TTSearch(Board.LogicalBoard, i);
                //}


                //MessageBox.Show(
                //    $@"
                //       Algorytm: {bmi.AlgorithmName}
                //       Głębokość przeszukiwania: {bmi.SearchDepth}
                //       Czas [s]: {bmi.Time}
                //       Szybkość [pos/s]: {bmi.Speed}
                //       Łącznie przeanalizowanych pozycji: {bmi.NumberOfPositionEvaluated.ToString("N0")}
                //       Teoretyczna ilość pozycji do analizy: {bmi.NumberOfEstimateMinMaxPositionCount.ToString("N0")}
                //       Ilość odciętych pozycji: {bmi.NumberOfEvaluationReduction.ToString("N0")} ({bmi.SavedPercentage} %)
                //       Ilość odcięć alfa-beta: {bmi.NumberOfAlfaBetaCutOffs.ToString("N0")}
                //       Ścieżka: {bmi.Path}
                //       "
                //       );

                //Board.LogicalBoard.MakeMove(bmi.StartSquare, bmi.FinishSquare, bmi.PromotionSign);
                //Board.Deselect();
                //Board.DisplayPositionFormLogicalBoard();
                //DisplayArrows();
                //if (Board.LogicalBoard.IsKingInCheck)
                //    Board.SetCheck(Board.LogicalBoard.SideToMove);
                //HandleGameEndings();

                //MessageBox.Show($"Fast return count: {AI.SearchTree.SkippedEvaluations}");

                //Stopwatch stoper = new Stopwatch();
                //stoper.Start();
                //for (int i = 0; i < 10000; i++)
                //{
                //    Board.LogicalBoard.GenerateBoards();
                //}
                //stoper.Stop();
                //MessageBox.Show($"Czas: {stoper.ElapsedMilliseconds} ms");

            }
            else if(e.Key == Key.Space)
            {
                Stopwatch stoper = new Stopwatch();
                
                string[] feny = new string[10] {
                "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",
                "r1bqkbnr/1pp2ppp/p1p5/4p3/4P3/5N2/PPPP1PPP/RNBQ1RK1 b kq - 1 5",
                "r1bqk1nr/pppp1ppp/1bn5/8/2BPPp2/5N2/PPP3PP/RNBQ1RK1 b kq - 4 6",
                "r1bqr1k1/ppp2pbp/3p1np1/2nPp3/2P1P3/2NB1N2/PP3PPP/R1BQ1RK1 w - - 1 10",
                "rnbqk2r/pp1p1ppp/4pn2/8/1b1NP3/2N5/PPP2PPP/R1BQKB1R w KQkq - 3 6",
                "r1b1rbk1/2ppqpp1/pp2p2n/n2PP2p/4Q3/2NBBN2/PPP2PPP/3RR1K1 w - - 1 1",
                "rnb2bnr/pppp1k1p/8/8/5Q2/4B3/PqP3PP/RN3RK1 b - - 0 11",
                "r1bqk1nr/pppnppbp/3p2p1/8/2BPP3/5N2/PPP2PPP/RNBQK2R w KQkq - 1 1",
                "r1q2rk1/pp3pbp/2np1np1/2p1p3/4P3/PPNP1NPB/1BP2P2/R2QR1K1 b - - 0 13",
                "R7/8/8/8/8/3K4/8/3k4 w - - 1 1"
                };

                AI.Board plansza = new AI.Board();

                stoper.Start();

                for (int i = 0; i < 10; i++)
                {
                    plansza.SetPositionFromFEN(feny[i]);
                    plansza.GenerateBoards();

                    for (int j = 0; j < 1000; j++)
                    {
                        AI.Evaluation.EvaluatePosition(plansza);
                    }
                }

                stoper.Stop();

                MessageBox.Show($"Czas: {stoper.ElapsedMilliseconds} ms\nIlość ewaluacji: 10.000\nSzybkość: {10000000/stoper.ElapsedMilliseconds} pos/s.");
            }

        }

        private void em_OnZakotwiczonyClick(object sender, RoutedEventArgs e)
        {
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.CenterX = 0.5;
            rotateTransform.CenterY = 0.5;

            if (IsFenBoxPined)
            {
                xe_StackPanelBottom.Style = (Style)FindResource("FENBoxStyle");
                IsFenBoxPined = false;
                rotateTransform.Angle = 90;
                xe_PinImage.RenderTransform = rotateTransform;
            }
            else
            {
                xe_StackPanelBottom.Style = (Style)FindResource("FENBoxStyleZakotwiczony");
                IsFenBoxPined = true;
                rotateTransform.Angle = 0;
                xe_PinImage.RenderTransform = rotateTransform;
            }

            //DOROBIC PRZYCISK DO ZAKOTWICZANIA
        }

        #endregion

        #region ========================================= COMMANDS ========================================================

        private void CommandBinding_RotateBoard_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (xe_FENTextBox != null) xe_FENTextBox.Text = Board.LogicalBoard.ExportFEN();
            //Odswiezanie FEN-string
            e.CanExecute = true;
        }

        private void CommandBinding_RoatetBoard_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Board.RotateBoard();
        }

        private void CommandBinding_ChangeColor_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_ChangeColor_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Board.SetColors();
        }

        private void CommandBinding_NewBoard_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_NewBoard_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Board.ResetBoard();
        }

        private void CommandBinding_UndoMove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Board != null)
            {
                if (Board.LogicalBoard.PreviousBoardList.Count > 0)
                    e.CanExecute = true;
            }
        }

        private void CommandBinding_UndoMove_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Board.LogicalBoard.UndoMove();
            Board.DisplayPositionFormLogicalBoard();
            if (Board.LogicalBoard.IsKingInCheck)
                Board.SetCheck(Board.LogicalBoard.SideToMove);
            DisplayArrows();
        }

        private void CommandBinding_FenImport_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_FenImport_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FenDialog fenDialog = new FenDialog();
            if (fenDialog.ShowDialog() == true)
            {
                Board.SetPositionFromFEN(fenDialog.FENstring);
                Board.LogicalBoard.PreviousBoardList.Clear();
            }
        }

        private void CommandBinding_EvaluatePosition_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_EvaluatePosition_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(xe_AdvanceEvaluation_CheckBox.IsChecked == true)
            {
                AdvanceEvaluation evalWindow = new AdvanceEvaluation(Board.LogicalBoard);
                evalWindow.ShowDialog();
            }
            else
            { 
                string UltimateValue = AI.Evaluation.EvaluatePosition(Board.LogicalBoard).ToString("0.00");
                MessageBox.Show($@"WARTOŚ POZYCJI: {UltimateValue}");
            }
        }

        private void CommandBinding_PerformAnalyse_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_PerformAnalyse_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Analyse analiza = new Analyse(Board);
            analiza.ShowDialog();
        }

        private void CommandBinding_Info_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Info_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppInfo appinfo = new AppInfo();
            appinfo.Show();
        }

        #endregion


    }
}
