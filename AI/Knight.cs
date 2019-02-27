using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.AI
{
    class Knight : Piece
    {
        public static ulong[] WhiteHashValues = new ulong[64];
        public static ulong[] BlackHashValues = new ulong[64];

        List<Board> _boardList = new List<Board>();

        int[] liczby = { -19, -8, 12, 21, 19, 8, -12, -21 };

        public Knight(ExtandCoords extandCoords, Side side) : base(extandCoords, side)
        {
            if (Side == Side.White)
                Sign = 'N';
            else
                Sign = 'n';
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
                    if (boardToAdd != null) _boardList.Add(boardToAdd);
                }
                else // CAPTURE
                {
                    Board boardToAdd = HandleCaptureMove(board, biezacePole, poleDocelowe);
                    if (boardToAdd != null) _boardList.Add(boardToAdd);
                }

            }
            return _boardList;
        }

        //void HandleEmptySquareMove(Board board, int biezacePole, int poleDocelowe)
        //{
        //    Board retBoard = new Board(board);

        //    retBoard.StartSquare = ExtandCoords;
        //    retBoard.FinishSquare = (ExtandCoords)poleDocelowe;

        //    retBoard.PieceArrangement120[biezacePole] = '0';
        //    retBoard.PieceArrangement120[poleDocelowe] = Sign;

        //    if(Side == Side.White)
        //    {
        //        Piece movingPiece = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == ExtandCoords);
        //        movingPiece.ExtandCoords = (ExtandCoords)poleDocelowe;
        //    }
        //    else
        //    {
        //        Piece movingPiece = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == ExtandCoords);
        //        movingPiece.ExtandCoords = (ExtandCoords)poleDocelowe;
        //    }

        //    if (retBoard.IsKingSquareAttacked(Side.White))
        //    return;
            
        //    _boardList.Add(retBoard);
        //}

        //void HandleCaptureMove(Board board, int biezacePole, int poleDocelowe)
        //{
        //    Board retBoard = new Board(board);

        //    retBoard.StartSquare = ExtandCoords;
        //    retBoard.FinishSquare = (ExtandCoords)poleDocelowe;

        //    retBoard.PieceArrangement120[biezacePole] = '0';
        //    retBoard.PieceArrangement120[poleDocelowe] = Sign;

        //    retBoard.NoCaptureOrPawnMove = 0;

        //    if(Side == Side.White)
        //    {
        //        if (char.IsUpper(board.PieceArrangement120[poleDocelowe]))
        //            return;

        //        Piece movingPiece = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == ExtandCoords);
        //        movingPiece.ExtandCoords = (ExtandCoords)poleDocelowe;

        //        Piece targetPiece = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == (ExtandCoords)poleDocelowe);
        //        retBoard.BlackPiecesList.Remove(targetPiece);
        //    }
        //    else
        //    {
        //        if (char.IsLower(board.PieceArrangement120[poleDocelowe]))
        //            return;

        //        Piece movingPiece = retBoard.BlackPiecesList.Find(x => x.ExtandCoords == ExtandCoords);
        //        movingPiece.ExtandCoords = (ExtandCoords)poleDocelowe;

        //        Piece targetPiece = retBoard.WhitePiecesList.Find(x => x.ExtandCoords == (ExtandCoords)poleDocelowe);
        //        retBoard.WhitePiecesList.Remove(targetPiece);
        //    }

        //    if (retBoard.IsKingSquareAttacked(Side.White))
        //    return;
        //    _boardList.Add(retBoard);
        //}
    }
}
