using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Cheops.ChessBoardComponent
{
    public class Square : Border
    {



        #region ======================================= VARIABLES =========================================================

        #endregion

        #region ======================================= PROPERTIES ========================================================

        /// <summary>
        /// Determines square color for this square.
        /// </summary>
        public SquareColor SquareColor { get; }

        /// <summary>
        /// Determines coords for this square
        /// </summary>
        public Coords Coords { get; }

        /// <summary>
        /// Determines whether there is a <see cref="ChessBoardComponent.Piece"/> on this square.
        /// </summary>
        public bool HasPiece { get; private set; }

        /// <summary>
        /// A <see cref="Piece"/> holding by this square. NOTE! If there is no piece this square is null.
        /// </summary>
        public Piece Piece { get; private set; }

        /// <summary>
        /// Determines whether this <see cref="Square"/> is hightlighted.
        /// </summary>
        public bool IsHightlighted { get; private set; }

        /// <summary>
        /// Determines whether there is a border around this <see cref="Square"/>.
        /// </summary>
        public bool IsDistinguished { get; private set; }

        /// <summary>
        /// Determines whether this <see cref="Square"/> is in check.
        /// </summary>
        public bool IsInCheck { get; private set; }

        /// <summary>
        /// Determines whether this <see cref="Square"/> is selected.
        /// </summary>
        public bool IsSelected { get; private set; }

        #endregion

        #region ================================== DEPENDENCY PROPERTIES ==================================================

        #endregion

        #region ==================================== EVENTS & DELEGATES ===================================================

        #endregion

        #region ================================ CONSTRUCTOR / DESTRUCTOR =================================================

        /// <summary>
        /// Default constructor for <see cref="Square"/> class.
        /// </summary>
        /// <param name="i">Row of the square.</param>
        /// <param name="j">Column of the square.</param>
        public Square(int i, int j)
        {
            Grid grid = new Grid();
            grid.Background = Brushes.Transparent;
            Coords = (Coords)(i * 8 + j);

            if (i % 2 == 0) // Jesli rzad parzysty tj. 0,2,4,6
            {
                if (j % 2 == 0) // Jesli linia parzysta tj. 0,2,4,6
                {
                    //LightSquares
                    SquareColor = SquareColor.Light;
                }
                else
                {
                    //DarkSquares
                    SquareColor = SquareColor.Dark;
                }
            }
            else // Jesli rzad nieparzysty tj. 1,3,5,7
            {
                if (j % 2 == 0) // Jesli linia parzysta tj. 0,2,4,6
                {
                    //DarkSquares
                    SquareColor = SquareColor.Dark;
                }
                else
                {
                    //LightSquares
                    SquareColor = SquareColor.Light;
                }
            }

            Child = grid;

        }

        #endregion

        #region ====================================== PUBLIC METHODS =====================================================

        /// <summary>
        /// Hightlights this <see cref="Square"/>. Removes selection if any. Squares that are under check 
        /// cannot be hightlighted.
        /// </summary>
        /// <param name="color">Color in which this <see cref="Square"/> will be hightlighted.</param>
        public void Hightlight(Brush color)
        {
            if (!IsInCheck)
            {
                IsHightlighted = true;
                IsSelected = false;
                (Child as Grid).Background = color;
            }
        }

        /// <summary>
        /// Removes the hightlight form this square.
        /// </summary>
        public void ResetHightlight()
        {
            if (IsHightlighted)
            {
                IsHightlighted = false;
                (Child as Grid).Background = Brushes.Transparent;
            }

        }

        /// <summary>
        /// Draws a border around this <see cref="Square"/> with a given color. Retruns "true" if the border
        /// was already drawn.
        /// </summary>
        /// <param name="color">Color of border</param>
        public bool SetBorder(Brush color)
        {
            bool returnValue = false;

            if (IsDistinguished)
                returnValue = true;
            else IsDistinguished = true;

            BorderThickness = new Thickness(5);
            BorderBrush = color;

            return returnValue;
        }

        /// <summary>
        /// Removes the border around this <see cref="Square"/>.
        /// </summary>
        public void ResetBorder()
        {
            IsDistinguished = false;
            BorderThickness = new Thickness(0);
        }

        /// <summary>
        /// Places the <see cref="ChessBoardComponent.Piece"/> on itself. Returns "true" if placed piece replaces any other.
        /// </summary>
        public bool PlacePiece(Piece piece)
        {
            bool returnValue = false;

            if (HasPiece)
                returnValue = true;
            else HasPiece = true;

            Binding binding = new Binding("ActualWidth");
            binding.Source = this;
            piece.SetBinding(WidthProperty, binding);
            //Te dwa bloki powinny byc w konstruktorze klasy "Piece" a z jakich przyczyn tam nie mozna sie
            //odwolac do rodzica obrazka ...
            Binding binding2 = new Binding("ActualHeight");
            binding2.Source = this;
            piece.SetBinding(HeightProperty, binding2);

            Piece = piece;

            //Image i1 = new Image();
            //i1.Source = piece.Image;
            //i1.Height = 0.9 * Height;
            //i1.Width = 0.9 * Width;
            //i1.HorizontalAlignment = HorizontalAlignment.Center;
            //i1.VerticalAlignment = VerticalAlignment.Center;
            //i1.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.HighQuality);

            Grid grid = (Child as Grid);
            grid.Children.Clear();
            grid.Children.Add(piece);

            return returnValue;
        }

        /// <summary>
        /// Removes a piece form itself. Removes check as well.
        /// </summary>
        public void RemovePiece()
        {
            HasPiece = false;
            IsInCheck = false;
            Piece = null;
            Grid grid = (Child as Grid);
            grid.Children.Clear();

        }

        /// <summary>
        /// Sets current square to be in check if there is a <see cref="King"/> piece on it. Removes hightlight 
        /// from this square (if any). Returns "false" if there was no <see cref="King"/> on this suqare. Removes selection.
        /// </summary>
        public bool SetCheck(Brush color)
        {
            if (Piece is King)
            {
                IsHightlighted = false;
                IsSelected = false;
                IsInCheck = true;
                (Child as Grid).Background = color;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Removes check, hightlights and selection form this square.
        /// </summary>
        public void RemoveCheck()
        {
            if (IsInCheck)
            {
                IsInCheck = false;
                IsSelected = false;
                IsHightlighted = false;
                (Child as Grid).Background = Brushes.Transparent;
            }
        }

        /// <summary>
        /// Set selection on this square with a specified color. Removes hithtlights.
        /// </summary>
        public void Select(Brush color)
        {
            IsSelected = true;
            ResetHightlight();

            (Child as Grid).Background = color;

        }

        /// <summary>
        /// Removes selection form current <see cref="Square"/>.
        /// </summary>
        /// <param name="checkColor">Pass <see cref="ChessBoard.InCheckColor"/> here.</param>
        public void Deselect(Brush checkColor)
        {
            IsSelected = false;

            if (IsInCheck)
                SetCheck(checkColor);
            else
                (Child as Grid).Background = Brushes.Transparent;
        }


        #endregion

        #region ====================================== PRIVATE METHODS ====================================================

        #endregion

        #region ======================================= EVENT METHODS =====================================================

        #endregion

        #region ========================================= COMMANDS ========================================================

        #endregion













    }
}
