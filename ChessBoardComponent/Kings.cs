using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.ChessBoardComponent
{
    /// <summary>
    /// Represents a king on a chessboard.
    /// </summary>
    class King : Piece
    {
        /// <summary>
        /// Default constructor for a <see cref="King"/> class.
        /// </summary>
        /// <param name="side">Side which controls thie king.</param>
        /// <param name="parentBoard">Board on which this king is created.</param>
        public King(Side side, ChessBoard parentBoard) : base(side, parentBoard)
        {

        }
    }
}
