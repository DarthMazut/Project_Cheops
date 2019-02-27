using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.AI
{
    class Pawn : Piece
    {
        public static ulong[] WhiteHashValues = new ulong[64];
        public static ulong[] BlackHashValues = new ulong[64];

        List<Board> _boardList = new List<Board>();

        int[] liczbyAttack;
        int[] liczbySprint;
        int krok;
        char[] promSigns;

        public Pawn(ExtandCoords extandCoords, Side side) : base(extandCoords, side)
        {
            if (Side == Side.White)
            {
                Sign = 'P';
                liczbyAttack = new int[] { -11, -9};
                liczbySprint = new int[] { -10, -20};
                krok = -10;
                promSigns = new char[] {'N','B','R','Q'};
                
            }  
            else
            {
                Sign = 'p';
                liczbyAttack = new int[] { 11, 9 };
                liczbySprint = new int[] { 10, 20 };
                krok = 10;
                promSigns = new char[] { 'n', 'b', 'r', 'q' };
            }
                
        }

        /// <summary>
        /// Returns a list of boards arisen as a result of this piece possible moves.
        /// </summary>
        /// <param name="board">Board on which this piece is at.</param>
        public override List<Board> GenerateBoards(Board board)
        {
            _boardList.Clear();

            HandlePawnStartPos(board);
            HandlePawnNormalPos(board);
            HandlePawnPromotion(board);

            return _boardList;
        }   


        void HandlePawnStartPos(Board board)
        {
            int max;
            int min;
            int maxEnPas;
            int minEnPas;

            if(Side == Side.White)
            {
                max = 88;
                min = 81;
                maxEnPas = 68;
                minEnPas = 61;
            }
            else
            {
                max = 38;
                min = 31;
                maxEnPas = 58;
                minEnPas = 51;
            }

            int biezacePole = (int)ExtandCoords;
            int poleDocelowe;
            char znakDocelowy;

            if (biezacePole >= min && biezacePole <= max)
            {
                foreach (int liczba in liczbySprint)
                {
                    poleDocelowe = biezacePole + liczba;
                    znakDocelowy = board.PieceArrangement120[poleDocelowe];

                    if (znakDocelowy == '0')
                    {
                        Board boardToAdd = HandleEmptySquareMove(board, biezacePole, poleDocelowe);
                        if (boardToAdd != null)
                        {
                            boardToAdd.NoCaptureOrPawnMove = 0;
                            if (poleDocelowe >= minEnPas && poleDocelowe <= maxEnPas) // EnPassant
                            {
                                if (Side == Side.White)
                                {
                                    if (boardToAdd.PieceArrangement120[poleDocelowe + 10] != '0')
                                        break;

                                    boardToAdd.EnPassantSquare = (ExtandCoords)poleDocelowe + 10;
                                    boardToAdd.EnPassantPawn = (ExtandCoords)poleDocelowe;
                                }
                                else
                                {
                                    if (boardToAdd.PieceArrangement120[poleDocelowe - 10] != '0')
                                        break;

                                    boardToAdd.EnPassantSquare = (ExtandCoords)poleDocelowe - 10;
                                    boardToAdd.EnPassantPawn = (ExtandCoords)poleDocelowe;
                                }
                                //************** TT **************
                                  HandleTTSetEnPassantSquare((int)boardToAdd.EnPassantSquare, boardToAdd);
                                //************** TT **************
                            }
                            _boardList.Add(boardToAdd);
                        }
                    }
                }

                foreach (int liczba in liczbyAttack)
                {
                    poleDocelowe = biezacePole + liczba;
                    znakDocelowy = board.PieceArrangement120[poleDocelowe];

                    if (znakDocelowy != '0' && znakDocelowy != '+')
                    {
                        Board boardToAdd = HandleCaptureMove(board, biezacePole, poleDocelowe);
                        if (boardToAdd != null) _boardList.Add(boardToAdd);
                    }
                }
            }

        }

        void HandlePawnNormalPos(Board board)
        {
            int max = 78;
            int min = 41;

            int biezacePole = (int)ExtandCoords;
            int poleDocelowe;
            char znakDocelowy;

            if (biezacePole >= min && biezacePole <= max)
            {
                poleDocelowe = biezacePole + krok;
                znakDocelowy = board.PieceArrangement120[poleDocelowe];

                if (znakDocelowy == '0') //EMPTY SQUARE
                {
                    Board boardToAdd = HandleEmptySquareMove(board, biezacePole, poleDocelowe);
                    if(boardToAdd != null)
                    {
                        boardToAdd.NoCaptureOrPawnMove = 0;
                        _boardList.Add(boardToAdd);
                    }
                    
                }

                foreach (int liczba in liczbyAttack)
                {
                    poleDocelowe = biezacePole + liczba;
                    znakDocelowy = board.PieceArrangement120[poleDocelowe];

                    if (znakDocelowy != '0' && znakDocelowy != '+')
                    {
                        Board boardToAdd = HandleCaptureMove(board, biezacePole, poleDocelowe);
                        if (boardToAdd != null) _boardList.Add(boardToAdd);
                    }

                    if (board.EnPassantSquare != null)
                    {
                        if (poleDocelowe == (int)board.EnPassantSquare) //EnPassantCapture
                        {
                            Board retBoard = new Board(board);

                            retBoard.StartSquare = ExtandCoords;
                            retBoard.FinishSquare = (ExtandCoords)poleDocelowe;

                            //************** TT **************
                            HandleTTEnPassantCapture(biezacePole, poleDocelowe, (int)board.EnPassantPawn, retBoard);
                            //************** TT **************

                            retBoard.PieceArrangement120[biezacePole] = '0';
                            retBoard.PieceArrangement120[poleDocelowe] = Sign;
                            retBoard.PieceArrangement120[(int)board.EnPassantPawn] = '0';

                            retBoard.NoCaptureOrPawnMove = 0;
                            retBoard.IsCapturePosition = true;

                            if (Side == Side.White)
                            {
                                Piece movingPiece = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                                movingPiece.ExtandCoords = (ExtandCoords)poleDocelowe;

                                Piece targetPiece = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == board.EnPassantPawn);
                                retBoard.BlackPiecesList.Remove(targetPiece);
                            }
                            else
                            {
                                Piece movingPiece = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                                movingPiece.ExtandCoords = (ExtandCoords)poleDocelowe;

                                Piece targetPiece = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == board.EnPassantPawn);
                                retBoard.WhitePiecesList.Remove(targetPiece);
                            }


                            if (retBoard.IsKingSquareAttacked(Helper.SwitchSide(Side)))
                                retBoard.IsKingInCheck = true;

                            if (!retBoard.IsKingSquareAttacked(Side.White))
                                _boardList.Add(retBoard);
                        } 
                    }
                }


            }
        }

        void HandlePawnPromotion(Board board)
        {
            int max;
            int min;

            if (Side == Side.White)
            {
                max = 38;
                min = 31;
            }
            else
            {
                max = 88;
                min = 81;
            }

            int biezacePole = (int)ExtandCoords;
            int poleDocelowe;
            char znakDocelowy;

            if (biezacePole >= min && biezacePole <= max)
            {

                poleDocelowe = biezacePole + krok;
                znakDocelowy = board.PieceArrangement120[poleDocelowe];

                if (znakDocelowy == '0') //EMPTY SQUARE PROMOTION
                {

                    foreach (char promSign in promSigns)
                    {
                        Board retBoard = new Board(board);

                        retBoard.StartSquare = ExtandCoords;
                        retBoard.FinishSquare = (ExtandCoords)poleDocelowe;

                        //******************************************
                        HandleTTPromotionEmptySquare(biezacePole, poleDocelowe, retBoard, promSign);
                        //******************************************

                        retBoard.PieceArrangement120[biezacePole] = '0';
                        retBoard.PieceArrangement120[poleDocelowe] = promSign;

                        retBoard.NoCaptureOrPawnMove = 0;

                        if (Side == Side.White)
                        {
                            Piece movingPiece = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                            retBoard.WhitePiecesList.Remove(movingPiece);
                            AddPieceToList(retBoard, promSign, (ExtandCoords)poleDocelowe);
                        }
                        else
                        {
                            Piece movingPiece = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                            retBoard.BlackPiecesList.Remove(movingPiece);
                            AddPieceToList(retBoard, promSign, (ExtandCoords)poleDocelowe);
                        }

                        if (retBoard.IsKingSquareAttacked(Helper.SwitchSide(Side)))
                            retBoard.IsKingInCheck = true;

                        retBoard.IsPromotionPosition = true;
                        retBoard.PromotionSign = promSign;

                        if (!retBoard.IsKingSquareAttacked(Side))
                            _boardList.Add(retBoard);
                    }
                    
                }

                //-------------------------------BICIE NA POLE PROMOCYJNE

                foreach (int liczba in liczbyAttack)
                {
                    poleDocelowe = biezacePole + liczba;
                    znakDocelowy = board.PieceArrangement120[poleDocelowe];

                    if (znakDocelowy != '0' && znakDocelowy != '+')
                    {
                        foreach (char promSign in promSigns)
                        {
                            Board retBoard = new Board(board);

                            retBoard.StartSquare = ExtandCoords;
                            retBoard.FinishSquare = (ExtandCoords)poleDocelowe;

                            //******************************************
                            HandleTTPromotionCapture(biezacePole, poleDocelowe, retBoard, promSign);
                            //******************************************

                            retBoard.PieceArrangement120[biezacePole] = '0';
                            retBoard.PieceArrangement120[poleDocelowe] = promSign;

                            retBoard.NoCaptureOrPawnMove = 0;
                            retBoard.IsCapturePosition = true;
                            retBoard.IsPromotionPosition = true;
                            retBoard.PromotionSign = promSign;

                            if (Side == Side.White && char.IsLower(znakDocelowy))
                            {
                                Piece movingPiece = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                                retBoard.WhitePiecesList.Remove(movingPiece);
                                AddPieceToList(retBoard, promSign, (ExtandCoords)poleDocelowe);

                                Piece removeEnemyPiece = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == (ExtandCoords)poleDocelowe);
                                retBoard.BlackPiecesList.Remove(removeEnemyPiece);
                            }
                            else if (Side == Side.Black && char.IsUpper(znakDocelowy))
                            {
                                Piece movingPiece = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                                retBoard.BlackPiecesList.Remove(movingPiece);
                                AddPieceToList(retBoard, promSign, (ExtandCoords)poleDocelowe);

                                Piece removeEnemyPiece = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == (ExtandCoords)poleDocelowe);
                                retBoard.WhitePiecesList.Remove(removeEnemyPiece);
                            }
                            else
                                continue;

                            CheckRookCapture(retBoard);

                            if (retBoard.IsKingSquareAttacked(Helper.SwitchSide(Side)))
                                retBoard.IsKingInCheck = true;

                            if (!retBoard.IsKingSquareAttacked(Side))
                                _boardList.Add(retBoard);
                        }
                    }
                }
            }

        }

        void AddPieceToList(Board boardToAddPiece, char pieceSign, ExtandCoords newPosition)
        {
            if(pieceSign == 'n' || pieceSign == 'N')
            {
                if(Side == Side.White)
                {
                    boardToAddPiece.WhitePiecesList.Add(new Knight(newPosition, Side));
                }
                else
                {
                    boardToAddPiece.BlackPiecesList.Add(new Knight(newPosition, Side));
                }
            }
            else if (pieceSign == 'b' || pieceSign == 'B')
            {
                if (Side == Side.White)
                {
                    boardToAddPiece.WhitePiecesList.Add(new Bishop(newPosition, Side));
                }
                else
                {
                    boardToAddPiece.BlackPiecesList.Add(new Bishop(newPosition, Side));
                }
            }
            else if (pieceSign == 'r' || pieceSign == 'R')
            {
                if (Side == Side.White)
                {
                    boardToAddPiece.WhitePiecesList.Add(new Rook(newPosition, Side));
                }
                else
                {
                    boardToAddPiece.BlackPiecesList.Add(new Rook(newPosition, Side));
                }
            }
            else
            {
                if (Side == Side.White)
                {
                    boardToAddPiece.WhitePiecesList.Add(new Queen(newPosition, Side));
                }
                else
                {
                    boardToAddPiece.BlackPiecesList.Add(new Queen(newPosition, Side));
                }
            }
        }

        void CheckRookCapture(Board board)
        {
            if (board.FinishSquare == ExtandCoords.A1)
                board.CanWhiteCastleQueenSide = false;
            if (board.FinishSquare == ExtandCoords.H1)
                board.CanWhiteCastleKingSide = false;
            if (board.FinishSquare == ExtandCoords.A8)
                board.CanBlackCastleQueenSide = false;
            if (board.FinishSquare == ExtandCoords.H8)
                board.CanBlackCastleKingSide = false;
        }
    }
}
