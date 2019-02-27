using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.AI
{
    public abstract class Piece : ICloneable
    {
        public ExtandCoords ExtandCoords { get; set; }

        public Side Side { get; set; }

        public char Sign { get; set; }

        public Piece(ExtandCoords extandCoords, Side side)
        {
            //Coords = coords;
            ExtandCoords = extandCoords;
            Side = side;
        }

        public abstract List<Board> GenerateBoards(Board board);

        /// <summary>
        /// Contains implementation of <see cref="ICloneable"/>. Returns SHALLOW copy of current object. 
        /// </summary>
        public object Clone()
        {
            //return new Knight(ExtandCoords, Side);
            return MemberwiseClone();
        }

        /// <summary>
        /// Return the <see cref="Board"/> arised by a specified move onto empty square by this piece.
        /// </summary>
        /// <param name="board">Board at which this piece is making move.</param>
        /// <param name="biezacePole">Current (starting) position of this piece.</param>
        /// <param name="poleDocelowe">Finishing (empty) square to move.</param>
        protected Board HandleEmptySquareMove(Board board, int biezacePole, int poleDocelowe)
        {
            Board retBoard = new Board(board);
            
            retBoard.StartSquare = ExtandCoords;
            retBoard.FinishSquare = (ExtandCoords)poleDocelowe;

            // *****************************************************
            HandleTTMove(biezacePole, poleDocelowe, retBoard);
            // *****************************************************

            retBoard.PieceArrangement120[biezacePole] = '0';
            retBoard.PieceArrangement120[poleDocelowe] = Sign;

            if (Side == Side.White)
            {
                Piece movingPiece = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                movingPiece.ExtandCoords = (ExtandCoords)poleDocelowe;
            }
            else
            {
                Piece movingPiece = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                movingPiece.ExtandCoords = (ExtandCoords)poleDocelowe;
            }

            if (retBoard.IsKingSquareAttacked(Side))
                return null;

            if (retBoard.IsKingSquareAttacked(Helper.SwitchSide(Side)))
                retBoard.IsKingInCheck = true;

            return retBoard;
        }

        /// <summary>
        /// Return the <see cref="Board"/> arised by a specified capture by this piece.
        /// </summary>
        /// <param name="board">Board at which capture occured.</param>
        /// <param name="biezacePole">Current (starting) position of this piece.</param>
        /// <param name="poleDocelowe">Finishing (capture) square of move.</param>
        protected Board HandleCaptureMove(Board board, int biezacePole, int poleDocelowe)
        {
            Board retBoard = new Board(board);

            retBoard.StartSquare = ExtandCoords;
            retBoard.FinishSquare = (ExtandCoords)poleDocelowe;

            // *****************************************************
            HandleTTCapture(biezacePole, poleDocelowe, retBoard);
            // *****************************************************

            retBoard.PieceArrangement120[biezacePole] = '0';
            retBoard.PieceArrangement120[poleDocelowe] = Sign;

            retBoard.NoCaptureOrPawnMove = 0;
            retBoard.IsCapturePosition = true;

            if (Side == Side.White)
            {
                if (char.IsUpper(board.PieceArrangement120[poleDocelowe]))
                    return null;

                Piece movingPiece = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                movingPiece.ExtandCoords = (ExtandCoords)poleDocelowe;

                Piece targetPiece = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == (ExtandCoords)poleDocelowe);
                retBoard.BlackPiecesList.Remove(targetPiece);

                if (retBoard.IsKingSquareAttacked(Side.White))
                    return null;
            }
            else
            {
                if (char.IsLower(board.PieceArrangement120[poleDocelowe]))
                    return null;

                Piece movingPiece = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == ExtandCoords);
                movingPiece.ExtandCoords = (ExtandCoords)poleDocelowe;

                Piece targetPiece = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == (ExtandCoords)poleDocelowe);
                retBoard.WhitePiecesList.Remove(targetPiece);

                if (retBoard.IsKingSquareAttacked(Side.Black))
                    return null;
            }

            CheckRookCapture(retBoard);
            
            if (retBoard.IsKingSquareAttacked(Helper.SwitchSide(Side)))
                retBoard.IsKingInCheck = true;

            return retBoard;
        }


        void CheckRookCapture(Board board)
        {
            if (board.FinishSquare == ExtandCoords.A1)
            {
                if(board.CanWhiteCastleQueenSide)
                {
                    board.CanWhiteCastleQueenSide = false;
                    board.HashValue ^= Board.WhiteQueenCastleHashValue;
                }
                
            }
                
            if (board.FinishSquare == ExtandCoords.H1)
            {
                if (board.CanWhiteCastleKingSide)
                {
                    board.CanWhiteCastleKingSide = false;
                    board.HashValue ^= Board.WhiteKingCastleHashValue;
                }

            }

            if (board.FinishSquare == ExtandCoords.A8)
            {
                if (board.CanBlackCastleQueenSide)
                {
                    board.CanBlackCastleQueenSide = false;
                    board.HashValue ^= Board.BlackQueenCastleHashValue;
                }

            }

            if (board.FinishSquare == ExtandCoords.H8)
            {
                if (board.CanBlackCastleKingSide)
                {
                    board.CanBlackCastleKingSide = false;
                    board.HashValue ^= Board.BlackKingCastleHashValue;
                }

            }


        }

        ulong[] GetProperHashTable(char sign)
        {
            switch (sign)
            {
                case 'P':
                    return Pawn.WhiteHashValues;
                case 'p':
                    return Pawn.BlackHashValues;
                case 'N':
                    return Knight.WhiteHashValues;
                case 'n':
                    return Knight.BlackHashValues;
                case 'B':
                    return Bishop.WhiteHashValues;
                case 'b':
                    return Bishop.BlackHashValues;
                case 'R':
                    return Rook.WhiteHashValues;
                case 'r':
                    return Rook.BlackHashValues;
                case 'Q':
                    return Queen.WhiteHashValues;
                case 'q':
                    return Queen.BlackHashValues;
                case 'K':
                    return King.WhiteHashValues;
                case 'k':
                    return King.BlackHashValues;
                default:
                    throw new Exception("Funkcja GetProperHashTable otrzymała nieprawidłowy argument.");
            }
        }

        /// <summary>
        /// Handles the Zorbis Key Changes while standard move performed. 
        /// </summary>
        /// <param name="startSquare">Starting square of move (0-119).</param>
        /// <param name="finishSquare">Finish square of move (0-119).</param>
        /// <param name="board">Board where Zorbist Key will be changed.</param>
        protected void HandleTTMove(int startSquare, int finishSquare, Board board)
        {
            char signToMove = board.PieceArrangement120[startSquare];
            ulong[] unitHashTable = GetProperHashTable(signToMove);

            int start = Helper.Int120ToInt64(startSquare);
            int finish = Helper.Int120ToInt64(finishSquare);

            board.HashValue ^= unitHashTable[start];
            board.HashValue ^= unitHashTable[finish];
        }

        /// <summary>
        /// Handles the Zorbis Key Changes while standard capture performed. 
        /// </summary>
        /// <param name="startSquare">A square from attack begins.</param>
        /// <param name="finishSquare">A square where move finishes.</param>
        /// <param name="board">Board where Zorbist Key will be changed.</param>
        protected void HandleTTCapture(int startSquare, int finishSquare, Board board)
        {
            char attackingSign = board.PieceArrangement120[startSquare];
            char attackedSign = board.PieceArrangement120[finishSquare];

            ulong[] attackingHashTable = GetProperHashTable(attackingSign);
            ulong[] attackedHashTable = GetProperHashTable(attackedSign);

            int start = Helper.Int120ToInt64(startSquare);
            int finish = Helper.Int120ToInt64(finishSquare);

            board.HashValue ^= attackingHashTable[start];
            board.HashValue ^= attackedHashTable[finish];
            board.HashValue ^= attackingHashTable[finish];
        }

        /// <summary>
        /// Handles the Zorbis Key Changes while EnPassant captured occured.
        /// </summary>
        /// <param name="startSquare">A square from attack begins.</param>
        /// <param name="finishSquare">A square where move finishes.</param>
        /// <param name="captureSquare">A square where a pawn to capture is.</param>
        /// <param name="board">Board where Zorbist Key will be changed.</param>
        protected void HandleTTEnPassantCapture(int startSquare, int finishSquare, int captureSquare, Board board)
        {
            char attackingSign = board.PieceArrangement120[startSquare];
            char attackedSign = board.PieceArrangement120[captureSquare];

            ulong[] attackingHashTable = GetProperHashTable(attackingSign);
            ulong[] attackedHashTable = GetProperHashTable(attackedSign);

            int start = Helper.Int120ToInt64(startSquare);
            int finish = Helper.Int120ToInt64(finishSquare);
            int capture = Helper.Int120ToInt64(captureSquare);

            board.HashValue ^= attackingHashTable[start];
            board.HashValue ^= attackingHashTable[finish];
            board.HashValue ^= attackedHashTable[capture];
        }

        /// <summary>
        /// Handles the Zorbis Key Changes while castling.
        /// </summary>
        protected void HandleTTCastle(Side sideToCastle, CastleType castleType, Board board)
        {
            if(sideToCastle == Side.White)
            {
                if(castleType == CastleType.KingSide)
                {
                    board.HashValue ^= King.WhiteHashValues[(int)Coords.E1];
                    board.HashValue ^= King.WhiteHashValues[(int)Coords.G1];
                    board.HashValue ^= Rook.WhiteHashValues[(int)Coords.H1];
                    board.HashValue ^= Rook.WhiteHashValues[(int)Coords.F1];
                   // board.HashValue ^= Board.WhiteKingCastleHashValue;
                }
                else
                {
                    board.HashValue ^= King.WhiteHashValues[(int)Coords.E1];
                    board.HashValue ^= King.WhiteHashValues[(int)Coords.C1];
                    board.HashValue ^= Rook.WhiteHashValues[(int)Coords.A1];
                    board.HashValue ^= Rook.WhiteHashValues[(int)Coords.D1];
                    //board.HashValue ^= Board.WhiteQueenCastleHashValue;
                }
            }
            else
            {
                if (castleType == CastleType.KingSide)
                {
                    board.HashValue ^= King.BlackHashValues[(int)Coords.E8];
                    board.HashValue ^= King.BlackHashValues[(int)Coords.G8];
                    board.HashValue ^= Rook.BlackHashValues[(int)Coords.H8];
                    board.HashValue ^= Rook.BlackHashValues[(int)Coords.F8];
                   // board.HashValue ^= Board.BlackKingCastleHashValue;
                }
                else
                {
                    board.HashValue ^= King.BlackHashValues[(int)Coords.E8];
                    board.HashValue ^= King.BlackHashValues[(int)Coords.C8];
                    board.HashValue ^= Rook.BlackHashValues[(int)Coords.A8];
                    board.HashValue ^= Rook.BlackHashValues[(int)Coords.D8];
                    //board.HashValue ^= Board.BlackQueenCastleHashValue;
                }
            }
        }

        /// <summary>
        /// Sets specified square as EnPassant square.
        /// </summary>
        /// <param name="squareToSet">Square (0-119) to set as EnPassant square.</param>
        /// <param name="board">>Board where Zorbist Key will be changed.</param>
        protected void HandleTTSetEnPassantSquare(int squareToSet, Board board)
        {
            int sq = Helper.Int120ToInt64(squareToSet);

            board.HashValue ^= Board.EnPassantSquaresHash[sq];
        }

        /// <summary>
        /// Handles the Zorbis Key Changes while promotion on empty square.
        /// </summary>
        /// <param name="startSquare">A square where pawn to promote is.</param>
        /// <param name="finishSquare">A square of promotion.</param>
        /// <param name="board">Board where Zorbist Key will be changed.</param>
        protected void HandleTTPromotionEmptySquare(int startSquare, int finishSquare, Board board, char promotionSign)
        {
            char signToMove = board.PieceArrangement120[startSquare];

            ulong[] unitHashTable = GetProperHashTable(signToMove);
            ulong[] promotionPieceHashTable = GetProperHashTable(promotionSign);

            int start = Helper.Int120ToInt64(startSquare);
            int finish = Helper.Int120ToInt64(finishSquare);

            board.HashValue ^= unitHashTable[start];
            board.HashValue ^= promotionPieceHashTable[finish];
        }

        /// <summary>
        /// Handles the Zorbis Key Changes while promotion with capture.
        /// </summary>
        /// <param name="startSquare">A square where pawn to promote is.</param>
        /// <param name="finishSquare">A square of promotion.</param>
        /// <param name="board">Board where Zorbist Key will be changed.</param>
        /// <param name="promotionSign">A sign of promotion.</param>
        protected void HandleTTPromotionCapture(int startSquare, int finishSquare, Board board, char promotionSign)
        {
            char attackingSign = board.PieceArrangement120[startSquare];
            char attackedSign = board.PieceArrangement120[finishSquare];
            ulong[] promotionPieceHashTable = GetProperHashTable(promotionSign);

            ulong[] attackingHashTable = GetProperHashTable(attackingSign);
            ulong[] attackedHashTable = GetProperHashTable(attackedSign);

            int start = Helper.Int120ToInt64(startSquare);
            int finish = Helper.Int120ToInt64(finishSquare);

            board.HashValue ^= attackingHashTable[start];
            board.HashValue ^= attackedHashTable[finish];
            board.HashValue ^= promotionPieceHashTable[finish];
        }
    }
}
