using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.AI
{
    /// <summary>
    /// Holds a set of algorithms used for game tree exploration.
    /// </summary>
    public static class SearchTree
    {
        static Stopwatch stoper = new Stopwatch();
        static Random kostka = new Random();

        public static int EvaluationCounter = 0;
        public static int AlfaBetaCuttOff = 0;

        static Agent SwitchAgent(Agent agent)
        {
            if (agent == Agent.Maximizer)
                return Agent.Minimizer;
            else
                return Agent.Maximizer;
        }

        //==================================================================================
        //============================= STATIC SORT ========================================
        //==================================================================================

        static void StaticSort(Board board)
        {
            for (int i = 0; i < board._possibleBoardsList.Count; i++)
            {
                if (board._possibleBoardsList[i].IsKingInCheck ||
                    board._possibleBoardsList[i].IsCapturePosition ||
                    board._possibleBoardsList[i].IsPromotionPosition)
                {
                    Board toSort = board._possibleBoardsList[i];
                    board._possibleBoardsList.Remove(toSort);
                    board._possibleBoardsList.Insert(0, toSort);
                }

            }
        }

        //==================================================================================
        //============================= KILLER MOVES =======================================
        //==================================================================================

        static KillerMove[,] killerTable = new KillerMove[2, 10];

        static bool whichKiller = true;

        static void KillerSort(Board board, int depth)
        {
            Board firstBoard = null;
            for (int i = 0; i < 1; i++)
            {
                if (killerTable[i, depth] != null)
                {
                    firstBoard = board._possibleBoardsList.Find(
                        x => (x.StartSquare == killerTable[i, depth].StartSquare) &&
                        (x.FinishSquare == killerTable[i, depth].FinishSquare) &&
                        (x.PromotionSign == killerTable[i, depth].PromotionSign));

                    if (firstBoard != null)
                    {
                        Board item = firstBoard;
                        board._possibleBoardsList.Remove(item);
                        board._possibleBoardsList.Insert(0, item);
                    }
                }
            }

        }

        static void KillerSave(int depth, Board bestScore)
        {
            killerTable[Convert.ToInt32(whichKiller), depth] = new KillerMove(bestScore.StartSquare, bestScore.FinishSquare, bestScore.PromotionSign);
            whichKiller = !whichKiller;
        }

        //Pogarsza wynik...
        static void KillerMoveDown()
        {
            for (int i = 2; i < 10; i++)
            {
                killerTable[0, i] = killerTable[0, i - 2];
                killerTable[1, i] = killerTable[1, i - 2];
            }
        }

        //==================================================================================
        //============================= HISTORY TABLE ======================================
        //==================================================================================

        static HistoryMove[] TablicaHistorii = new HistoryMove[4096];

        public static void InitializeHistoryTables()
        {
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    TablicaHistorii[i * 64 + j] = new HistoryMove
                    {
                        StartSquare = Helper.CoordsToExtandCoords((Coords)i),
                        FinishSquare = Helper.CoordsToExtandCoords((Coords)j)
                    };
                }
            }
        }

        static void FillHistoryTable(int depth, Board bestScore)
        {
            HistoryMove currentHisMove = Array.Find(TablicaHistorii, x => (x.StartSquare == bestScore.StartSquare) && (x.FinishSquare == bestScore.FinishSquare));
            if (currentHisMove != null)
                currentHisMove.Value += (int)Math.Pow(depth, 2);
        }

        static void HistoryTableSort()
        {
            TablicaHistorii = TablicaHistorii.OrderBy(x => x.Value).ToArray();
        }

        static void HistoryMovesSort(Board board)
        {
            Board firstBoard = null;
            foreach (HistoryMove hisMove in TablicaHistorii)
            {
                firstBoard = board._possibleBoardsList.Find(x => (x.StartSquare == hisMove.StartSquare) && (x.FinishSquare == hisMove.FinishSquare));

                if (firstBoard != null)
                {
                    Board item = firstBoard;
                    board._possibleBoardsList.Remove(item);
                    board._possibleBoardsList.Insert(0, item);
                }
            }
        }

        //==================================================================================
        //=========================== NULL MOVE PRUNNING ===================================
        //==================================================================================

        static bool isNullMoveOk = false;

        //==================================================================================
        //========================= TRANSPOSITION TABLES ===================================
        //==================================================================================

        public static int SkippedEvaluations = 0;

        public static int FastReturn = 0;

        static ulong GenerateRandomLong()
        {
            //ulong a, b, c, d;
            //a = (ulong)kostka.Next(ushort.MaxValue);
            //b = (ulong)kostka.Next(ushort.MaxValue);
            //c = (ulong)kostka.Next(ushort.MaxValue);
            //d = (ulong)kostka.Next(ushort.MaxValue);

            //ulong retNum = a * b * c * d;
            //return retNum;

            long result = kostka.Next((int.MinValue >> 32), (int.MaxValue >> 32));
            result = (result << 32);
            result = result | (long)kostka.Next(int.MinValue, int.MaxValue);
            return (ulong)result;

        }

        public static void InitializeHashValues()
        {
            for (int i = 0; i < 64; i++)
            {
                Pawn.WhiteHashValues[i] = GenerateRandomLong();
                Pawn.BlackHashValues[i] = GenerateRandomLong();
                Knight.WhiteHashValues[i] = GenerateRandomLong();
                Knight.BlackHashValues[i] = GenerateRandomLong();
                Bishop.WhiteHashValues[i] = GenerateRandomLong();
                Bishop.BlackHashValues[i] = GenerateRandomLong();
                Rook.WhiteHashValues[i] = GenerateRandomLong();
                Rook.BlackHashValues[i] = GenerateRandomLong();
                Queen.WhiteHashValues[i] = GenerateRandomLong();
                Queen.BlackHashValues[i] = GenerateRandomLong();
                King.WhiteHashValues[i] = GenerateRandomLong();
                King.BlackHashValues[i] = GenerateRandomLong();

                Board.EnPassantSquaresHash[i] = GenerateRandomLong();
            }

            Board.WhiteToMoveHashValue = GenerateRandomLong();
            Board.BlackToMoveHashValue = GenerateRandomLong();

            Board.WhiteKingCastleHashValue = GenerateRandomLong();
            Board.BlackKingCastleHashValue = GenerateRandomLong();
            Board.WhiteQueenCastleHashValue = GenerateRandomLong();
            Board.BlackQueenCastleHashValue = GenerateRandomLong();

        }

        public const ulong TTRecordNumber = 2000000;

        public static TTRecord[] TranspositionTable = new TTRecord[TTRecordNumber];

        static void SaveInTT(Board currentBoard, Board bestMoveBoard, int depth, NodeType nodeType)
        {
            TTRecord record = TranspositionTable[currentBoard.HashValue % TTRecordNumber];

            if ((depth > record.Depth) && (depth >= 0) || (depth == record.Depth && record.NodeType != NodeType.Exact && nodeType == NodeType.Exact))
            {
                record.Depth = depth;
                record.Score = bestMoveBoard.EvaluatedValue;
                record.NodeType = nodeType;
                record.BestMoveStartSquare = bestMoveBoard.StartSquare;
                record.BestMoveFinishSquare = bestMoveBoard.FinishSquare;
                record.ZorbistKey = currentBoard.HashValue;
                record.Path = new List<string>(bestMoveBoard.Path);
            }

        }

        static TTRecord GetTTRecord(Board board)
        {
            TTRecord record;
            record = TranspositionTable[board.HashValue % TTRecordNumber];
            if (record.ZorbistKey == board.HashValue)
                return record;
            return null;
        }

        static void SendMoveToTop(Board board, ExtandCoords? startSquare, ExtandCoords? finishSquare)
        {
            Board firstBoard = null;

            firstBoard = board._possibleBoardsList.Find(
                x => (x.StartSquare == startSquare) &&
                (x.FinishSquare == finishSquare));

            if (firstBoard != null)
            {
                Board item = firstBoard;
                board._possibleBoardsList.Remove(item);
                board._possibleBoardsList.Insert(0, item);
            }

        }

        static void AddAge()
        {
            for (ulong i = 0; i < TTRecordNumber; i++)
            {
                TranspositionTable[i].Depth -= 1;
            }
        }

        //==================================================================================
        //========================= RECURSIVE ALGORITHMS ===================================
        //==================================================================================

        static Board MinMax(Board board, Agent agent, int depth)
        {
            board.GenerateBoards();
            if (depth == 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                return board;
            }
            else
            {
                Board bestScore = board;

                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = MinMax(nextBoard, SwitchAgent(agent), depth - 1);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;
                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = MinMax(nextBoard, SwitchAgent(agent), depth - 1);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;
                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
            }
        }

        static Board AlphaBeta(Board board, Agent agent, int depth, double alfa, double beta)
        {
            board.GenerateBoards();
            if (depth == 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                return board;
            }
            else
            {
                Board bestScore = board;

                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = AlphaBeta(nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue > alfa)
                                alfa = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;
                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = AlphaBeta(nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue < beta)
                                beta = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;
                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
            }
        }

        static Board KillerMoves(Board board, Agent agent, int depth, double alfa, double beta)
        {
            board.GenerateBoards();
            if (depth == 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                return board;
            }
            else
            {
                Board bestScore = board;

                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {

                        Board currentScore = KillerMoves(nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue > alfa)
                                alfa = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);

                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    KillerSort(board, depth);


                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = KillerMoves(nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue < beta)
                                beta = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);

                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
            }
        }

        static Board KillerMovesWithSort(Board board, Agent agent, int depth, double alfa, double beta)
        {
            board.GenerateBoards();
            if (depth == 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                return board;
            }
            else
            {
                Board bestScore = board;

                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    StaticSort(board);

                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {

                        Board currentScore = KillerMovesWithSort(nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue > alfa)
                                alfa = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);

                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    StaticSort(board);

                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = KillerMovesWithSort(nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue < beta)
                                beta = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);

                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
            }
        }

        static Board HistoryTable(Board board, Agent agent, int depth, double alfa, double beta)
        {
            board.GenerateBoards();
            if (depth == 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                return board;
            }
            else
            {
                Board bestScore = board;

                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    HistoryTableSort();
                    HistoryMovesSort(board);
                    StaticSort(board);
                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {

                        Board currentScore = HistoryTable(nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue > alfa)
                                alfa = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);
                                if (!bestScore.IsCapturePosition)
                                {
                                    FillHistoryTable(depth, bestScore);
                                }
                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    //FillHistoryTable(depth, bestScore);
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    HistoryTableSort();
                    HistoryMovesSort(board);
                    StaticSort(board);
                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = HistoryTable(nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue < beta)
                                beta = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);
                                if (!bestScore.IsCapturePosition)
                                {
                                    FillHistoryTable(depth, bestScore);
                                }
                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    //FillHistoryTable(depth, bestScore);
                    return bestScore;
                }
            }
        }

        static Board NullMove(Board board, Agent agent, int depth, double alfa, double beta)
        {
            board.GenerateBoards();
            if (depth <= 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                return board;
            }
            else
            {
                Board bestScore = board;

                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    if (Evaluation.GetGameState(board) != GameStage.Enidng && !board.IsKingInCheck && isNullMoveOk)
                    {
                        isNullMoveOk = false;
                        Board sameBoard = new Board();
                        sameBoard.GetValuesFromBoard(board);
                        Board nullMoveEvaluation = NullMove(sameBoard, SwitchAgent(agent), depth - 3, alfa, alfa + 1);

                        if (nullMoveEvaluation.EvaluatedValue >= beta)
                        {
                            return nullMoveEvaluation;
                        }
                    }
                    else
                        isNullMoveOk = true;

                    StaticSort(board);
                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {

                        Board currentScore = NullMove(nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue > alfa)
                                alfa = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);

                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    if (Evaluation.GetGameState(board) != GameStage.Enidng && !board.IsKingInCheck && isNullMoveOk)
                    {
                        isNullMoveOk = false;
                        Board sameBoard = new Board();
                        sameBoard.GetValuesFromBoard(board);
                        Board nullMoveEvaluation = NullMove(sameBoard, SwitchAgent(agent), depth - 3, beta - 1, beta);

                        if (nullMoveEvaluation.EvaluatedValue <= alfa)
                        {
                            return nullMoveEvaluation;
                        }
                    }
                    else
                        isNullMoveOk = true;

                    StaticSort(board);
                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = NullMove(nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue < beta)
                                beta = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);

                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
            }
        }

        static Board TT(Board board, Agent agent, int depth, double alfa, double beta)
        {
            Board bestScore = new Board();

            TTRecord record = GetTTRecord(board);
            if (record != null)
            {
                if (depth == record.Depth)
                {
                    if (record.NodeType == NodeType.Exact)
                    {
                        board.EvaluatedValue = record.Score;
                        SkippedEvaluations++;
                        return board;
                    }
                }
            }

            board.GenerateBoards();
            if (depth <= 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                SaveInTT(board, board, 0, NodeType.Exact);
                return board;
            }
            else
            {


                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    #region NullMoveMax

                    if (Evaluation.GetGameState(board) != GameStage.Enidng && !board.IsKingInCheck && isNullMoveOk)
                    {
                        isNullMoveOk = false;
                        Board sameBoard = new Board();
                        sameBoard.GetValuesFromBoard(board);
                        Board nullMoveEvaluation = TT(sameBoard, SwitchAgent(agent), depth - 3, alfa, alfa + 1);

                        if (nullMoveEvaluation.EvaluatedValue >= beta)
                        {
                            return nullMoveEvaluation;
                        }
                    }
                    else
                        isNullMoveOk = true;

                    #endregion

                    StaticSort(board);
                    KillerSort(board, depth);

                    #region TT

                    if (record != null)
                    {
                        if (depth <= record.Depth)
                        {
                            if (record.NodeType == NodeType.Exact)
                            {
                                bestScore.EvaluatedValue = record.Score;
                                bestScore.Path = new List<string>(record.Path);
                                bestScore.StartSquare = record.BestMoveStartSquare;
                                bestScore.FinishSquare = record.BestMoveFinishSquare;
                                return bestScore;
                            }
                            else
                            {
                                //bestScore.EvaluatedValue = record.Score;
                                //bestScore.Path = new List<string>(record.Path);
                                //bestScore.StartSquare = record.BestMoveStartSquare;
                                //bestScore.FinishSquare = record.BestMoveFinishSquare;
                                SendMoveToTop(board, record.BestMoveStartSquare, record.BestMoveFinishSquare);
                            }
                        }
                        else
                        {
                            SendMoveToTop(board, record.BestMoveStartSquare, record.BestMoveFinishSquare);
                        }
                    }

                    #endregion

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {

                        Board currentScore = TT(nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;
                        }
                        if (bestScore.EvaluatedValue > alfa)
                            alfa = bestScore.EvaluatedValue;
                        if (alfa >= beta)
                        {
                            AlfaBetaCuttOff++;
                            KillerSave(depth, bestScore);

                            SaveInTT(board, bestScore, depth, NodeType.Maximizer);

                            board._possibleBoardsList.Clear();
                            return bestScore;
                        }



                    }
                    board._possibleBoardsList.Clear();
                    SaveInTT(board, bestScore, depth, NodeType.Exact);
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    #region NullMoveMin

                    if (Evaluation.GetGameState(board) != GameStage.Enidng && !board.IsKingInCheck && isNullMoveOk)
                    {
                        isNullMoveOk = false;
                        Board sameBoard = new Board();
                        sameBoard.GetValuesFromBoard(board);
                        Board nullMoveEvaluation = TT(sameBoard, SwitchAgent(agent), depth - 3, beta - 1, beta);

                        if (nullMoveEvaluation.EvaluatedValue <= alfa)
                        {
                            return nullMoveEvaluation;
                        }
                    }
                    else
                        isNullMoveOk = true;

                    #endregion

                    StaticSort(board);
                    KillerSort(board, depth);

                    #region TT

                    if (record != null)
                    {
                        if (depth <= record.Depth)
                        {
                            if (record.NodeType == NodeType.Exact)
                            {
                                bestScore.EvaluatedValue = record.Score;
                                bestScore.Path = new List<string>(record.Path);
                                bestScore.StartSquare = record.BestMoveStartSquare;
                                bestScore.FinishSquare = record.BestMoveFinishSquare;
                                return bestScore;
                            }
                            else
                            {
                                //bestScore.EvaluatedValue = record.Score;
                                //bestScore.Path = new List<string>(record.Path);
                                //bestScore.StartSquare = record.BestMoveStartSquare;
                                //bestScore.FinishSquare = record.BestMoveFinishSquare;
                                SendMoveToTop(board, record.BestMoveStartSquare, record.BestMoveFinishSquare);
                            }
                        }
                        else
                        {
                            SendMoveToTop(board, record.BestMoveStartSquare, record.BestMoveFinishSquare);
                        }
                    }

                    #endregion

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = TT(nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;
                        }
                        if (bestScore.EvaluatedValue < beta)
                            beta = bestScore.EvaluatedValue;
                        if (alfa >= beta)
                        {
                            AlfaBetaCuttOff++;
                            KillerSave(depth, bestScore);

                            SaveInTT(board, bestScore, depth, NodeType.Minimizer);

                            board._possibleBoardsList.Clear();
                            return bestScore;
                        }



                    }
                    board._possibleBoardsList.Clear();
                    SaveInTT(board, bestScore, depth, NodeType.Exact);
                    return bestScore;
                }
            }
        }

        //public static DummyBoard MinMaxTest(DummyBoard board, string Agent, int depth)
        //{
        //    board.GetPossibleBoards();
        //    if (depth == 0 || board.PossibleBoards.Count == 0)
        //    {
        //        EvaluationCounter++;
        //        board.Value = 1;
        //        return board;
        //    }
        //    else
        //    {
        //        DummyBoard bestScore = new DummyBoard();

        //        if (Agent == "max")
        //        {
        //            bestScore.Value = -999;

        //            foreach (DummyBoard nextBoard in board.PossibleBoards)
        //            {
        //                DummyBoard currentScore = MinMaxTest(nextBoard, SwitchAgent(Agent), depth - 1);
        //                if (currentScore.Value > bestScore.Value)
        //                {
        //                    bestScore = currentScore;
        //                }

        //            }
        //            board.PossibleBoards.Clear();
        //            return bestScore;
        //        }
        //        else
        //        {
        //            bestScore.Value = 999;

        //            foreach (DummyBoard nextBoard in board.PossibleBoards)
        //            {
        //                DummyBoard currentScore = MinMaxTest(nextBoard, SwitchAgent(Agent), depth - 1);
        //                if (currentScore.Value < bestScore.Value)
        //                {
        //                    bestScore = currentScore;
        //                }

        //            }
        //            board.PossibleBoards.Clear();
        //            return bestScore;
        //        }
        //    }
        //}

        /// <summary>
        /// Performing a min-max search based on a given position. Returns dedicated type which holds usefull information.
        /// </summary>
        /// <param name="board">Board to analyse.</param>
        /// <param name="depth">Maximum depth of algorithm search.</param>
        public static BestMoveInfo MinMaxSearch(Board board, int depth)
        {
            Board CopyBoard = new Board();
            CopyBoard.GetValuesFromBoard(board);

            Agent agent;

            if (CopyBoard.SideToMove == Side.White)
                agent = Agent.Maximizer;
            else
                agent = Agent.Minimizer;

            stoper.Start();

            Board wynikowa = MinMax(CopyBoard, agent, depth);

            stoper.Stop();

            BestMoveInfo bmi = new BestMoveInfo();
            bmi.StartSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(0, 2));
            bmi.FinishSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(7, 2));
            bmi.NumberOfAlfaBetaCutOffs = 0;
            bmi.NumberOfPositionEvaluated = EvaluationCounter;
            bmi.Time = Convert.ToInt32(stoper.ElapsedMilliseconds / 1000) + 1;
            bmi.SearchDepth = depth;
            bmi.PromotionSign = wynikowa.Path[board.Path.Count][12];
            bmi.NumberOfEstimateMinMaxPositionCount = (ulong)bmi.NumberOfPositionEvaluated;
            bmi.NumberOfEvaluationReduction = 0;
            bmi.Speed = bmi.NumberOfPositionEvaluated / bmi.Time;
            bmi.AlgorithmName = "min-max";
            bmi.SavedPercentage = (double)bmi.NumberOfEvaluationReduction * 100 / bmi.NumberOfEstimateMinMaxPositionCount;

            for (int i = board.Path.Count; i < wynikowa.Path.Count; i++)
            {
                bmi.Path += $"{wynikowa.Path[i]}\n";
            }

            EvaluationCounter = 0;
            AlfaBetaCuttOff = 0;
            stoper.Reset();

            return bmi;
        }

        /// <summary>
        /// Performing an alpha-beta search based on a given position. Returns dedicated type which holds usefull information.
        /// </summary>
        /// <param name="board">Board to analyse.</param>
        /// <param name="depth">Maximum depth of algorithm search.</param>
        public static BestMoveInfo AlphaBetaSearch(Board board, int depth)
        {
            Board CopyBoard = new Board();
            CopyBoard.GetValuesFromBoard(board);

            Agent agent;

            if (CopyBoard.SideToMove == Side.White)
                agent = Agent.Maximizer;
            else
                agent = Agent.Minimizer;

            stoper.Start();

            Board wynikowa = AlphaBeta(CopyBoard, agent, depth, -9999, 9999);

            stoper.Stop();

            BestMoveInfo bmi = new BestMoveInfo();
            bmi.StartSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(0, 2));
            bmi.FinishSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(7, 2));
            bmi.NumberOfAlfaBetaCutOffs = AlfaBetaCuttOff;
            bmi.NumberOfPositionEvaluated = EvaluationCounter;
            bmi.Time = Convert.ToInt32(stoper.ElapsedMilliseconds / 1000) + 1;
            bmi.SearchDepth = depth;
            bmi.PromotionSign = wynikowa.Path[board.Path.Count][12];
            bmi.NumberOfEstimateMinMaxPositionCount = (ulong)Math.Pow(board._possibleBoardsList.Count, depth);
            bmi.NumberOfEvaluationReduction = (int)(bmi.NumberOfEstimateMinMaxPositionCount - (ulong)bmi.NumberOfPositionEvaluated);
            bmi.Speed = bmi.NumberOfPositionEvaluated / bmi.Time;
            bmi.AlgorithmName = "alfa-beta";
            bmi.SavedPercentage = (double)bmi.NumberOfEvaluationReduction * 100 / bmi.NumberOfEstimateMinMaxPositionCount;

            for (int i = board.Path.Count; i < wynikowa.Path.Count; i++)
            {
                bmi.Path += $"{wynikowa.Path[i]}\n";
            }



            EvaluationCounter = 0;
            AlfaBetaCuttOff = 0;
            stoper.Reset();

            return bmi;
        }

        /// <summary>
        /// Performing an alpha-beta search with a killer moves heuristic based on a given position. 
        /// Returns dedicated type which holds usefull information.
        /// </summary>
        /// <param name="board">Board to analyse.</param>
        /// <param name="depth">Maximum depth of algorithm search.</param>
        public static BestMoveInfo KillerSearch(Board board, int depth)
        {
            Board CopyBoard = new Board();
            CopyBoard.GetValuesFromBoard(board);

            //KillerMoveDown();

            Agent agent;

            if (CopyBoard.SideToMove == Side.White)
                agent = Agent.Maximizer;
            else
                agent = Agent.Minimizer;

            stoper.Start();

            Board wynikowa = KillerMoves(CopyBoard, agent, depth, -9999, 9999);

            stoper.Stop();

            BestMoveInfo bmi = new BestMoveInfo();
            bmi.StartSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(0, 2));
            bmi.FinishSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(7, 2));
            bmi.NumberOfAlfaBetaCutOffs = AlfaBetaCuttOff;
            bmi.NumberOfPositionEvaluated = EvaluationCounter;
            bmi.Time = Convert.ToInt32(stoper.ElapsedMilliseconds / 1000) + 1;
            bmi.SearchDepth = depth;
            bmi.PromotionSign = wynikowa.Path[board.Path.Count][12];
            bmi.NumberOfEstimateMinMaxPositionCount = (ulong)Math.Pow(board._possibleBoardsList.Count, depth);
            bmi.NumberOfEvaluationReduction = (int)(bmi.NumberOfEstimateMinMaxPositionCount - (ulong)bmi.NumberOfPositionEvaluated);
            bmi.Speed = bmi.NumberOfPositionEvaluated / bmi.Time;
            bmi.AlgorithmName = "alfa-beta with killer moves";
            bmi.SavedPercentage = (double)bmi.NumberOfEvaluationReduction * 100 / bmi.NumberOfEstimateMinMaxPositionCount;

            for (int i = board.Path.Count; i < wynikowa.Path.Count; i++)
            {
                bmi.Path += $"{wynikowa.Path[i]}\n";
            }

            EvaluationCounter = 0;
            AlfaBetaCuttOff = 0;
            stoper.Reset();

            return bmi;
        }

        /// <summary>
        /// Performing an alpha-beta search with a killer moves heuristic with additional sort based on a given position. 
        /// Returns dedicated type which holds usefull information. Checks, captures and promotions are sorted as 
        /// first to check.
        /// </summary>
        /// <param name="board">Board to analyse.</param>
        /// <param name="depth">Maximum depth of algorithm search.</param>
        public static BestMoveInfo KillerWithSortSearch(Board board, int depth)
        {
            Board CopyBoard = new Board();
            CopyBoard.GetValuesFromBoard(board);

            //KillerMoveDown();

            Agent agent;

            if (CopyBoard.SideToMove == Side.White)
                agent = Agent.Maximizer;
            else
                agent = Agent.Minimizer;

            stoper.Start();

            Board wynikowa = KillerMovesWithSort(CopyBoard, agent, depth, -9999, 9999);

            stoper.Stop();

            BestMoveInfo bmi = new BestMoveInfo();
            bmi.StartSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(0, 2));
            bmi.FinishSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(7, 2));
            bmi.NumberOfAlfaBetaCutOffs = AlfaBetaCuttOff;
            bmi.NumberOfPositionEvaluated = EvaluationCounter;
            bmi.Time = Convert.ToInt32(stoper.ElapsedMilliseconds / 1000) + 1;
            bmi.SearchDepth = depth;
            bmi.PromotionSign = wynikowa.Path[board.Path.Count][12];
            bmi.NumberOfEstimateMinMaxPositionCount = (ulong)Math.Pow(board._possibleBoardsList.Count, depth);
            bmi.NumberOfEvaluationReduction = (int)(bmi.NumberOfEstimateMinMaxPositionCount - (ulong)bmi.NumberOfPositionEvaluated);
            bmi.Speed = bmi.NumberOfPositionEvaluated / bmi.Time;
            bmi.AlgorithmName = "alfa-beta with killer moves + static sort";
            bmi.SavedPercentage = (double)bmi.NumberOfEvaluationReduction * 100 / bmi.NumberOfEstimateMinMaxPositionCount;

            for (int i = board.Path.Count; i < wynikowa.Path.Count; i++)
            {
                bmi.Path += $"{wynikowa.Path[i]}\n";
            }

            EvaluationCounter = 0;
            AlfaBetaCuttOff = 0;
            stoper.Reset();

            return bmi;
        }

        /// <summary>
        /// Performing an alpha-beta search with a killer moves heuristic with additional sort based on a given position.
        /// Also this algorithm is using heuristic know as history tables.
        /// Returns dedicated type which holds usefull information. Checks, captures and promotions are sorted as 
        /// first to check.
        /// </summary>
        /// <param name="board">Board to analyse.</param>
        /// <param name="depth">Maximum depth of algorithm search.</param>
        public static BestMoveInfo HistorySearch(Board board, int depth)
        {
            Board CopyBoard = new Board();
            CopyBoard.GetValuesFromBoard(board);

            //KillerMoveDown();
            foreach (HistoryMove hisMove in TablicaHistorii)
            {
                hisMove.Value /= 2;
            }

            Agent agent;

            if (CopyBoard.SideToMove == Side.White)
                agent = Agent.Maximizer;
            else
                agent = Agent.Minimizer;

            stoper.Start();

            Board wynikowa = HistoryTable(CopyBoard, agent, depth, -9999, 9999);

            stoper.Stop();

            BestMoveInfo bmi = new BestMoveInfo();
            bmi.StartSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(0, 2));
            bmi.FinishSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(7, 2));
            bmi.NumberOfAlfaBetaCutOffs = AlfaBetaCuttOff;
            bmi.NumberOfPositionEvaluated = EvaluationCounter;
            bmi.Time = Convert.ToInt32(stoper.ElapsedMilliseconds / 1000) + 1;
            bmi.SearchDepth = depth;
            bmi.PromotionSign = wynikowa.Path[board.Path.Count][12];
            bmi.NumberOfEstimateMinMaxPositionCount = (ulong)Math.Pow(board._possibleBoardsList.Count, depth);
            bmi.NumberOfEvaluationReduction = (int)bmi.NumberOfEstimateMinMaxPositionCount - bmi.NumberOfPositionEvaluated;
            bmi.Speed = bmi.NumberOfPositionEvaluated / bmi.Time;
            bmi.AlgorithmName = "alfa-beta with killer moves + static sort + history table";
            bmi.SavedPercentage = (double)bmi.NumberOfEvaluationReduction * 100 / bmi.NumberOfEstimateMinMaxPositionCount;

            for (int i = board.Path.Count; i < wynikowa.Path.Count; i++)
            {
                bmi.Path += $"{wynikowa.Path[i]}\n";
            }

            EvaluationCounter = 0;
            AlfaBetaCuttOff = 0;
            stoper.Reset();

            return bmi;
        }

        /// <summary>
        /// Performing an alpha-beta search with a killer moves heuristic with additional sort based on a given position. 
        /// Returns dedicated type which holds usefull information. Checks, captures and promotions are sorted as 
        /// first to check. It also provides a Null-move heuristic.
        /// </summary>
        /// <param name="board">Board to analyse.</param>
        /// <param name="depth">Maximum depth of algorithm search.</param>
        public static BestMoveInfo NullMoveSearch(Board board, int depth)
        {
            Board CopyBoard = new Board();
            CopyBoard.GetValuesFromBoard(board);
            isNullMoveOk = true;
            //KillerMoveDown();

            Agent agent;

            if (CopyBoard.SideToMove == Side.White)
                agent = Agent.Maximizer;
            else
                agent = Agent.Minimizer;

            stoper.Start();

            Board wynikowa = NullMove(CopyBoard, agent, depth, -9999, 9999);

            stoper.Stop();

            BestMoveInfo bmi = new BestMoveInfo();
            bmi.StartSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(0, 2));
            bmi.FinishSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(7, 2));
            bmi.NumberOfAlfaBetaCutOffs = AlfaBetaCuttOff;
            bmi.NumberOfPositionEvaluated = EvaluationCounter;
            bmi.Time = Convert.ToInt32(stoper.ElapsedMilliseconds / 1000) + 1;
            bmi.SearchDepth = depth;
            bmi.PromotionSign = wynikowa.Path[board.Path.Count][12];
            bmi.NumberOfEstimateMinMaxPositionCount = (ulong)Math.Pow(board._possibleBoardsList.Count, depth);
            bmi.NumberOfEvaluationReduction = (int)bmi.NumberOfEstimateMinMaxPositionCount - bmi.NumberOfPositionEvaluated;
            bmi.Speed = bmi.NumberOfPositionEvaluated / bmi.Time;
            bmi.AlgorithmName = "alfa-beta with killer moves + static sort + null-move";
            bmi.SavedPercentage = (double)bmi.NumberOfEvaluationReduction * 100 / bmi.NumberOfEstimateMinMaxPositionCount;

            for (int i = board.Path.Count; i < wynikowa.Path.Count; i++)
            {
                bmi.Path += $"{wynikowa.Path[i]}\n";
            }

            EvaluationCounter = 0;
            AlfaBetaCuttOff = 0;
            stoper.Reset();

            return bmi;
        }

        /// <summary>
        /// Performing an ultimate search with use of Transposition Tables.
        /// </summary>
        public static BestMoveInfo TTSearch(Board board, int depth)
        {
            Board CopyBoard = new Board();
            CopyBoard.GetValuesFromBoard(board);
            isNullMoveOk = true;
            //KillerMoveDown();

            Agent agent;

            if (CopyBoard.SideToMove == Side.White)
                agent = Agent.Maximizer;
            else
                agent = Agent.Minimizer;

            stoper.Start();

            Board wynikowa = TT(CopyBoard, agent, depth, -9999, 9999);

            stoper.Stop();

            BestMoveInfo bmi = new BestMoveInfo();
            bmi.StartSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(0, 2));
            bmi.FinishSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(7, 2));
            bmi.NumberOfAlfaBetaCutOffs = AlfaBetaCuttOff;
            bmi.NumberOfPositionEvaluated = EvaluationCounter;
            bmi.Time = Convert.ToInt32(stoper.ElapsedMilliseconds / 1000) + 1;
            bmi.SearchDepth = depth;
            bmi.PromotionSign = wynikowa.Path[board.Path.Count][12];
            bmi.NumberOfEstimateMinMaxPositionCount = (ulong)Math.Pow(board._possibleBoardsList.Count, depth);
            bmi.NumberOfEvaluationReduction = (int)bmi.NumberOfEstimateMinMaxPositionCount - bmi.NumberOfPositionEvaluated;
            bmi.Speed = bmi.NumberOfPositionEvaluated / bmi.Time;
            bmi.AlgorithmName = "Transposition Tables search";
            bmi.SavedPercentage = (double)bmi.NumberOfEvaluationReduction * 100 / bmi.NumberOfEstimateMinMaxPositionCount;

            for (int i = board.Path.Count; i < wynikowa.Path.Count; i++)
            {
                bmi.Path += $"{wynikowa.Path[i]}\n";
            }

            EvaluationCounter = 0;
            AlfaBetaCuttOff = 0;
            SkippedEvaluations = 0;
            stoper.Reset();
            //AddAge();

            return bmi;
        }

        //============================================================================================================
        //============================================ BACKGROUND WORKER =============================================
        //============================================================================================================

        public delegate Board SearchAlgorithm(BackgroundWorker bw, Board board, Agent agent, int depth, double alfa, double beta);

        public static BestMoveInfo BWSearch(BackgroundWorker bw, string algorithmName, SearchAlgorithm algorithm, Board board, int depth)
        {
            Board CopyBoard = new Board();
            CopyBoard.GetValuesFromBoard(board);
            isNullMoveOk = true;

            foreach (HistoryMove hisMove in TablicaHistorii)
            {
                hisMove.Value /= 2;
            }

            Agent agent;

            if (CopyBoard.SideToMove == Side.White)
                agent = Agent.Maximizer;
            else
                agent = Agent.Minimizer;

            stoper.Start();

            Board wynikowa = algorithm(bw, CopyBoard, agent, depth, -9999, 9999);

            stoper.Stop();

            BestMoveInfo bmi = new BestMoveInfo();
            bmi.StartSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(0, 2));
            bmi.FinishSquare = (Coords)Enum.Parse(typeof(Coords), wynikowa.Path[board.Path.Count].Substring(7, 2));
            bmi.NumberOfAlfaBetaCutOffs = AlfaBetaCuttOff;
            bmi.NumberOfPositionEvaluated = EvaluationCounter;
            bmi.Time = Convert.ToInt32(stoper.ElapsedMilliseconds);
            bmi.SearchDepth = depth;
            bmi.PromotionSign = wynikowa.Path[board.Path.Count][12];

            if(algorithmName == "Min-Max")
            {
                bmi.NumberOfEstimateMinMaxPositionCount = (ulong)EvaluationCounter;
                bmi.NumberOfEvaluationReduction = 0;
            }
            else
            {
                bmi.NumberOfEstimateMinMaxPositionCount = (ulong)Math.Pow(board._possibleBoardsList.Count, depth);
                bmi.NumberOfEvaluationReduction = (int)bmi.NumberOfEstimateMinMaxPositionCount - bmi.NumberOfPositionEvaluated;
            }

            bmi.Speed = bmi.NumberOfPositionEvaluated*1000 / bmi.Time;
            bmi.AlgorithmName = algorithmName;
            bmi.SavedPercentage = (double)bmi.NumberOfEvaluationReduction * 100 / bmi.NumberOfEstimateMinMaxPositionCount;

            for (int i = board.Path.Count; i < wynikowa.Path.Count; i++)
            {
                bmi.Path += $"{wynikowa.Path[i]}\n";
            }

            EvaluationCounter = 0;
            AlfaBetaCuttOff = 0;
            SkippedEvaluations = 0;
            stoper.Reset();

            return bmi;
        }

        public static Board MinMaxBW(BackgroundWorker bw, Board board, Agent agent, int depth, double alfa, double beta)
        {
            if (bw.CancellationPending)
                return new Board();

            board.GenerateBoards();
            if (depth == 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                return board;
            }
            else
            {
                Board bestScore = board;

                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = MinMaxBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;
                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = MinMaxBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;
                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
            }
        }

        public static Board AlphaBetaBW(BackgroundWorker bw, Board board, Agent agent, int depth, double alfa, double beta)
        {
            if (bw.CancellationPending)
                return new Board();

            board.GenerateBoards();
            if (depth == 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                return board;
            }
            else
            {
                Board bestScore = board;

                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = AlphaBetaBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue > alfa)
                                alfa = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;
                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = AlphaBetaBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue < beta)
                                beta = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;
                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
            }
        }

        public static Board KillerMovesBW(BackgroundWorker bw, Board board, Agent agent, int depth, double alfa, double beta)
        {
            board.GenerateBoards();
            if (depth == 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                return board;
            }
            else
            {
                Board bestScore = board;

                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {

                        Board currentScore = KillerMovesBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue > alfa)
                                alfa = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);

                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    KillerSort(board, depth);


                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = KillerMovesBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue < beta)
                                beta = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);

                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
            }
        }

        public static Board KillerMovesWithSortBW(BackgroundWorker bw, Board board, Agent agent, int depth, double alfa, double beta)
        {
            board.GenerateBoards();
            if (depth == 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                return board;
            }
            else
            {
                Board bestScore = board;

                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    StaticSort(board);

                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {

                        Board currentScore = KillerMovesWithSortBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue > alfa)
                                alfa = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);

                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    StaticSort(board);

                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = KillerMovesWithSortBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue < beta)
                                beta = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);

                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
            }
        }

        public static Board HistoryTableBW(BackgroundWorker bw, Board board, Agent agent, int depth, double alfa, double beta)
        {
            board.GenerateBoards();
            if (depth == 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                return board;
            }
            else
            {
                Board bestScore = board;

                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    HistoryTableSort();
                    HistoryMovesSort(board);
                    StaticSort(board);
                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {

                        Board currentScore = HistoryTableBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue > alfa)
                                alfa = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);
                                if (!bestScore.IsCapturePosition)
                                {
                                    FillHistoryTable(depth, bestScore);
                                }
                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    HistoryTableSort();
                    HistoryMovesSort(board);
                    StaticSort(board);
                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = HistoryTableBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue < beta)
                                beta = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);
                                if (!bestScore.IsCapturePosition)
                                {
                                    FillHistoryTable(depth, bestScore);
                                }
                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
            }
        }

        public static Board NullMoveBW(BackgroundWorker bw, Board board, Agent agent, int depth, double alfa, double beta)
        {
            board.GenerateBoards();
            if (depth <= 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                return board;
            }
            else
            {
                Board bestScore = board;

                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    if (Evaluation.GetGameState(board) != GameStage.Enidng && !board.IsKingInCheck && isNullMoveOk)
                    {
                        isNullMoveOk = false;
                        Board sameBoard = new Board();
                        sameBoard.GetValuesFromBoard(board);
                        Board nullMoveEvaluation = NullMoveBW(bw, sameBoard, SwitchAgent(agent), depth - 3, alfa, alfa + 1);

                        if (nullMoveEvaluation.EvaluatedValue >= beta)
                        {
                            return nullMoveEvaluation;
                        }
                    }
                    else
                        isNullMoveOk = true;

                    StaticSort(board);
                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {

                        Board currentScore = NullMoveBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue > alfa)
                                alfa = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);

                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    if (Evaluation.GetGameState(board) != GameStage.Enidng && !board.IsKingInCheck && isNullMoveOk)
                    {
                        isNullMoveOk = false;
                        Board sameBoard = new Board();
                        sameBoard.GetValuesFromBoard(board);
                        Board nullMoveEvaluation = NullMoveBW(bw, sameBoard, SwitchAgent(agent), depth - 3, beta - 1, beta);

                        if (nullMoveEvaluation.EvaluatedValue <= alfa)
                        {
                            return nullMoveEvaluation;
                        }
                    }
                    else
                        isNullMoveOk = true;

                    StaticSort(board);
                    KillerSort(board, depth);

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = NullMoveBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;

                            if (bestScore.EvaluatedValue < beta)
                                beta = bestScore.EvaluatedValue;
                            if (alfa >= beta)
                            {
                                AlfaBetaCuttOff++;

                                KillerSave(depth, bestScore);

                                break;
                            }

                        }

                    }
                    board._possibleBoardsList.Clear();
                    return bestScore;
                }
            }
        }

        public static Board TTBW(BackgroundWorker bw, Board board, Agent agent, int depth, double alfa, double beta)
        {
            Board bestScore = new Board();

            TTRecord record = GetTTRecord(board);
            if (record != null)
            {
                if (depth == record.Depth)
                {
                    if (record.NodeType == NodeType.Exact)
                    {
                        board.EvaluatedValue = record.Score;
                        SkippedEvaluations++;
                        return board;
                    }
                }
            }

            board.GenerateBoards();
            if (depth <= 0 || board._possibleBoardsList.Count == 0)
            {
                EvaluationCounter++;
                board.EvaluatedValue = Evaluation.EvaluatePosition(board);
                SaveInTT(board, board, 0, NodeType.Exact);
                return board;
            }
            else
            {


                if (agent == Agent.Maximizer)
                {
                    bestScore.EvaluatedValue = -9999;

                    #region NullMoveMax

                    if (Evaluation.GetGameState(board) != GameStage.Enidng && !board.IsKingInCheck && isNullMoveOk)
                    {
                        isNullMoveOk = false;
                        Board sameBoard = new Board();
                        sameBoard.GetValuesFromBoard(board);
                        Board nullMoveEvaluation = TTBW(bw, sameBoard, SwitchAgent(agent), depth - 3, alfa, alfa + 1);

                        if (nullMoveEvaluation.EvaluatedValue >= beta)
                        {
                            return nullMoveEvaluation;
                        }
                    }
                    else
                        isNullMoveOk = true;

                    #endregion

                    StaticSort(board);
                    KillerSort(board, depth);

                    #region TT

                    if (record != null)
                    {
                        if (depth <= record.Depth)
                        {
                            if (record.NodeType == NodeType.Exact)
                            {
                                bestScore.EvaluatedValue = record.Score;
                                bestScore.Path = new List<string>(record.Path);
                                bestScore.StartSquare = record.BestMoveStartSquare;
                                bestScore.FinishSquare = record.BestMoveFinishSquare;
                                return bestScore;
                            }
                            else
                            {
                                //bestScore.EvaluatedValue = record.Score;
                                //bestScore.Path = new List<string>(record.Path);
                                //bestScore.StartSquare = record.BestMoveStartSquare;
                                //bestScore.FinishSquare = record.BestMoveFinishSquare;
                                SendMoveToTop(board, record.BestMoveStartSquare, record.BestMoveFinishSquare);
                            }
                        }
                        else
                        {
                            SendMoveToTop(board, record.BestMoveStartSquare, record.BestMoveFinishSquare);
                        }
                    }

                    #endregion

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {

                        Board currentScore = TTBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue > bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;
                        }
                        if (bestScore.EvaluatedValue > alfa)
                            alfa = bestScore.EvaluatedValue;
                        if (alfa >= beta)
                        {
                            AlfaBetaCuttOff++;
                            KillerSave(depth, bestScore);

                            SaveInTT(board, bestScore, depth, NodeType.Maximizer);

                            board._possibleBoardsList.Clear();
                            return bestScore;
                        }



                    }
                    board._possibleBoardsList.Clear();
                    SaveInTT( board, bestScore, depth, NodeType.Exact);
                    return bestScore;
                }
                else
                {
                    bestScore.EvaluatedValue = 9999;

                    #region NullMoveMin

                    if (Evaluation.GetGameState(board) != GameStage.Enidng && !board.IsKingInCheck && isNullMoveOk)
                    {
                        isNullMoveOk = false;
                        Board sameBoard = new Board();
                        sameBoard.GetValuesFromBoard(board);
                        Board nullMoveEvaluation = TTBW(bw, sameBoard, SwitchAgent(agent), depth - 3, beta - 1, beta);

                        if (nullMoveEvaluation.EvaluatedValue <= alfa)
                        {
                            return nullMoveEvaluation;
                        }
                    }
                    else
                        isNullMoveOk = true;

                    #endregion

                    StaticSort(board);
                    KillerSort(board, depth);

                    #region TT

                    if (record != null)
                    {
                        if (depth <= record.Depth)
                        {
                            if (record.NodeType == NodeType.Exact)
                            {
                                bestScore.EvaluatedValue = record.Score;
                                bestScore.Path = new List<string>(record.Path);
                                bestScore.StartSquare = record.BestMoveStartSquare;
                                bestScore.FinishSquare = record.BestMoveFinishSquare;
                                return bestScore;
                            }
                            else
                            {
                                //bestScore.EvaluatedValue = record.Score;
                                //bestScore.Path = new List<string>(record.Path);
                                //bestScore.StartSquare = record.BestMoveStartSquare;
                                //bestScore.FinishSquare = record.BestMoveFinishSquare;
                                SendMoveToTop(board, record.BestMoveStartSquare, record.BestMoveFinishSquare);
                            }
                        }
                        else
                        {
                            SendMoveToTop(board, record.BestMoveStartSquare, record.BestMoveFinishSquare);
                        }
                    }

                    #endregion

                    foreach (Board nextBoard in board._possibleBoardsList)
                    {
                        Board currentScore = TTBW(bw, nextBoard, SwitchAgent(agent), depth - 1, alfa, beta);
                        if (currentScore.EvaluatedValue < bestScore.EvaluatedValue)
                        {
                            bestScore = currentScore;
                        }
                        if (bestScore.EvaluatedValue < beta)
                            beta = bestScore.EvaluatedValue;
                        if (alfa >= beta)
                        {
                            AlfaBetaCuttOff++;
                            KillerSave(depth, bestScore);

                            SaveInTT(board, bestScore, depth, NodeType.Minimizer);

                            board._possibleBoardsList.Clear();
                            return bestScore;
                        }



                    }
                    board._possibleBoardsList.Clear();
                    SaveInTT(board, bestScore, depth, NodeType.Exact);
                    return bestScore;
                }
            }
        }



    }

        public class DummyBoard
    {
        public int Value { get; set; } = 0;

        public List<DummyBoard> PossibleBoards = new List<DummyBoard>();

        public void GetPossibleBoards()
        {
            for (int i = 0; i < 35; i++)
            {
                PossibleBoards.Add(new DummyBoard());
            }
        }
    }

}
