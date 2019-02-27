using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Cheops.ChessBoardComponent
{
    /// <summary>
    /// Base class for pieces on a chessboard i.e. pawns, knights, bishops etc.
    /// </summary>
    public abstract class Piece : Image
    {
        /// <summary>
        /// Side that controls this piece.
        /// </summary>
        public Side Side { get; }

        /// <summary>
        /// Holds the shortcut letter used in FEN-notation to determine a piece.
        /// </summary>
        public char LettrSign { get; protected set; }

        /// <summary>
        /// Base constructor for classes that derive from this class.
        /// </summary>
        /// <param name="side">Side which controls created piece.</param>
        /// <param name="parentBoard">Board on which this piece is created.</param>
        public Piece(Side side, ChessBoard parentBoard)
        {
            Side = side;
            SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.HighQuality);
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;

            Binding binding = new Binding($"PieceSet.{Side.ToString().ToLower()}{GetType().Name}");
            binding.Source = parentBoard; // Jak znalezc ChessBoard w Drzewie wizualnym?
            SetBinding(SourceProperty, binding);

            if (Side == Side.White)
                LettrSign = GetType().Name.ToUpper()[0];
            else
                LettrSign = GetType().Name.ToLower()[0];

        }

        public override string ToString()
        {
            return $"{Side.ToString()} {GetType().Name}";
        }

    }
}
