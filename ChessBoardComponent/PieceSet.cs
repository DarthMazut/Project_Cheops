using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Cheops.ChessBoardComponent
{
    /// <summary>
    /// Holds the references to the pieces images.
    /// </summary>
    public class PieceSet
    {
        /// <summary>
        /// Holds the image for a white pawn.
        /// </summary>
        public BitmapImage whitePawn { get; set; }

        /// <summary>
        /// Holds the image for a white knight.
        /// </summary>
        public BitmapImage whiteKnight { get; set; }

        /// <summary>
        /// Holds the image for a white bishop.
        /// </summary>
        public BitmapImage whiteBishop { get; set; }

        /// <summary>
        /// Holds the image for a white rook.
        /// </summary>
        public BitmapImage whiteRook { get; set; }

        /// <summary>
        /// Holds the image for a white queen.
        /// </summary>
        public BitmapImage whiteQueen { get; set; }

        /// <summary>
        /// Holds the image for a white king.
        /// </summary>
        public BitmapImage whiteKing { get; set; }

        /// <summary>
        /// Holds the image for a black pawn.
        /// </summary>
        public BitmapImage blackPawn { get; set; }

        /// <summary>
        /// Holds the image for a black knight.
        /// </summary>
        public BitmapImage blackKnight { get; set; }

        /// <summary>
        /// Holds the image for a black bishop.
        /// </summary>
        public BitmapImage blackBishop { get; set; }

        /// <summary>
        /// Holds the image for a black rook.
        /// </summary>
        public BitmapImage blackRook { get; set; }
    
        /// <summary>
        /// Holds the image for a black queen.
        /// </summary>
        public BitmapImage blackQueen { get; set; }

        /// <summary>
        /// Holds the image for a black king.
        /// </summary>
        public BitmapImage blackKing { get; set; }


        /// <summary>
        /// Creates an instance of a blank <see cref="PieceSet"/> object.
        /// </summary>
        public PieceSet()
        {
            
        }

        /// <summary>
        /// Creates an instance of a <see cref="PieceSet"/> object. It will try to load the pieces from a given URI string.
        /// It will use the default naming convenction i.e. whitePawn, blackBishop, whiteKing etc.
        /// </summary>
        /// <param name="folderPath">The path to the folder which holds the pieces images.</param>
        /// <param name="uriKind">The kind of uri. Default is Relative.</param>
        /// <param name="imageFormat">The format of the piece image withour dot. Default is *.png.</param>
        public PieceSet(string folderPath, UriKind uriKind = UriKind.Relative, string imageFormat = "png")
        {
            whitePawn = new BitmapImage(new Uri(folderPath + "/whitePawn." + imageFormat, UriKind.Relative));
            whiteBishop = new BitmapImage(new Uri(folderPath + "/whiteBishop." + imageFormat, UriKind.Relative));
            whiteKnight = new BitmapImage(new Uri(folderPath + "/whiteKnight." + imageFormat, UriKind.Relative));
            whiteRook = new BitmapImage(new Uri(folderPath + "/whiteRook." + imageFormat, UriKind.Relative));
            whiteQueen = new BitmapImage(new Uri(folderPath + "/whiteQueen." + imageFormat, UriKind.Relative));
            whiteKing = new BitmapImage(new Uri(folderPath + "/whiteKing." + imageFormat, UriKind.Relative));

            blackPawn = new BitmapImage(new Uri(folderPath + "/blackPawn." + imageFormat, UriKind.Relative));
            blackBishop = new BitmapImage(new Uri(folderPath + "/blackBishop." + imageFormat, UriKind.Relative));
            blackKnight = new BitmapImage(new Uri(folderPath + "/blackKnight." + imageFormat, UriKind.Relative));
            blackRook = new BitmapImage(new Uri(folderPath + "/blackRook." + imageFormat, UriKind.Relative));
            blackQueen = new BitmapImage(new Uri(folderPath + "/blackQueen." + imageFormat, UriKind.Relative));
            blackKing = new BitmapImage(new Uri(folderPath + "/blackKing." + imageFormat, UriKind.Relative));

        }
    }
}
