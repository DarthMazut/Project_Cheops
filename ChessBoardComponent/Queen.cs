using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.ChessBoardComponent
{
    /// <summary>
    /// Represents a queen on a chessboard.
    /// </summary>
    class Queen : Piece
    {
        /// <summary>
        /// Default constructor for a <see cref="Queen"/> class.
        /// </summary>
        /// <param name="side">Side which controls thie queen.</param>
        /// <param name="parentBoard">Board on which this queen is created.</param>
        public Queen(Side side, ChessBoard parentBoard) : base(side, parentBoard)
        {

        }
    }
}
