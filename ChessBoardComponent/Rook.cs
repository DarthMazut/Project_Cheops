using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.ChessBoardComponent
{
    /// <summary>
    /// Represents a rook on a chessboard.
    /// </summary>
    class Rook : Piece
    {
        /// <summary>
        /// Default constructor for a <see cref="Rook"/> class.
        /// </summary>
        /// <param name="side">Side which controls thie rook.</param>
        /// <param name="parentBoard">Board on which this rook is created.</param>
        public Rook(Side side, ChessBoard parentBoard) : base(side, parentBoard)
        {

        }
    }
}
