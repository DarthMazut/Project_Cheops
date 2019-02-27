using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cheops.AI;

namespace Cheops.ChessBoardComponent
{
    /// <summary>
    /// Provides a chessboard with inner logic.
    /// </summary>
    public class SmartBoard : ChessBoard
    {
        /// <summary>
        /// Represent the inner logic of the <see cref="SmartBoard"/> component.
        /// </summary>
        public Board LogicalBoard { get; set; }

        /// <summary>
        /// Default constructor for <see cref="SmartBoard"/> class.
        /// </summary>
        public SmartBoard()
        {
            Board board = new Board();
            LogicalBoard = board;
            DisplayPositionFormLogicalBoard();
        }

        

        /// <summary>
        /// Displays the position from 64 square array of <see cref="LogicalBoard"/>. 
        /// </summary>
        public void DisplayPositionFormLogicalBoard()
        {
            ClearBoard();

            for (int i = 0; i < 64; i++)
            {

                char znak = LogicalBoard.PieceArrangement[i];

                switch (znak)
                {
                    case '0':
                        continue;
                    case 'P':
                        PlacePiece((Coords)i, new Pawn(Side.White, this));
                        break;
                    case 'p':
                        PlacePiece((Coords)i, new Pawn(Side.Black, this));
                        break;
                    case 'N':
                        PlacePiece((Coords)i, new Knight(Side.White, this));
                        break;
                    case 'n':
                        PlacePiece((Coords)i, new Knight(Side.Black, this));
                        break;
                    case 'B':
                        PlacePiece((Coords)i, new Bishop(Side.White, this));
                        break;
                    case 'b':
                        PlacePiece((Coords)i, new Bishop(Side.Black, this));
                        break;
                    case 'R':
                        PlacePiece((Coords)i, new Rook(Side.White, this));
                        break;
                    case 'r':
                        PlacePiece((Coords)i, new Rook(Side.Black, this));
                        break;
                    case 'Q':
                        PlacePiece((Coords)i, new Queen(Side.White, this));
                        break;
                    case 'q':
                        PlacePiece((Coords)i, new Queen(Side.Black, this));
                        break;
                    case 'K':
                        PlacePiece((Coords)i, new King(Side.White, this));
                        break;
                    case 'k':
                        PlacePiece((Coords)i, new King(Side.Black, this));
                        break;
                }
            }
        }
        
        /// <summary>
        /// Resets the board to the starting position.
        /// </summary>
        public void ResetBoard()
        {
            LogicalBoard.ResetBoard();
            DisplayPositionFormLogicalBoard();
        }

        /// <summary>
        /// Sets the position based on given FEN-string.
        /// </summary>
        /// <param name="FENstring">FEN-string.</param>
        public void SetPositionFromFEN(string FENstring)
        {
            LogicalBoard.SetPositionFromFEN(FENstring);
            DisplayPositionFormLogicalBoard();
        }
    }
}
