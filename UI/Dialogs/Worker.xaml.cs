using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cheops.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for Worker.xaml
    /// </summary>
    public partial class Worker : Window
    {
        BackgroundWorker worker = new BackgroundWorker();

        public Worker(int algorithmNumber, int depth, ChessBoardComponent.SmartBoard board, bool iterativeDeepning)
        {
            InitializeComponent();

            worker.WorkerReportsProgress = false;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += workerPracuj;
            worker.RunWorkerCompleted += workerZakonczone;

            AI.BGInfo bginfo = new AI.BGInfo();

            switch (algorithmNumber)
            {
                case 0:
                    bginfo = new AI.BGInfo("Min-Max", AI.SearchTree.MinMaxBW, board, depth, iterativeDeepning);
                    break;
                case 1:
                    bginfo = new AI.BGInfo("Alfa-Beta", AI.SearchTree.AlphaBetaBW, board, depth, iterativeDeepning);
                    break;
                case 2:
                    bginfo = new AI.BGInfo("Killer Moves", AI.SearchTree.KillerMovesBW, board, depth, iterativeDeepning);
                    break;
                case 3:
                    bginfo = new AI.BGInfo("Killer Moves + Simple Sort", AI.SearchTree.KillerMovesWithSortBW, board, depth, iterativeDeepning);
                    break;
                case 4:
                    bginfo = new AI.BGInfo("Killer Moves + Simple Sort + History Table", AI.SearchTree.HistoryTableBW, board, depth, iterativeDeepning);
                    break;
                case 5:
                    bginfo = new AI.BGInfo("Null Move", AI.SearchTree.NullMoveBW, board, depth, iterativeDeepning);
                    break;
                case 6:
                    bginfo = new AI.BGInfo("Transposition Tables", AI.SearchTree.TTBW, board, depth, iterativeDeepning);
                    break;

            }

            worker.RunWorkerAsync(bginfo);
        }


        private void workerZakonczone(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == false)
            {
                AI.BGInfo bginfo = e.Result as AI.BGInfo;
                AI.BestMoveInfo bmi = bginfo.bmi;
                ChessBoardComponent.SmartBoard Board = bginfo.Board;

                Close();

                MessageBox.Show(
                       $@"
                       Algorytm: {bmi.AlgorithmName}
                       Głębokość przeszukiwania: {bmi.SearchDepth}
                       Czas [ms]: {bmi.Time}
                       Szybkość [pos/s]: {bmi.Speed}
                       Łącznie przeanalizowanych pozycji: {bmi.NumberOfPositionEvaluated.ToString("N0")}
                       Teoretyczna ilość pozycji do analizy: {bmi.NumberOfEstimateMinMaxPositionCount.ToString("N0")}
                       Ilość odciętych pozycji: {bmi.NumberOfEvaluationReduction.ToString("N0")} ({bmi.SavedPercentage} %)
                       Ilość odcięć alfa-beta: {bmi.NumberOfAlfaBetaCutOffs.ToString("N0")}
                       Ścieżka: {bmi.Path}
                       "
                          );

                Board.LogicalBoard.MakeMove(bmi.StartSquare, bmi.FinishSquare, bmi.PromotionSign);
                Board.Deselect();
                Board.DisplayPositionFormLogicalBoard();
                Helper.MainWindowHandle.DisplayArrows();
                if (Board.LogicalBoard.IsKingInCheck)
                    Board.SetCheck(Board.LogicalBoard.SideToMove);
                Helper.MainWindowHandle.HandleGameEndings(); 
            }

            
        }

        private void workerPracuj(object sender, DoWorkEventArgs e)
        {
            AI.BGInfo args = e.Argument as AI.BGInfo;

            if (args.IteDeepning)
            {
                for (int i = 1; i < args.Depth+1; i++)
                {
                    try
                    {
                        args.bmi = AI.SearchTree.BWSearch(sender as BackgroundWorker, args.AlgorithmName, args.Algorithm, args.Board.LogicalBoard, i);
                    }
                    catch (Exception) {}
                }
            } 
            else
            {
                try
                {
                    args.bmi = AI.SearchTree.BWSearch(sender as BackgroundWorker, args.AlgorithmName, args.Algorithm, args.Board.LogicalBoard, args.Depth);
                }
                catch (Exception){}
            }

            if ((sender as BackgroundWorker).CancellationPending)
            {
                e.Cancel = true;
                return;
            }
               
            e.Result = args;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            worker.CancelAsync();
        }

        private void xe_Cancel_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
            Close();
        }
    }
}
