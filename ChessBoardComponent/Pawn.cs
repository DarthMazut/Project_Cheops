using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Cheops.ChessBoardComponent
{
    /// <summary>
    /// Represents a pawn on a chessboard.
    /// </summary>
    class Pawn : Piece
    {
        /// <summary>
        /// Default constructor for a <see cref="Pawn"/> class.
        /// </summary>
        /// <param name="side">Side which controls thie pawn.</param>
        /// <param name="parentBoard">Board on which this pawn is created.</param>
        public Pawn(Side side, ChessBoard parentBoard) : base(side, parentBoard)
        {

        }
    }
}
