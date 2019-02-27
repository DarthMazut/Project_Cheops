using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.AI
{
    /// <summary>
    /// Contains implementation of evaluation function.
    /// </summary>
    public static class Evaluation
    {

        public static Dictionary<char, double> _piecesValues = new Dictionary<char, double>()
        {
             { 'P', 1 },
             { 'p', -1 },
             { 'N', 3 },
             { 'n', -3 },
             { 'B', 3.2 },
             { 'b', -3.2 },
             { 'R', 4.8 },
             { 'r', -4.8},
             { 'Q', 9.2 },
             { 'q', -9.2 },
        };

        //==============================================================================================================
        //=================================== FUNKCJA EWALUACYJNA ======================================================
        //==============================================================================================================

        public const double w_blockedBishops = 0.3;
        public const double w_blockedRooks = 0.35;
        public const double w_Is8thRankMatePossible = 0.1;
        public const double w_doublePawns = 0.15;
        public const double w_isolatedPawns = 0.25;
        public const double w_minorPiecesAtEdges = 0.15;
        public const double w_directionsKingOpen = 0.15;
        public const double w_directionsKingOpenEnding = 0.05;
        public const double w_IsKingInCenter = 0.45;
        public const double w_KingSourrandingSquareAttack = 0.45;
        public const double w_PinnedPieces = 0.5;
        public const double w_PinnedPiecesCheck = 0.5;
        public const double w_CastleBlocked = 0.15;
        //============== NAGRODY ================
        public const double w_PawnChainLength = 0.1;
        public const double w_PawnInTheCenter = 0.25;
        public const double w_PawnAboutToPromote = 0.4;
        public const double w_PassPawns = 0.3;
        public const double w_MinorPieceInTheCenter = 0.3;
        public const double w_BishopPair = 0.25;
        public const double w_LongDiagonalBishop = 0.4;
        public const double w_SupportedKnights = 0.25;
        public const double w_AreRooksConnected = 0.5;
        public const double w_QueenInCenterOpening = 0.1;
        public const double w_QueenInCenterMidEnd = 0.15;
        public const double w_KingProtectingMaterial = 0.1;
        public const double w_BishopBattery = 0.15;
        public const double w_RookBattery = 0.20;



        //==============================================================================================================
        //========================================== WAGI ==============================================================
        //==============================================================================================================

        /// <summary>
        /// Returns a score based on given position.
        /// </summary>
        /// <param name="board">Position to analyse.</param>
        public static double EvaluatePosition(Board board)
        {
            double finalScore = 0;

            // =================== KARY ===================

            finalScore += EvaluateMaterial(board);
            finalScore += (-w_blockedBishops * CountBlockedBishops(board, Side.White));
            finalScore += (w_blockedBishops * CountBlockedBishops(board, Side.Black));
            finalScore += (-w_blockedRooks * CountBlockedRooks(board, Side.White));
            finalScore += (w_blockedRooks * CountBlockedRooks(board, Side.Black));
            finalScore += (-w_Is8thRankMatePossible * Convert.ToInt32(IsPossible8thRankMate(board, Side.White)));
            finalScore += (w_Is8thRankMatePossible * Convert.ToInt32(IsPossible8thRankMate(board, Side.Black)));

            int[] whietPawnDistribution = GetPawnDistribution(board, Side.White);
            int[] blackPawnDistribution = GetPawnDistribution(board, Side.Black);

            finalScore += (-w_doublePawns * CountDoubledPawns(whietPawnDistribution));
            finalScore += (w_doublePawns * CountDoubledPawns(blackPawnDistribution));
            finalScore += (-w_isolatedPawns * CountIsolatedPawns(whietPawnDistribution));
            finalScore += (w_isolatedPawns * CountIsolatedPawns(blackPawnDistribution));
            finalScore += (-w_minorPiecesAtEdges * CountKnightsAndBishopsAtEdges(board, Side.White));
            finalScore += (w_minorPiecesAtEdges * CountKnightsAndBishopsAtEdges(board, Side.Black));

            if (GetGameState(board) != GameStage.Enidng)
            {
                finalScore += (-w_directionsKingOpen * HowManyDirectionsKingIsOpen(board, Side.White));
                finalScore += (w_directionsKingOpen * HowManyDirectionsKingIsOpen(board, Side.Black));
                finalScore += (-w_IsKingInCenter * Convert.ToInt32(IsKingInCenter(board, Side.White)));
                finalScore += (w_IsKingInCenter * Convert.ToInt32(IsKingInCenter(board, Side.Black)));
            }
            else
            {
                finalScore += (-w_directionsKingOpenEnding * HowManyDirectionsKingIsOpen(board, Side.White));
                finalScore += (w_directionsKingOpenEnding * HowManyDirectionsKingIsOpen(board, Side.Black));
            }

            finalScore += (-w_KingSourrandingSquareAttack * CountKingSurroundingsSquareAttackers(board, Side.White));
            finalScore += (w_KingSourrandingSquareAttack * CountKingSurroundingsSquareAttackers(board, Side.Black));

            double currentValue;

            if (!board.IsKingSquareAttacked(Side.White))
            {
                foreach (Piece pinnedPiece in GetPinnedPieces(board, Side.White))
                {
                    _piecesValues.TryGetValue(pinnedPiece.Sign, out currentValue);
                    finalScore += currentValue * -w_PinnedPieces;
                }
            }
            else
                finalScore += -w_PinnedPiecesCheck;

            if (!board.IsKingSquareAttacked(Side.Black))
            {
                foreach (Piece pinnedPiece in GetPinnedPieces(board, Side.Black))
                {
                    _piecesValues.TryGetValue(pinnedPiece.Sign, out currentValue);
                    finalScore += currentValue * -w_PinnedPieces;
                }
            }
            else
                finalScore += w_PinnedPiecesCheck;

            finalScore += (-w_CastleBlocked * Convert.ToInt32(IsCastleBlocked(board, Side.White)));
            finalScore += (w_CastleBlocked * Convert.ToInt32(IsCastleBlocked(board, Side.Black)));

            // =================== NAGRODY ===================

            finalScore += (w_PawnChainLength * GetPawnChainLength(board, Side.White));
            finalScore += (-w_PawnChainLength * GetPawnChainLength(board, Side.Black));
            finalScore += (w_PawnInTheCenter * CountPawnInTheCenter(board, Side.White));
            finalScore += (-w_PawnInTheCenter * CountPawnInTheCenter(board, Side.Black));
            finalScore += (w_PawnAboutToPromote * CountPawnsAboutToPromotion(board, Side.White));
            finalScore += (-w_PawnAboutToPromote * CountPawnsAboutToPromotion(board, Side.Black));
            finalScore += (w_PassPawns * CountPassPawns(board, Side.White));
            finalScore += (-w_PassPawns * CountPassPawns(board, Side.Black));
            finalScore += (w_MinorPieceInTheCenter * CountKnightAndBishopsInTheCenter(board, Side.White));
            finalScore += (-w_MinorPieceInTheCenter * CountKnightAndBishopsInTheCenter(board, Side.Black));
            finalScore += (w_BishopPair * Convert.ToInt32(HasBishopsPair(board, Side.White)));
            finalScore += (-w_BishopPair * Convert.ToInt32(HasBishopsPair(board, Side.Black)));
            finalScore += (w_LongDiagonalBishop * CountLongDiagonalBishops(board, Side.White));
            finalScore += (-w_LongDiagonalBishop * CountLongDiagonalBishops(board, Side.Black));
            finalScore += (w_SupportedKnights * CountSupportedKnights(board, Side.White));
            finalScore += (-w_SupportedKnights * CountSupportedKnights(board, Side.Black));
            finalScore += (w_AreRooksConnected * Convert.ToInt32(AreRooksConnected(board, Side.White)));
            finalScore += (-w_AreRooksConnected * Convert.ToInt32(AreRooksConnected(board, Side.Black)));

            if (GetGameState(board) == GameStage.Opening)
            {
                finalScore += (-w_QueenInCenterOpening * Convert.ToInt32(IsQueenInTheCenter(board, Side.White)));
                finalScore += (w_QueenInCenterOpening * Convert.ToInt32(IsQueenInTheCenter(board, Side.Black)));
            }
            else
            {
                finalScore += (w_QueenInCenterMidEnd * Convert.ToInt32(IsQueenInTheCenter(board, Side.White)));
                finalScore += (-w_QueenInCenterMidEnd * Convert.ToInt32(IsQueenInTheCenter(board, Side.Black)));
            }

            finalScore += (w_KingProtectingMaterial * KingMaterialProtection(board, Side.White));
            finalScore += (w_KingProtectingMaterial * KingMaterialProtection(board, Side.Black));
            finalScore += (w_BishopBattery * Convert.ToInt32(HasBishopBattery(board, Side.White)));
            finalScore += (-w_BishopBattery * Convert.ToInt32(HasBishopBattery(board, Side.Black)));
            finalScore += (w_RookBattery * Convert.ToInt32(HasRookBattery(board, Side.White)));
            finalScore += (-w_RookBattery * Convert.ToInt32(HasRookBattery(board, Side.Black)));

            //===== KOŃCÓWKI =======

            finalScore += IsMate(board);
            IsDraw(board, ref finalScore);

            //===================

            return finalScore;
        }

        public static double EvaluateMaterial(Board board)
        {
            double wynik = 0;
            double currentValue = 0;

            foreach (Piece piece in board.WhitePiecesList)
            {
                _piecesValues.TryGetValue(piece.Sign, out currentValue);
                if (piece is Knight && IsClosedPosition(board))
                    currentValue += 0.2;
                wynik += currentValue;
            }

            foreach (Piece piece in board.BlackPiecesList)
            {
                _piecesValues.TryGetValue(piece.Sign, out currentValue);
                if (piece is Knight && IsClosedPosition(board))
                    currentValue -= 0.2;
                wynik += currentValue;
            }

            double MaterialAdvantageBonus = (2 * wynik) / (board.WhitePiecesList.Count + board.BlackPiecesList.Count);
            wynik += MaterialAdvantageBonus;

            return wynik;
        }

        public static bool IsClosedPosition(Board board)
        {
            int PawnClashCount = 0;

            for (int i = 43; i < 47; i++)
            {
                for (int j = i; j < 77; j += 10)
                {
                    if (board.PieceArrangement120[j] == 'p' && board.PieceArrangement120[j + 10] == 'P')
                    {
                        PawnClashCount++;
                        break;
                    }

                }
            }

            if (PawnClashCount >= 2)
                return true;
            else
                return false;
        }

        public static GameStage GetGameState(Board board)
        {
            int WhiteStartingPieces = 0;
            int BlackStartingPieces = 0;

            int WhitePiecesCount = 0;
            int BlackPiecesCount = 0;

            char[] FirstRank = { 'R', 'N', 'B', 'Q', 'K', 'B', 'N', 'R' };
            char[] EigthRank = { 'r', 'n', 'b', 'q', 'k', 'b', 'n', 'r' };

            for (int i = 21; i < 29; i++)
            {
                if (board.PieceArrangement120[i] == EigthRank[i - 21])
                    BlackStartingPieces++;
            }

            for (int i = 91; i < 99; i++)
            {
                if (board.PieceArrangement120[i] == FirstRank[i - 91])
                    WhiteStartingPieces++;
            }

            foreach (Piece piece in board.WhitePiecesList)
            {
                if (!(piece is Pawn))
                    WhitePiecesCount++;
            }

            foreach (Piece piece in board.BlackPiecesList)
            {
                if (!(piece is Pawn))
                    BlackPiecesCount++;
            }

            if ((WhiteStartingPieces > 3 && BlackStartingPieces > 3) && (WhitePiecesCount > 6 && BlackPiecesCount > 6))
                return GameStage.Opening;

            if (BlackPiecesCount < 4 || WhitePiecesCount < 4)
                return GameStage.Enidng;

            return GameStage.MidGame;
        }

        public static int CountBlockedBishops(Board board, Side side)
        {
            int[] diagonala1 = { -9, 9 };
            int[] diagonala2 = { -11, 11 };
            int HowManyBishopsAreBlocked = 0;
            List<Piece> collection;

            if (side == Side.White)
                collection = board.WhitePiecesList;
            else
                collection = board.BlackPiecesList;

            foreach (Piece piece in collection)
            {
                if (piece is Bishop)
                {
                    HowManyBishopsAreBlocked++;
                    int currentPosition = (int)piece.ExtandCoords;
                    int emptySquares = 0;

                    foreach (int liczba in diagonala1)
                    {
                        for (int i = currentPosition + liczba; i < 100; i += liczba)
                        {
                            if (board.PieceArrangement120[i] == '+')
                                break;
                            if (board.PieceArrangement120[i] == '0')
                                emptySquares++;
                            else break;
                        }
                    }

                    if (emptySquares > 3)
                    {
                        HowManyBishopsAreBlocked--;
                        continue;
                    }

                    emptySquares = 0;
                    foreach (int liczba in diagonala2)
                    {
                        for (int i = currentPosition + liczba; i < 100; i += liczba)
                        {
                            if (board.PieceArrangement120[i] == '+')
                                break;
                            if (board.PieceArrangement120[i] == '0')
                                emptySquares++;
                            else break;
                        }
                    }

                    if (emptySquares > 3)
                    {
                        HowManyBishopsAreBlocked--;
                        continue;
                    }
                }
            }

            return HowManyBishopsAreBlocked;
        }

        public static int CountBlockedRooks(Board board, Side side)
        {
            int[] liczby = { -10, 10 };
            int HowManyRooksAreBlocked = 0;
            List<Piece> collection;

            if (side == Side.White)
                collection = board.WhitePiecesList;
            else
                collection = board.BlackPiecesList;

            foreach (Piece piece in collection)
            {
                if (piece is Rook)
                {
                    HowManyRooksAreBlocked++;
                    int currentPosition = (int)piece.ExtandCoords;
                    int emptySquares = 0;
                    foreach (int liczba in liczby)
                    {
                        for (int i = currentPosition + liczba; i < 100; i += liczba)
                        {
                            if (board.PieceArrangement120[i] == '+')
                                break;
                            if (board.PieceArrangement120[i] == '0')
                                emptySquares++;
                            else break;
                        }
                    }

                    if (emptySquares > 2)
                    {
                        HowManyRooksAreBlocked--;
                        break;
                    }

                }
            }

            return HowManyRooksAreBlocked;
        }

        public static bool IsPossible8thRankMate(Board board, Side side)
        {
            if (side == Side.White)
            {
                if (board.PieceArrangement120[86] == 'P' &&
                    board.PieceArrangement120[87] == 'P' &&
                    board.PieceArrangement120[88] == 'P' &&
                    board.PieceArrangement120[97] == 'K') return true;

                if (board.PieceArrangement120[87] == 'P' &&
                    board.PieceArrangement120[88] == 'P' &&
                    board.PieceArrangement120[98] == 'K') return true;

                if (board.PieceArrangement120[81] == 'P' &&
                    board.PieceArrangement120[83] == 'P' &&
                    board.PieceArrangement120[84] == 'P' &&
                    board.PieceArrangement120[92] == 'K') return true;

                if (board.PieceArrangement120[81] == 'P' &&
                    board.PieceArrangement120[82] == 'P' &&
                    board.PieceArrangement120[91] == 'K') return true;

            }
            else
            {
                if (board.PieceArrangement120[36] == 'p' &&
                    board.PieceArrangement120[37] == 'p' &&
                    board.PieceArrangement120[38] == 'p' &&
                    board.PieceArrangement120[27] == 'k') return true;

                if (board.PieceArrangement120[37] == 'p' &&
                    board.PieceArrangement120[38] == 'p' &&
                    board.PieceArrangement120[28] == 'k') return true;

                if (board.PieceArrangement120[31] == 'p' &&
                    board.PieceArrangement120[33] == 'p' &&
                    board.PieceArrangement120[34] == 'p' &&
                    board.PieceArrangement120[22] == 'k') return true;

                if (board.PieceArrangement120[31] == 'p' &&
                    board.PieceArrangement120[32] == 'p' &&
                    board.PieceArrangement120[21] == 'k') return true;
            }

            return false;
        }

        public static int[] GetPawnDistribution(Board board, Side side)
        {
            int[] distribution = new int[8];
            int columnPawns = 0;
            char znakPionka;

            if (side == Side.White)
                znakPionka = 'P';
            else
                znakPionka = 'p';

            for (int i = 31; i < 39; i++)
            {
                columnPawns = 0;
                for (int j = i; j < 90; j += 10)
                {
                    if (board.PieceArrangement120[j] == znakPionka)
                        columnPawns++;
                }

                distribution[i - 31] = columnPawns;
            }
            return distribution;
        }

        public static int CountDoubledPawns(int[] pawnDistribution)
        {
            int doubledPawnCount = 0;

            for (int i = 0; i < 8; i++)
            {
                if (pawnDistribution[i] > 1)
                {
                    doubledPawnCount += (pawnDistribution[i] - 1);
                }

            }
            return doubledPawnCount;
        }

        public static int CountIsolatedPawns(int[] pawnDistribution)
        {
            int isolatedPawns = 0;

            if (pawnDistribution[0] > 0)
            {
                if (pawnDistribution[1] == 0)
                    isolatedPawns += pawnDistribution[0];
            }

            if (pawnDistribution[7] > 0)
            {
                if (pawnDistribution[6] == 0)
                    isolatedPawns += pawnDistribution[7];
            }

            for (int i = 1; i < 7; i++)
            {
                if (pawnDistribution[i] > 0)
                {
                    if (pawnDistribution[i - 1] == 0 && pawnDistribution[i + 1] == 0)
                        isolatedPawns += pawnDistribution[i];
                }
            }

            return isolatedPawns;
        }

        public static int CountKnightsAndBishopsAtEdges(Board board, Side side)
        {
            int NumbersOfPiecesAtEdge = 0;
            int[] edgeCoords = { 21, 22, 23, 24, 24, 25, 26, 27, 28, 31, 41, 51, 61, 71, 81, 91, 38, 48, 58, 68, 78, 88, 98, 92, 93, 94, 95, 96, 97 };
            List<Piece> collection;

            if (side == Side.White)
                collection = board.WhitePiecesList;
            else
                collection = board.BlackPiecesList;


            foreach (Piece piece in collection)
            {
                if (piece is Knight || piece is Bishop)
                {
                    if (edgeCoords.Contains((int)piece.ExtandCoords))
                        NumbersOfPiecesAtEdge++;
                }
            }

            return NumbersOfPiecesAtEdge;
        }

        public static int HowManyDirectionsKingIsOpen(Board board, Side side)
        {
            int numberOfDirections = 0;
            char[] enemyList;
            int kingPosition;

            if (side == Side.White)
            {
                kingPosition = (int)board.WhitePiecesList.Find(x => x is King).ExtandCoords;
                enemyList = new char[] { 'p', 'n', 'b', 'r', 'q', 'k' };
            }
            else
            {
                kingPosition = (int)board.BlackPiecesList.Find(x => x is King).ExtandCoords;
                enemyList = new char[] { 'P', 'N', 'B', 'R', 'Q', 'K' };
            }

            int[] liczby = { -11, -10, -9, -1, 1, 9, 10, 11 };

            foreach (int liczba in liczby)
            {
                if (board.PieceArrangement120[kingPosition + liczba] == '0' ||
                    enemyList.Contains(board.PieceArrangement120[kingPosition + liczba]))
                {
                    if (board.PieceArrangement120[kingPosition + (liczba * 2)] == '0' ||
                   enemyList.Contains(board.PieceArrangement120[kingPosition + (liczba * 2)]))
                    {
                        //     if (board.PieceArrangement120[kingPosition + (liczba * 3)] == '0' ||
                        //enemyList.Contains(board.PieceArrangement120[kingPosition + (liczba * 3)]))
                        //     {
                        numberOfDirections++;
                        //}

                    }
                }


            }

            return numberOfDirections;
        }

        public static bool IsKingInCenter(Board board, Side side)
        {
            int[] polaCentralne = { 41, 42, 43, 44, 45, 46, 47, 48,
                                    51, 52, 53, 54, 55, 56, 57, 58,
                                    61, 62, 63, 64, 65, 66, 67, 68,
                                    71, 72, 73, 74, 75, 76, 77, 78};
            int kingPosition;

            if (side == Side.White)
                kingPosition = (int)board.WhitePiecesList.Find(x => x is King).ExtandCoords;
            else
                kingPosition = (int)board.BlackPiecesList.Find(x => x is King).ExtandCoords;

            if (polaCentralne.Contains(kingPosition))
                return true;

            return false;
        }

        public static int CountKingSurroundingsSquareAttackers(Board board, Side side)
        {
            int attackers = 0;
            ExtandCoords kingSquare;
            int[] liczby = { -11, -10, -9, -1, 1, 9, 10, 11 };

            if (side == Side.White)
                kingSquare = board.WhitePiecesList.Find(x => x is King).ExtandCoords;
            else
                kingSquare = board.BlackPiecesList.Find(x => x is King).ExtandCoords;

            foreach (int liczba in liczby)
            {
                if (board.PieceArrangement120[(int)kingSquare + liczba] != '+')
                    attackers += board.CountSquareAttackers(kingSquare + liczba, side);
            }

            return attackers;
        }

        public static List<Piece> GetPinnedPieces(Board board, Side side)
        {
            List<Piece> lista = new List<Piece>();
            List<Piece> collection;

            if (side == Side.White)
                collection = board.WhitePiecesList;
            else
                collection = board.BlackPiecesList;

            Board workBoard = new Board();
            foreach (Piece piece in collection)
            {
                if (!(piece is King))
                {
                    workBoard.GetValuesFromBoard(board);
                    workBoard.PieceArrangement120[(int)piece.ExtandCoords] = '0';

                    if (workBoard.IsKingSquareAttacked(side))
                        lista.Add(piece);
                }

            }

            return lista;
        }

        public static bool IsCastleBlocked(Board board, Side side)
        {
            if (side == Side.White)
            {
                if (board.PieceArrangement120[96] == '0' && board.PieceArrangement120[97] == '0')
                {
                    if (board.CanWhiteCastleKingSide)
                    {
                        if (board.IsSquareAttack(ExtandCoords.F1, Side.White) || board.IsSquareAttack(ExtandCoords.G1, Side.White) || board.IsKingSquareAttacked(Side.White))
                            return true;
                    }
                }
                else if (board.PieceArrangement120[94] == '0' && board.PieceArrangement120[93] == '0')
                {
                    if (board.CanWhiteCastleQueenSide)
                    {
                        if (board.IsSquareAttack(ExtandCoords.D1, Side.White) || board.IsSquareAttack(ExtandCoords.C1, Side.White) || board.IsKingSquareAttacked(Side.White))
                            return true;
                    }
                }
            }
            else
            {
                if (board.PieceArrangement120[26] == '0' && board.PieceArrangement120[27] == '0')
                {
                    if (board.CanBlackCastleKingSide)
                    {
                        if (board.IsSquareAttack(ExtandCoords.F8, Side.Black) || board.IsSquareAttack(ExtandCoords.G8, Side.Black) || board.IsKingSquareAttacked(Side.Black))
                            return true;
                    }
                }
                else if (board.PieceArrangement120[24] == '0' && board.PieceArrangement120[23] == '0')
                {
                    if (board.CanBlackCastleQueenSide)
                    {
                        if (board.IsSquareAttack(ExtandCoords.D8, Side.Black) || board.IsSquareAttack(ExtandCoords.C8, Side.Black) || board.IsKingSquareAttacked(Side.Black))
                            return true;
                    }
                }
            }
            return false;
        }

        //====================================================================================================

        public static int GetPawnChainLength(Board board, Side side)
        {
            char pawnSign;
            List<Piece> collection;
            int maxPawnChainLength = 0;
            int[] liczby;

            if (side == Side.White)
            {
                pawnSign = 'P';
                collection = board.WhitePiecesList;
                liczby = new int[] { 9, 11 };
            }
            else
            {
                pawnSign = 'p';
                collection = board.BlackPiecesList;
                liczby = new int[] { -9, -11 };
            }

            foreach (Piece pawn in collection)
            {
                if (pawn is Pawn)
                {
                    foreach (int liczba in liczby)
                    {
                        if (board.PieceArrangement120[(int)pawn.ExtandCoords + liczba] == pawnSign)
                        {
                            maxPawnChainLength++;
                            break;
                        }

                    }
                }
            }

            return maxPawnChainLength;
        }

        public static int CountPawnInTheCenter(Board board, Side side)
        {
            int counter = 0;
            int[] liczby;
            List<Piece> collection;

            if (side == Side.White)
            {
                liczby = new int[] { 43, 44, 45, 46, 53, 54, 55, 56, 64, 65 };
                collection = board.WhitePiecesList;
            }
            else
            {
                liczby = new int[] { 73, 74, 75, 76, 63, 54, 55, 66, 64, 65 };
                collection = board.BlackPiecesList;
            }

            foreach (Piece piece in collection)
            {
                if (piece is Pawn)
                {
                    if (liczby.Contains((int)piece.ExtandCoords))
                        counter++;
                }
            }
            return counter;
        }

        public static int CountPawnsAboutToPromotion(Board board, Side side)
        {
            int counter = 0;
            int[] liczby;
            List<Piece> collection;

            if (side == Side.White)
            {
                liczby = new int[] { 31, 32, 33, 34, 35, 36, 37, 38, 41, 42, 43, 44, 45, 46, 47, 48 };
                collection = board.WhitePiecesList;
            }
            else
            {
                liczby = new int[] { 71, 72, 73, 74, 75, 76, 77, 78, 81, 82, 83, 84, 85, 86, 87, 88 };
                collection = board.BlackPiecesList;
            }

            foreach (Piece piece in collection)
            {
                if (piece is Pawn)
                {
                    if (liczby.Contains((int)piece.ExtandCoords))
                        counter++;
                }
            }
            return counter;
        }

        public static int CountPassPawns(Board board, Side side)
        {
            int counter = 0;
            int[] liczby;
            List<Piece> collection;
            char enemyPawn;
            int emptyLines = 0;

            if (side == Side.White)
            {
                liczby = new int[] { -11, -10, -9 };
                collection = board.WhitePiecesList;
                enemyPawn = 'p';
            }
            else
            {
                liczby = new int[] { 11, 10, 9 };
                collection = board.BlackPiecesList;
                enemyPawn = 'P';
            }

            foreach (Piece piece in collection)
            {
                emptyLines = 0;
                if (piece is Pawn)
                {

                    foreach (int liczba in liczby)
                    {
                        for (int i = (int)piece.ExtandCoords + liczba; i < 100; i += liczby[1])
                        {
                            if (board.PieceArrangement120[i] == enemyPawn)
                                goto exit;
                            else if (board.PieceArrangement120[i] == '+')
                                break;
                        }
                        emptyLines++;
                    }
                    exit:;
                }
                if (emptyLines == 3)
                    counter++;

            }
            return counter;
        }

        public static int CountKnightAndBishopsInTheCenter(Board board, Side side)
        {
            int NumbersOfPiecesInTheCenter = 0;
            int[] centerCoords = {43,44,45,46,
                                  53,54,55,56,
                                  63,64,65,66,
                                  73,74,75,76 };
            List<Piece> collection;

            if (side == Side.White)
                collection = board.WhitePiecesList;
            else
                collection = board.BlackPiecesList;


            foreach (Piece piece in collection)
            {
                if (piece is Knight || piece is Bishop)
                {
                    if (centerCoords.Contains((int)piece.ExtandCoords))
                        NumbersOfPiecesInTheCenter++;
                }
            }

            return NumbersOfPiecesInTheCenter;
        }

        public static bool HasBishopsPair(Board board, Side side)
        {
            List<Piece> collection;
            int bishopCounter = 0;

            if (side == Side.White)
            {
                collection = board.WhitePiecesList;
            }
            else
            {
                collection = board.BlackPiecesList;
            }

            foreach (Piece piece in collection)
            {
                if (piece is Bishop)
                {
                    bishopCounter++;
                    if (bishopCounter >= 2)
                        return true;
                }
            }
            return false;
        }

        public static int CountLongDiagonalBishops(Board board, Side side)
        {
            int bishopCounter = 0;
            int[] longDiagonals = { 21, 32, 43, 54, 65, 76, 87, 98, 28, 37, 46, 55, 64, 73, 82, 91 };
            List<Piece> collection;

            if (side == Side.White)
                collection = board.WhitePiecesList;
            else
                collection = board.BlackPiecesList;


            foreach (Piece piece in collection)
            {
                if (piece is Bishop)
                {
                    if (longDiagonals.Contains((int)piece.ExtandCoords))
                        bishopCounter++;
                }
            }

            return bishopCounter;
        }

        public static int CountSupportedKnights(Board board, Side side)
        {
            char pawnSign;
            List<Piece> collection;
            int supportedKnights = 0;
            int[] liczby;

            if (side == Side.White)
            {
                pawnSign = 'P';
                collection = board.WhitePiecesList;
                liczby = new int[] { 9, 11 };
            }
            else
            {
                pawnSign = 'p';
                collection = board.BlackPiecesList;
                liczby = new int[] { -9, -11 };
            }

            foreach (Piece knight in collection)
            {
                if (knight is Knight)
                {
                    foreach (int liczba in liczby)
                    {
                        if (board.PieceArrangement120[(int)knight.ExtandCoords + liczba] == pawnSign)
                        {
                            supportedKnights++;
                            break;
                        }

                    }
                }
            }

            return supportedKnights;
        }

        public static bool AreRooksConnected(Board board, Side side)
        {
            List<Piece> collection;
            char rookSign;
            int[] liczby = { -1, -10, 1, 10 };

            if (side == Side.White)
            {
                collection = board.WhitePiecesList;
                rookSign = 'R';
            }
            else
            {
                collection = board.BlackPiecesList;
                rookSign = 'r';
            }

            Piece rook = collection.Find(x => x is Rook);
            if (rook == null)
                return false;

            foreach (int liczba in liczby)
            {
                for (int i = (int)rook.ExtandCoords + liczba; i < 100; i += liczba)
                {
                    if (board.PieceArrangement120[i] == '+')
                        break;
                    else if (board.PieceArrangement120[i] == '0')
                        continue;
                    else if (board.PieceArrangement120[i] == rookSign)
                        return true;
                    else
                        break;
                }
            }
            return false;
        }

        public static bool IsQueenInTheCenter(Board board, Side side)
        {
            int[] centerCoords = {43,44,45,46,
                                  53,54,55,56,
                                  63,64,65,66,
                                  73,74,75,76 };
            Piece queen;

            if (side == Side.White)
                queen = board.WhitePiecesList.Find(x => x is Queen);
            else
                queen = board.BlackPiecesList.Find(x => x is Queen);

            if (queen == null)
                return false;

            if (centerCoords.Contains((int)queen.ExtandCoords))
                return true;

            return false;
        }

        public static double KingMaterialProtection(Board board, Side side)
        {
            double materialProtection = 0;
            int[] liczby = { -11, -10, -9, -1, 1, 9, 10, 11 };
            Piece king;
            double currentValue;
            char currentSign;

            if (side == Side.White)
                king = board.WhitePiecesList.Find(x => x is King);
            else
                king = board.BlackPiecesList.Find(x => x is King);

            foreach (int liczba in liczby)
            {
                currentSign = board.PieceArrangement120[(int)king.ExtandCoords + liczba];
                if (_piecesValues.ContainsKey(currentSign))
                {
                    _piecesValues.TryGetValue(currentSign, out currentValue);
                    materialProtection += currentValue;
                }

            }

            return materialProtection;
        }

        public static bool HasBishopBattery(Board board, Side side)
        {
            List<Piece> collection;
            char bishopSign;
            int[] liczby = { -11, -9, 9, 11 };

            if (side == Side.White)
            {
                collection = board.WhitePiecesList;
                bishopSign = 'B';
            }
            else
            {
                collection = board.BlackPiecesList;
                bishopSign = 'b';
            }

            Piece queen = collection.Find(x => x is Queen);
            if (queen == null)
                return false;

            foreach (int liczba in liczby)
            {
                for (int i = (int)queen.ExtandCoords + liczba; i < 100; i += liczba)
                {
                    if (board.PieceArrangement120[i] == '+')
                        break;
                    else if (board.PieceArrangement120[i] == '0')
                        continue;
                    else if (board.PieceArrangement120[i] == bishopSign)
                        return true;
                    else
                        break;
                }
            }
            return false;
        }

        public static bool HasRookBattery(Board board, Side side)
        {
            List<Piece> collection;
            char rookSign;
            int[] liczby = { -10, -1, 1, 10 };

            if (side == Side.White)
            {
                collection = board.WhitePiecesList;
                rookSign = 'R';
            }
            else
            {
                collection = board.BlackPiecesList;
                rookSign = 'r';
            }

            Piece queen = collection.Find(x => x is Queen);
            if (queen == null)
                return false;

            foreach (int liczba in liczby)
            {
                for (int i = (int)queen.ExtandCoords + liczba; i < 100; i += liczba)
                {
                    if (board.PieceArrangement120[i] == '+')
                        break;
                    else if (board.PieceArrangement120[i] == '0')
                        continue;
                    else if (board.PieceArrangement120[i] == rookSign)
                        return true;
                    else
                        break;
                }
            }
            return false;
        }

        //======================================

        static double IsMate(Board board)
        {
            if (board._possibleBoardsList.Count == 0 && board.IsKingInCheck)
            {
                if (board.SideToMove == Side.White)
                    return -999;
                else
                    return 999;
            }
            return 0;
        }

        static void IsDraw(Board board, ref double finalScore)
        {
            if (board._possibleBoardsList.Count == 0 && board.IsKingInCheck == false)
            {
                finalScore = 0;
            }
            else if (board.WhitePiecesList.Count == 1 && board.BlackPiecesList.Count == 1)
            {
                finalScore = 0;
            }
            else if (board.NoCaptureOrPawnMove >= 50)
            {
                finalScore = 0;
            }
            else
            {
                int repetitions = 0;

                foreach (Board boardPrev in board.PreviousBoardList)
                {
                    if (Enumerable.SequenceEqual(boardPrev.PieceArrangement, board.PieceArrangement))
                    {
                        repetitions++;
                    }
                }
                if (repetitions >= 2)
                    finalScore = 0;
            }

        }

        //====================================
    }
}
