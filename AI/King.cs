using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cheops.AI
{
    class King : Piece
    {
        public static ulong[] WhiteHashValues = new ulong[64];
        public static ulong[] BlackHashValues = new ulong[64];

        List<Board> _boardList = new List<Board>();

        int[] liczby = { -11, -10, -9, -1, 1, 9, 10, 11 };

        public King(ExtandCoords extandCoords, Side side) : base(extandCoords, side)
        {
            if (Side == Side.White)
                Sign = 'K';
            else
                Sign = 'k';
        }

        /// <summary>
        /// Returns a list of boards arisen as a result of this piece possible moves.
        /// </summary>
        /// <param name="board">Board on which this piece is at.</param>
        public override List<Board> GenerateBoards(Board board)
        {
            _boardList.Clear();

            int biezacePole = (int)ExtandCoords;

            foreach (int liczba in liczby)
            {
                int poleDocelowe = biezacePole + liczba;
                char znakDocelowy = board.PieceArrangement120[poleDocelowe];

                if (znakDocelowy == '+') //OFF BOARD
                    continue;

                if (znakDocelowy == '0') // PUSTE POLE
                {
                    Board boardToAdd = HandleEmptySquareMove(board, biezacePole, poleDocelowe);
                    if (boardToAdd != null) 
                    {
                        RemoveCastleRights(boardToAdd, Side);
                        _boardList.Add(boardToAdd);
                    }
                }   
                else // CAPTURE
                {
                    Board boardToAdd = HandleCaptureMove(board, biezacePole, poleDocelowe);
                    if (boardToAdd != null)
                    {
                        RemoveCastleRights(boardToAdd, Side);
                        _boardList.Add(boardToAdd);
                    }
                }   
            }

            //ROSZADY
            if (Side == Side.White)
            {
                PerformWhiteKingSideCastle(board);
                PerformWhiteQueenSideCastle(board);
            }
            else
            {
                PerformBlackKingSideCastle(board);
                PerformBlackQueenSideCastle(board);

            }
            return _boardList;
        }

        void RemoveCastleRights(Board board, Side side)
        {
            if(side == Side.White)
            {
                if(board.CanWhiteCastleKingSide)
                {
                    board.CanWhiteCastleKingSide = false;
                    board.HashValue ^= Board.WhiteKingCastleHashValue;
                }

                if (board.CanWhiteCastleQueenSide)
                {
                    board.CanWhiteCastleQueenSide = false;
                    board.HashValue ^= Board.WhiteQueenCastleHashValue;
                } 
            }
            else
            {
                if (board.CanBlackCastleKingSide)
                {
                    board.CanBlackCastleKingSide = false;
                    board.HashValue ^= Board.BlackKingCastleHashValue;
                }

                if (board.CanBlackCastleQueenSide)
                {
                    board.CanBlackCastleQueenSide = false;
                    board.HashValue ^= Board.BlackQueenCastleHashValue;
                }
            }
        }
        
        void PerformWhiteKingSideCastle(Board board)
        {
            if (board.PieceArrangement120[96] == '0' && board.PieceArrangement120[97] == '0')
            {
                if (ExtandCoords == ExtandCoords.E1)
                {
                    if (board.CanWhiteCastleKingSide)
                    {
                        if (!board.IsSquareAttack(ExtandCoords.F1, Side) && !board.IsKingInCheck)
                        {
                            Board retBoard = new Board(board);

                            retBoard.StartSquare = ExtandCoords;
                            retBoard.FinishSquare = ExtandCoords.G1;

                            //*****************************************
                            HandleTTCastle(Side.White, CastleType.KingSide, retBoard);
                            //*****************************************

                            retBoard.PieceArrangement120[95] = '0';
                            retBoard.PieceArrangement120[97] = 'K';
                            retBoard.PieceArrangement120[98] = '0';
                            retBoard.PieceArrangement120[96] = 'R';

                            Piece movingPiece = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                            movingPiece.ExtandCoords = ExtandCoords.G1;

                            Piece movingRook = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == ExtandCoords.H1);
                            movingRook.ExtandCoords = ExtandCoords.F1;

                            if (retBoard.IsKingSquareAttacked(Helper.SwitchSide(Side)))
                                retBoard.IsKingInCheck = true;

                            RemoveCastleRights(retBoard, Side);

                            if (!retBoard.IsKingSquareAttacked(Side))
                                _boardList.Add(retBoard);
                        }
                    }
                }

            }
        }

        void PerformWhiteQueenSideCastle(Board board)
        {
            if (board.PieceArrangement120[92] == '0' &&
                board.PieceArrangement120[93] == '0' && 
                board.PieceArrangement120[94] == '0')
            {
                if (ExtandCoords == ExtandCoords.E1)
                {
                    if (board.CanWhiteCastleQueenSide)
                    {
                        if (!board.IsSquareAttack(ExtandCoords.D1, Side) && !board.IsKingInCheck)
                        {
                            Board retBoard = new Board(board);

                            retBoard.StartSquare = ExtandCoords;
                            retBoard.FinishSquare = ExtandCoords.C1;

                            //*****************************************
                            HandleTTCastle(Side.White, CastleType.QueenSide, retBoard);
                            //*****************************************

                            retBoard.PieceArrangement120[91] = '0';
                            retBoard.PieceArrangement120[92] = '0';
                            retBoard.PieceArrangement120[93] = 'K';
                            retBoard.PieceArrangement120[94] = 'R';
                            retBoard.PieceArrangement120[95] = '0';

                            Piece movingPiece = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                            movingPiece.ExtandCoords = ExtandCoords.C1;

                            Piece movingRook = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == ExtandCoords.A1);
                            movingRook.ExtandCoords = ExtandCoords.D1;

                            if (retBoard.IsKingSquareAttacked(Helper.SwitchSide(Side)))
                                retBoard.IsKingInCheck = true;

                            RemoveCastleRights(retBoard, Side);

                            if (!retBoard.IsKingSquareAttacked(Side))
                                _boardList.Add(retBoard);
                        }
                    }
                }

            }
        }

        void PerformBlackKingSideCastle(Board board)
        {
            if (board.PieceArrangement120[26] == '0' && board.PieceArrangement120[27] == '0')
            {
                if (ExtandCoords == ExtandCoords.E8)
                {
                    if (board.CanBlackCastleKingSide)
                    {
                        if (!board.IsSquareAttack(ExtandCoords.F8, Side) && !board.IsKingInCheck)
                        {
                            Board retBoard = new Board(board);

                            retBoard.StartSquare = ExtandCoords;
                            retBoard.FinishSquare = ExtandCoords.G8;

                            //*****************************************
                            HandleTTCastle(Side.Black, CastleType.KingSide, retBoard);
                            //*****************************************

                            retBoard.PieceArrangement120[25] = '0';
                            retBoard.PieceArrangement120[26] = 'r';
                            retBoard.PieceArrangement120[27] = 'k';
                            retBoard.PieceArrangement120[28] = '0';

                            Piece movingPiece = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                            movingPiece.ExtandCoords = ExtandCoords.G8;

                            Piece movingRook = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == ExtandCoords.H8);
                            movingRook.ExtandCoords = ExtandCoords.F8;

                            if (retBoard.IsKingSquareAttacked(Helper.SwitchSide(Side)))
                                retBoard.IsKingInCheck = true;

                            RemoveCastleRights(retBoard, Side);

                            if (!retBoard.IsKingSquareAttacked(Side))
                                _boardList.Add(retBoard);
                        }
                    }
                }

            }
        }

        void PerformBlackQueenSideCastle(Board board)
        {
            if (board.PieceArrangement120[22] == '0' &&
                 board.PieceArrangement120[23] == '0' &&
                 board.PieceArrangement120[24] == '0')
            {
                if (ExtandCoords == ExtandCoords.E8)
                {
                    if (board.CanBlackCastleQueenSide)
                    {
                        if (!board.IsSquareAttack(ExtandCoords.D8, Side) && !board.IsKingInCheck)
                        {
                            Board retBoard = new Board(board);

                            retBoard.StartSquare = ExtandCoords;
                            retBoard.FinishSquare = ExtandCoords.C8;

                            //*****************************************
                            HandleTTCastle(Side.Black, CastleType.QueenSide, retBoard);
                            //*****************************************

                            retBoard.PieceArrangement120[21] = '0';
                            retBoard.PieceArrangement120[22] = '0';
                            retBoard.PieceArrangement120[23] = 'k';
                            retBoard.PieceArrangement120[24] = 'r';
                            retBoard.PieceArrangement120[25] = '0';

                            Piece movingPiece = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                            movingPiece.ExtandCoords = ExtandCoords.C8;

                            Piece movingRook = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == ExtandCoords.A8);
                            movingRook.ExtandCoords = ExtandCoords.D8;

                            if (retBoard.IsKingSquareAttacked(Helper.SwitchSide(Side)))
                                retBoard.IsKingInCheck = true;

                            RemoveCastleRights(retBoard, Side);

                            if (!retBoard.IsKingSquareAttacked(Side))
                                _boardList.Add(retBoard);
                        }
                    }
                }

            }
        }

    }
}
