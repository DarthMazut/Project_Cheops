using System;
using System.Collections.Generic;
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
using Cheops.ChessBoardComponent;
using System.ComponentModel;
using Cheops.AI;

namespace Cheops.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for AdvanceEvaluation.xaml
    /// </summary>
    public partial class AdvanceEvaluation : Window, INotifyPropertyChanged
    {
        private double _fixedHeight;

        public double FixedHeight
        {
            get { return _fixedHeight; }
            set
            {
                if(_fixedHeight != value)
                {
                    _fixedHeight = value;
                    NotifyPropertyChanged(nameof(FixedHeight));
                }
                    
            }
        }

        private void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public double FinalScore { get; }
        public int NumberOfPossbileMoves { get; }
        public GameStage GameStage { get; } 
        public bool IsClosedPosition { get; }

        public AdvanceEvaluation(AI.Board board)
        {
            InitializeComponent();
            DataContext = this;
            xe_SmarBoard.PieceSet = new PieceSet("../img/PieceSet/Neo");
            xe_FENstring_TextBox.Text = board.ExportFEN();
            xe_SmarBoard.RoatetBoard(board.SideToMove);

            xe_SmarBoard.LogicalBoard = board;
            xe_SmarBoard.DisplayPositionFormLogicalBoard();

            NumberOfPossbileMoves = board._possibleBoardsList.Count;
            GameStage = Evaluation.GetGameState(board);
            IsClosedPosition = Evaluation.IsClosedPosition(board);
            FinalScore = Evaluation.EvaluatePosition(board);

            //Dokończyć
            xe_finalScoreEmfaza.ToolTip = "Lekka przewaga białych.";

            List<EvaluateItem> lista = new List<EvaluateItem>();

            lista.Add(new EvaluateItem
            (
                null,
                "Material Value",
                Evaluation.EvaluateMaterial(board),
                1
            ));

            //=========================================

            lista.Add(new EvaluateItem
            (
                Side.White,
                "Number of blocked bishops",
                Evaluation.CountBlockedBishops(board, Side.White),
                -Evaluation.w_blockedBishops
            ));

            lista.Add(new EvaluateItem
            (
                Side.Black,
                "Number of blocked bishops",
                Evaluation.CountBlockedBishops(board, Side.Black),
                Evaluation.w_blockedBishops
            ));

            //=========================================

            lista.Add(new EvaluateItem
            (
                Side.White,
                "Number of blocked rooks",
                Evaluation.CountBlockedRooks(board, Side.White),
                -Evaluation.w_blockedRooks
            ));

            lista.Add(new EvaluateItem
            (
                Side.Black,
                "Number of blocked rooks",
                Evaluation.CountBlockedRooks(board, Side.Black),
                Evaluation.w_blockedRooks
            ));

            //=========================================

            lista.Add(new EvaluateItem
            (
                Side.White,
                "Is 8th rank mate possible",
                Convert.ToDouble(Evaluation.IsPossible8thRankMate(board, Side.White)),
                -Evaluation.w_Is8thRankMatePossible
            ));

            lista.Add(new EvaluateItem
            (
                Side.Black,
                "Is 8th rank mate possible",
                Convert.ToDouble(Evaluation.IsPossible8thRankMate(board, Side.Black)),
                Evaluation.w_Is8thRankMatePossible
            ));

            //=========================================

            int[] whietPawnDistribution = Evaluation.GetPawnDistribution(board, Side.White);
            int[] blackPawnDistribution = Evaluation.GetPawnDistribution(board, Side.Black);

            //=========================================

            lista.Add(new EvaluateItem
            (
                Side.White,
                "Number of doubled pawns",
                Evaluation.CountDoubledPawns(whietPawnDistribution),
                -Evaluation.w_doublePawns
            ));

            lista.Add(new EvaluateItem
            (
                Side.Black,
                "Number of doubled pawns",
                Evaluation.CountDoubledPawns(blackPawnDistribution),
                Evaluation.w_doublePawns
            ));

            //=========================================

            lista.Add(new EvaluateItem
            (
                Side.White,
                "Number of isolated pawns",
                Evaluation.CountIsolatedPawns(whietPawnDistribution),
                -Evaluation.w_isolatedPawns
            ));

            lista.Add(new EvaluateItem
            (
                Side.Black,
                "Number of isolated pawns",
                Evaluation.CountIsolatedPawns(blackPawnDistribution),
                Evaluation.w_isolatedPawns
            ));

            //=========================================

            lista.Add(new EvaluateItem
            (
                Side.White,
                "Number of minor pieces at edges",
                Evaluation.CountKnightsAndBishopsAtEdges(board, Side.White),
                -Evaluation.w_minorPiecesAtEdges
            ));

            lista.Add(new EvaluateItem
            (
                Side.Black,
                "Number of minor pieces at edges",
                Evaluation.CountKnightsAndBishopsAtEdges(board, Side.Black),
                Evaluation.w_minorPiecesAtEdges
            ));

            //=========================================

            if (Evaluation.GetGameState(board) != GameStage.Enidng)
            {
                lista.Add(new EvaluateItem
            (
                Side.White,
                "Number of directions in which the king is open",
                Evaluation.HowManyDirectionsKingIsOpen(board, Side.White),
                -Evaluation.w_directionsKingOpen
            ));

                lista.Add(new EvaluateItem
            (
                Side.Black,
                "Number of directions in which the king is open",
                Evaluation.HowManyDirectionsKingIsOpen(board, Side.Black),
                Evaluation.w_directionsKingOpen
            ));


                lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Is king in center",
                    Convert.ToDouble(Evaluation.IsKingInCenter(board, Side.White)),
                    -Evaluation.w_IsKingInCenter
                ));

                lista.Add(new EvaluateItem
                (
                    Side.Black,
                    "Is king in center",
                    Convert.ToDouble(Evaluation.IsKingInCenter(board, Side.Black)),
                    Evaluation.w_IsKingInCenter
                ));


            }
            else
            {
                lista.Add(new EvaluateItem
             (
                 Side.White,
                 "Number of directions in which the king is open",
                 Evaluation.HowManyDirectionsKingIsOpen(board, Side.White),
                 -Evaluation.w_directionsKingOpenEnding
             ));

                lista.Add(new EvaluateItem
            (
                Side.Black,
                "Number of directions in which the king is open",
                Evaluation.HowManyDirectionsKingIsOpen(board, Side.Black),
                Evaluation.w_directionsKingOpenEnding
            ));
            }

            //=========================================

            lista.Add(new EvaluateItem
             (
                 Side.White,
                 "Number of king sourranding squares under attack",
                 Evaluation.CountKingSurroundingsSquareAttackers(board, Side.White),
                 -Evaluation.w_KingSourrandingSquareAttack
             ));

            lista.Add(new EvaluateItem
              (
                  Side.Black,
                  "Number of king sourranding squares under attack",
                  Evaluation.CountKingSurroundingsSquareAttackers(board, Side.Black),
                  Evaluation.w_KingSourrandingSquareAttack
              ));

            xe_DataGrid.ItemsSource = lista;

            //=======================================

            double currentValue;
            double sumValue = 0;

            if (!board.IsKingSquareAttacked(Side.White))
            {
                foreach (AI.Piece pinnedPiece in Evaluation.GetPinnedPieces(board, Side.White))
                {
                    Evaluation._piecesValues.TryGetValue(pinnedPiece.Sign, out currentValue);
                    sumValue += currentValue;
                }
            }
            else
                sumValue = Evaluation.w_PinnedPiecesCheck;

            lista.Add(new EvaluateItem
            (
                Side.White,
                "Number of pinned pieces",
                sumValue,
                -Evaluation.w_PinnedPieces
            ));

            sumValue = 0;

            if (!board.IsKingSquareAttacked(Side.Black))
            {
                foreach (AI.Piece pinnedPiece in Evaluation.GetPinnedPieces(board, Side.Black))
                {
                    Evaluation._piecesValues.TryGetValue(pinnedPiece.Sign, out currentValue);
                    sumValue += currentValue;
                }
            }
            else
                sumValue = -Evaluation.w_PinnedPiecesCheck;

            lista.Add(new EvaluateItem
            (
                Side.Black,
                "Number of pinned pieces",
                sumValue,
                -Evaluation.w_PinnedPieces
            ));

            //==================================

            lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Is castle blocked",
                    Convert.ToDouble(Evaluation.IsCastleBlocked(board, Side.White)),
                    -Evaluation.w_CastleBlocked
                ));

            lista.Add(new EvaluateItem
                (
                    Side.Black,
                    "Is castle blocked",
                    Convert.ToDouble(Evaluation.IsCastleBlocked(board, Side.Black)),
                    Evaluation.w_CastleBlocked
                ));

            //========================================================
            //              NAGRODY
            //========================================================

            lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Pawn chain length",
                    Evaluation.GetPawnChainLength(board, Side.White),
                    Evaluation.w_PawnChainLength
                ));

            lista.Add(new EvaluateItem
               (
                   Side.Black,
                   "Pawn chain length",
                   Evaluation.GetPawnChainLength(board, Side.Black),
                   -Evaluation.w_PawnChainLength
               ));

            //=======================================================

            lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Number of pawns in the center",
                    Evaluation.CountPawnInTheCenter(board, Side.White),
                    Evaluation.w_PawnInTheCenter
                ));

            lista.Add(new EvaluateItem
                (
                    Side.Black,
                    "Number of pawns in the center",
                    Evaluation.CountPawnInTheCenter(board, Side.Black),
                    -Evaluation.w_PawnInTheCenter
                ));

            //=======================================================

            lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Number of pawns about to promote",
                    Evaluation.CountPawnsAboutToPromotion(board, Side.White),
                    Evaluation.w_PawnAboutToPromote
                ));

            lista.Add(new EvaluateItem
                (
                    Side.Black,
                    "Number of pawns about to promote",
                    Evaluation.CountPawnsAboutToPromotion(board, Side.Black),
                    -Evaluation.w_PawnAboutToPromote
                ));

            //=======================================================

            lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Number of pass pawns",
                    Evaluation.CountPassPawns(board, Side.White),
                    Evaluation.w_PassPawns
                ));

            lista.Add(new EvaluateItem
               (
                   Side.Black,
                   "Number of pass pawns",
                   Evaluation.CountPassPawns(board, Side.Black),
                   -Evaluation.w_PassPawns
               ));

            //=======================================================

            lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Number of minor pieces in the center",
                    Evaluation.CountKnightAndBishopsInTheCenter(board, Side.White),
                    Evaluation.w_MinorPieceInTheCenter
                ));

            lista.Add(new EvaluateItem
                (
                    Side.Black,
                    "Number of minor pieces in the center",
                    Evaluation.CountKnightAndBishopsInTheCenter(board, Side.Black),
                    -Evaluation.w_MinorPieceInTheCenter
                ));


            //========================================================

            lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Has bishops pair",
                    Convert.ToDouble(Evaluation.HasBishopsPair(board, Side.White)),
                    Evaluation.w_BishopPair
                ));

            lista.Add(new EvaluateItem
                (
                    Side.Black,
                    "Has bishops pair",
                    Convert.ToDouble(Evaluation.HasBishopsPair(board, Side.Black)),
                    -Evaluation.w_BishopPair
                ));

            //=======================================================

            lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Number of bishops on long diagonal",
                    Evaluation.CountLongDiagonalBishops(board, Side.White),
                    Evaluation.w_LongDiagonalBishop
                ));

            lista.Add(new EvaluateItem
                (
                    Side.Black,
                    "Number of bishops on long diagonal",
                    Evaluation.CountLongDiagonalBishops(board, Side.Black),
                    -Evaluation.w_LongDiagonalBishop
                ));

            //=======================================================

            lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Number of supported knights",
                    Evaluation.CountSupportedKnights(board, Side.White),
                    Evaluation.w_SupportedKnights
                ));

            lista.Add(new EvaluateItem
                (
                    Side.Black,
                    "Number of supported knights",
                    Evaluation.CountSupportedKnights(board, Side.Black),
                    -Evaluation.w_SupportedKnights
                ));

            //========================================================

            lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Has connected rooks",
                    Convert.ToDouble(Evaluation.AreRooksConnected(board, Side.White)),
                    Evaluation.w_AreRooksConnected
                ));

            lista.Add(new EvaluateItem
                (
                    Side.Black,
                    "Has connected rooks",
                    Convert.ToDouble(Evaluation.AreRooksConnected(board, Side.Black)),
                    -Evaluation.w_AreRooksConnected
                ));

            //=================================================

            if (Evaluation.GetGameState(board) == GameStage.Opening)
            {
                lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Is queen in the center",
                    Convert.ToDouble(Evaluation.IsQueenInTheCenter(board, Side.White)),
                    -Evaluation.w_QueenInCenterOpening
                ));

                lista.Add(new EvaluateItem
                (
                    Side.Black,
                    "Is queen in the center",
                    Convert.ToDouble(Evaluation.IsQueenInTheCenter(board, Side.Black)),
                    Evaluation.w_QueenInCenterOpening
                ));
            }
            else
            {
                lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Is queen in the center",
                    Convert.ToDouble(Evaluation.IsQueenInTheCenter(board, Side.White)),
                    Evaluation.w_QueenInCenterMidEnd
                ));

                lista.Add(new EvaluateItem
                (
                    Side.Black,
                    "Is queen in the center",
                    Convert.ToDouble(Evaluation.IsQueenInTheCenter(board, Side.Black)),
                    -Evaluation.w_QueenInCenterMidEnd
                ));
            }

            //==========================================

            lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Material which protects the king",
                    Evaluation.KingMaterialProtection(board, Side.White),
                    Evaluation.w_KingProtectingMaterial
                ));

            lista.Add(new EvaluateItem
                (
                    Side.Black,
                    "Material which protects the king",
                    Evaluation.KingMaterialProtection(board, Side.Black),
                    Evaluation.w_KingProtectingMaterial
                ));

            //========================================================

            lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Has bishop battery",
                    Convert.ToDouble(Evaluation.HasBishopBattery(board, Side.White)),
                    Evaluation.w_BishopBattery
                ));

            lista.Add(new EvaluateItem
               (
                   Side.Black,
                   "Has bishop battery",
                   Convert.ToDouble(Evaluation.HasBishopBattery(board, Side.Black)),
                   -Evaluation.w_BishopBattery
               ));

            //========================================================

            lista.Add(new EvaluateItem
                (
                    Side.White,
                    "Has rook battery",
                    Convert.ToDouble(Evaluation.HasRookBattery(board, Side.White)),
                    Evaluation.w_RookBattery
                ));

            lista.Add(new EvaluateItem
               (
                   Side.Black,
                   "Has rook battery",
                   Convert.ToDouble(Evaluation.HasRookBattery(board, Side.Black)),
                   -Evaluation.w_RookBattery
               ));


        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void em_OnRoatetBoard(object sender, RoutedEventArgs e)
        {
            xe_SmarBoard.RotateBoard();
        }

        private void em_OnThemeChange_Click(object sender, RoutedEventArgs e)
        {
            if(Background == Brushes.Gray)
            {
                Background = Brushes.White;
                xe_FENstring_TextBox.Background = Brushes.White;
                xe_finalScoreEmfaza.Foreground = Brushes.Red;
                xe_DataGrid.RowBackground = Brushes.White;
            }
            else
            {
                Background = Brushes.Gray;
                xe_FENstring_TextBox.Background = Brushes.DarkGray;
                xe_finalScoreEmfaza.Foreground = Brushes.White;
                xe_DataGrid.RowBackground = Brushes.DarkGray;
            }
        }

        private void em_OnWindowResize(object sender, SizeChangedEventArgs e)
        {
            FixedHeight = ActualHeight - 250;
        }
    }
}
