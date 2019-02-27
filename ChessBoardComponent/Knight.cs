using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.ChessBoardComponent
{
    /// <summary>
    /// Represents a knight on a chessboard.
    /// </summary>
    class Knight : Piece
    {
        /// <summary>
        /// Default constructor for a <see cref="Knight"/> class.
        /// </summary>
        /// <param name="side">Side which controls thie knight.</param>
        /// <param name="parentBoard">Board on which this knight is created.</param>
        public Knight(Side side, ChessBoard parentBoard) : base(side, parentBoard)
        {
            if (Side == Side.White)
                LettrSign = 'N';
            else
                LettrSign = 'n';
        }
    }
}
