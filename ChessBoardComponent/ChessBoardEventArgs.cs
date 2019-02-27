using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cheops.ChessBoardComponent
{
    /// <summary>
    /// Event args used when square click occur.
    /// </summary>
    public class ChessBoardClickEventArgs : MouseButtonEventArgs
    {
        /// <summary>
        /// Clicked square.
        /// </summary>
        public Square Square { get; set; }

        public ChessBoardClickEventArgs(MouseDevice mouse, int timestamp, MouseButton button, Square sq) : base(mouse, timestamp, button)
        {
            Square = sq;
        }
    }

    /// <summary>
    /// Event args used when arrow click occur.
    /// </summary>
    public class ChessBoardArrowClickEventArgs : MouseButtonEventArgs
    {
        /// <summary>
        /// Clicked arrow.
        /// </summary>
        public Arrow Arrow { get; set; }

        public ChessBoardArrowClickEventArgs(MouseDevice mouse, int timestamp, MouseButton button, Arrow arrow) : base(mouse, timestamp, button)
        {
            Arrow = arrow;
        }
    }

    /// <summary>
    /// Event args used after Move was made.
    /// </summary>
    public class ChessBoardMoveEventArgs : EventArgs
    {
        /// <summary>
        /// Determines <see cref="Side"/> which has made a move.
        /// </summary>
        public Side SideToMove { get; set; }

        /// <summary>
        /// A square from which to move begun.
        /// </summary>
        public Square StartSquare { get; set; }

        /// <summary>
        /// Destination square of performed move.
        /// </summary>
        public Square FinishSquare { get; set; }


        /// <summary>
        /// Creates new instance of <see cref="ChessBoardMoveEventArgs"/>.
        /// </summary>
        /// <param name="sideMadeMove">Side which made a move.</param>
        public ChessBoardMoveEventArgs(Side sideMadeMove, Square start, Square finish)
        {
            SideToMove = sideMadeMove;
            StartSquare = start;
            FinishSquare = finish;
        }
    }


}
