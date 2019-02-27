using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.ChessBoardComponent
{
    /// <summary>
    /// Represents a bishop on a chessboard.
    /// </summary>
    class Bishop : Piece
    {
        /// <summary>
        /// Default constructor for a <see cref="Bishop"/> class.
        /// </summary>
        /// <param name="side">Side which controls thie bishop.</param>
        /// <param name="parentBoard">Board on which this bishop is created.</param>
        public Bishop(Side side, ChessBoard parentBoard) : base(side, parentBoard)
        {

        }
    }
}
