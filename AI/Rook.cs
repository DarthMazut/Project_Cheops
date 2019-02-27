using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.AI
{
    class Rook : Piece
    {
        public static ulong[] WhiteHashValues = new ulong[64];
        public static ulong[] BlackHashValues = new ulong[64];

        List<Board> _boardList = new List<Board>();

        int[] _liczby = { -10, 1, 10, -1 };

        char[] _ownSigns;
        char[] _enemySigns;

        public Rook(ExtandCoords extandCoords, Side side) : base(extandCoords, side)
        {
            if (Side == Side.White)
            {
                Sign = 'R';
                _ownSigns = new char[] { 'P', 'R', 'N', 'B', 'Q', 'K' };
                _enemySigns = new char[] { 'p', 'r', 'n', 'b', 'q', 'k' };
            }
            else
            {
                Sign = 'r';
                _ownSigns = new char[] { 'p', 'r', 'n', 'b', 'q', 'k' };
                _enemySigns = new char[] { 'P', 'R', 'N', 'B', 'Q', 'K' };
            }
        }

        public override List<Board> GenerateBoards(Board board)
        {
            _boardList.Clear();


            int biezacePole = (int)ExtandCoords;


            foreach (int liczba in _liczby)
            {
                for (int i = biezacePole + liczba; i > 0; i += liczba)
                {
                    if (board.PieceArrangement120[i] == '+') // OFF-BOARD
                        break;
                    if (board.PieceArrangement120[i] == '0') // EMPTY SQUARE
                    {
                        Board boardToAdd = HandleEmptySquareMove(board, biezacePole, i);
                        if (boardToAdd != null)
                        {
                            RemoveCastleRights(boardToAdd, biezacePole);
                            _boardList.Add(boardToAdd);
                        }
                    }
                    if (_ownSigns.Contains(board.PieceArrangement120[i]))
                        break;
                    if (_enemySigns.Contains(board.PieceArrangement120[i]))
                    {
                        Board boardToAdd = HandleCaptureMove(board, biezacePole, i);
                        if (boardToAdd != null)
                        {
                            RemoveCastleRights(boardToAdd, biezacePole);
                            _boardList.Add(boardToAdd);
                        }
                        break;
                    }
                }
            }
            return _boardList;
        }


        void RemoveCastleRights(Board board, int startSquare)
        {
            if (startSquare == 91)
                board.CanWhiteCastleQueenSide = false;
            if (startSquare == 98)
                board.CanWhiteCastleKingSide = false;
            if (startSquare == 21)
                board.CanBlackCastleQueenSide = false;
            if (startSquare == 28)
                board.CanBlackCastleKingSide = false;

        }


    }   

}
