using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Cheops.ChessBoardComponent
{
    /// <summary>
    /// Represents the chessboard.
    /// </summary>
    public partial class ChessBoard : UserControl
    {

        #region ======================================= VARIABLES =========================================================

        /// <summary>
        /// Holds a set of letters displayed at the board margin (Horizontally).
        /// </summary>
        List<string> MarginLetters = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H" };

        /// <summary>
        /// Holds a set of numbers displayed at the board margin (Vertically).
        /// </summary>
        List<string> MarginNumbers = new List<string> { "8", "7", "6", "5", "4", "3", "2", "1" };

        /// <summary>
        /// Holds a list of created arrows.
        /// </summary>
        List<Arrow> ArrowList;

        /// <summary>
        /// Holds a <see cref="Square"/> on which invoking arrow starts.
        /// </summary>
        Square ArrowInvokeSquare;

        #endregion

        #region ====================================== PROPERTIES =========================================================

        /// <summary>
        /// Holds the 64 squares currently used by the component.
        /// </summary>
        protected List<Square> SquareList { get; private set; } = new List<Square>();

        /// <summary>
        /// Holds an instance of currently selected <see cref="Square"/>.
        /// </summary>
        public Square SelectedSquare { get; private set; }

        /// <summary>
        /// Determines the current side from which user is looking at the Chessboard.
        /// </summary>
        public Side ViewSide { get; private set; } = Side.White;

        /// <summary>
        /// Determines whether the <see cref="InvokeArrow(Square)"/> method was called and awaits 
        /// for <see cref="SubmitArrow(Square)"/> method to be called.
        /// </summary>
        public bool IsDrawingArrow { get; private set; }

        #endregion

        #region ================================= DEPENDENCY PROPERTIES ================================================= 

        /// <summary>
        /// A color for light squares of the chessboard.
        /// </summary>
        public Brush LightSquareColor
        {
            get { return (Brush)GetValue(LightSquareColorProperty); }
            set { SetValue(LightSquareColorProperty, value); }
        }

        public static readonly DependencyProperty LightSquareColorProperty =
            DependencyProperty.Register(nameof(LightSquareColor), typeof(Brush), typeof(ChessBoard), new PropertyMetadata(Brushes.LightBlue));

        /// <summary>
        /// A color for dark squares of the chessboard.
        /// </summary>
        public Brush DarkSquareColor
        {
            get { return (Brush)GetValue(DarkSquareColorProperty); }
            set { SetValue(DarkSquareColorProperty, value); }
        }

        public static readonly DependencyProperty DarkSquareColorProperty =
            DependencyProperty.Register(nameof(DarkSquareColor), typeof(Brush), typeof(ChessBoard), new PropertyMetadata(Brushes.SeaGreen));


        /// <summary>
        /// Holds a set of images that represents pieces on chessboard.
        /// </summary>
        public PieceSet PieceSet
        {
            get { return (PieceSet)GetValue(PieceSetProperty); }
            set { SetValue(PieceSetProperty, value); }
        }

        public static readonly DependencyProperty PieceSetProperty =
            DependencyProperty.Register(nameof(PieceSet), typeof(PieceSet), typeof(ChessBoard), new PropertyMetadata(new PieceSet()));


        /// <summary>
        /// Gets or sets <seealso cref="FontFamily"/> for margin coords letters and numbers.
        /// </summary>
        public FontFamily CoordsFontFamily
        {
            get { return (FontFamily)GetValue(CoordsFontFamilyProperty); }
            set { SetValue(CoordsFontFamilyProperty, value); }
        }

        public static readonly DependencyProperty CoordsFontFamilyProperty =
            DependencyProperty.Register(nameof(CoordsFontFamily), typeof(FontFamily), typeof(ChessBoard), new PropertyMetadata(new FontFamily("Segoe UI")));



        /// <summary>
        /// Gets or sets the color of coords letters and numbers at the margin of chessboard.
        /// </summary>
        public Brush CoordsColor
        {
            get { return (Brush)GetValue(CoordsColorProperty); }
            set { SetValue(CoordsColorProperty, value); }
        }

        public static readonly DependencyProperty CoordsColorProperty =
            DependencyProperty.Register(nameof(CoordsColor), typeof(Brush), typeof(ChessBoard), new PropertyMetadata(Brushes.Black));


        /// <summary>
        /// Gets or sets outer border of this <see cref="ChessBoard"/> instance.
        /// </summary>
        public Thickness OuterBorderThickness
        {
            get { return (Thickness)GetValue(OuterBorderThicknessProperty); }
            set { SetValue(OuterBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty OuterBorderThicknessProperty =
            DependencyProperty.Register(nameof(OuterBorderThickness), typeof(Thickness), typeof(ChessBoard), new PropertyMetadata(new Thickness(1)));

        /// <summary>
        /// Gets or sets inner border of this <see cref="ChessBoard"/> instance.
        /// </summary>
        public Thickness InnerBorderThickness
        {
            get { return (Thickness)GetValue(InnerBorderThicknessProperty); }
            set { SetValue(InnerBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty InnerBorderThicknessProperty =
            DependencyProperty.Register(nameof(InnerBorderThickness), typeof(Thickness), typeof(ChessBoard), new PropertyMetadata(new Thickness(1)));

        /// <summary>
        /// Gets or sets color of a margin of this <see cref="ChessBoard"/> instance.
        /// </summary>
        public Brush MarginColor
        {
            get { return (Brush)GetValue(MarginColorProperty); }
            set { SetValue(MarginColorProperty, value); }
        }

        public static readonly DependencyProperty MarginColorProperty =
            DependencyProperty.Register(nameof(MarginColor), typeof(Brush), typeof(ChessBoard), new PropertyMetadata(Brushes.PeachPuff));


        /// <summary>
        /// Gets or sets color of outer margin of this <see cref="ChessBoard"/>.
        /// </summary>
        public Brush OuterBorderColor
        {
            get { return (Brush)GetValue(OuterMarginColorProperty); }
            set { SetValue(OuterMarginColorProperty, value); }
        }

        public static readonly DependencyProperty OuterMarginColorProperty =
            DependencyProperty.Register(nameof(OuterBorderColor), typeof(Brush), typeof(ChessBoard), new PropertyMetadata(Brushes.Black));

        /// <summary>
        /// Gets or sets color of inner margin of this <see cref="ChessBoard"/>.
        /// </summary>
        public Brush InnerBorderColor
        {
            get { return (Brush)GetValue(InnerMarginColorProperty); }
            set { SetValue(InnerMarginColorProperty, value); }
        }

        public static readonly DependencyProperty InnerMarginColorProperty =
            DependencyProperty.Register(nameof(InnerBorderColor), typeof(Brush), typeof(ChessBoard), new PropertyMetadata(Brushes.Black));


        /// <summary>
        /// Determines default color of hightlighted square.
        /// </summary>
        public Brush HightlightColor
        {
            get { return (Brush)GetValue(HightlightColorProperty); }
            set { SetValue(HightlightColorProperty, value); }
        }

        public static readonly DependencyProperty HightlightColorProperty =
            DependencyProperty.Register(nameof(HightlightColor), typeof(Brush), typeof(ChessBoard), new PropertyMetadata(Brushes.Orange));

        /// <summary>
        /// Gets or sets the color of a border of the square which has a set border. KURLA! Moglem pisac jednak 
        /// komentarze po polsku...
        /// </summary>
        public Brush DistinguishedBorderColor
        {
            get { return (Brush)GetValue(DistinguishedBorderColorProperty); }
            set { SetValue(DistinguishedBorderColorProperty, value); }
        }

        public static readonly DependencyProperty DistinguishedBorderColorProperty =
            DependencyProperty.Register(nameof(DistinguishedBorderColor), typeof(Brush), typeof(ChessBoard), new PropertyMetadata(Brushes.Yellow));


        /// <summary>
        /// Gets or sets the color of a square which holds a <see cref="King"/> that is currently in check.
        /// </summary>
        public Brush InCheckColor
        {
            get { return (Brush)GetValue(InCheckColorProperty); }
            set { SetValue(InCheckColorProperty, value); }
        }

        public static readonly DependencyProperty InCheckColorProperty =
            DependencyProperty.Register(nameof(InCheckColor), typeof(Brush), typeof(ChessBoard), new PropertyMetadata(Brushes.Red));


        /// <summary>
        /// Holds a <see cref="Style"/> used by every <see cref="Square"/>. NOTE that this property isn't yet fixed.
        /// </summary>
        public Style SquareStyle
        {
            get { return (Style)GetValue(SquareStyleProperty); }
            set { SetValue(SquareStyleProperty, value); }
        }

        public static readonly DependencyProperty SquareStyleProperty =
            DependencyProperty.Register(nameof(SquareStyle), typeof(Style), typeof(ChessBoard), new PropertyMetadata(new Style()));



        /// <summary>
        /// Gets or sets default color for selected <see cref="Square"/>.
        /// </summary>
        public Brush SelectionColor
        {
            get { return (Brush)GetValue(SelectionColorProperty); }
            set { SetValue(SelectionColorProperty, value); }
        }

        public static readonly DependencyProperty SelectionColorProperty =
            DependencyProperty.Register(nameof(SelectionColor), typeof(Brush), typeof(ChessBoard), new PropertyMetadata(Brushes.Lime));


        /// <summary>
        /// Determines the default color of Arrows.
        /// </summary>
        public Brush ArrowColor
        {
            get { return (Brush)GetValue(ArrowColorProperty); }
            set { SetValue(ArrowColorProperty, value); }
        }

        public static readonly DependencyProperty ArrowColorProperty =
            DependencyProperty.Register(nameof(ArrowColor), typeof(Brush), typeof(ChessBoard), new PropertyMetadata(Brushes.Purple));


        /// <summary>
        /// Determines the default thickness of arrows.
        /// </summary>
        public double ArrowThickness
        {
            get { return (double)GetValue(ArrowThicknessProperty); }
            set { SetValue(ArrowThicknessProperty, value); }
        }

        public static readonly DependencyProperty ArrowThicknessProperty =
            DependencyProperty.Register(nameof(ArrowThickness), typeof(double), typeof(ChessBoard), new PropertyMetadata(5.0));

        /// <summary>
        /// Determines the color of arrow when cursor is over.
        /// </summary>
        public Brush ArrowHoverColor
        {
            get { return (Brush)GetValue(ArrowHoverColorProperty); }
            set { SetValue(ArrowHoverColorProperty, value); }
        }

        public static readonly DependencyProperty ArrowHoverColorProperty =
            DependencyProperty.Register(nameof(ArrowHoverColor), typeof(Brush), typeof(ChessBoard), new PropertyMetadata(Brushes.Violet));



        #endregion

        #region ================================= EVENTS & DELEGATES =====================================================

        /// <summary>
        /// Mouse left button click on any <see cref="Square"/> of the chessboard.
        /// </summary>
        public event EventHandler<ChessBoardClickEventArgs> SquareLeftMouseButtonClick;

        /// <summary>
        /// Mouse right button click on any <see cref="Square"/> of the chessboard.
        /// </summary>
        public event EventHandler<ChessBoardClickEventArgs> SquareRightMouseButtonClick;

        /// <summary>
        /// Mouse left button click on any arrow.
        /// </summary>
        public event EventHandler<ChessBoardArrowClickEventArgs> ArrowLeftMouseButtonClick;

        /// <summary>
        /// Mouse right button click on any arrow.
        /// </summary>
        public event EventHandler<ChessBoardArrowClickEventArgs> ArrowRightMouseButtonClick;

        #endregion

        #region ============================= CONSTRUCTOR / DESTRUCTOR ===================================================

        /// <summary>
        /// Default constructor for a <see cref="ChessBoard"/> component. Fills the <see cref="SquareList"/>.
        /// </summary>
        public ChessBoard()
        {
            InitializeComponent();

            ArrowList = new List<Arrow>();

            SetMarginLetters();
            InitializeSquares();
            PopulateBoard();

        }



        #endregion

        #region ================================== PUBLIC METHODS =======================================================

        /// <summary>
        /// Rotates the board.
        /// </summary>
        public void RotateBoard()
        {
            InnerGrid.Children.Clear();
            //RemoveMarginLetters();
            RemoveChildrenOfType<TextBlock>(OuterGrid);
            MarginNumbers.Reverse();
            MarginLetters.Reverse();
            SquareList.Reverse();
            SetMarginLetters();
            PopulateBoard();
            SwitchViewSide();
        }

        /// <summary>
        /// Rotate the board to specified side.
        /// </summary>
        /// <param name="side">Side from which user is looking at the Chessboard.</param>
        public void RoatetBoard(Side side)
        {
            if (side != ViewSide)
                RotateBoard();

        }

        /// <summary>
        /// Hightlights the <see cref="Square"/> corresponding to given <see cref="Coords"/>.
        /// </summary>
        /// <param name="coords">Coords of <see cref="Square"/> to hightlight.</param>
        /// <param name="color">Color of hightlight.</param>
        public void HightlightSquare(Coords coords, Brush color)
        {
            Square targetSquare = SquareList.Find(x => x.Coords == coords);
            targetSquare.Hightlight(color);
        }

        /// <summary>
        /// Hightlights the square defined by given coords with a <see cref="HightlightColor"/>. 
        /// </summary>
        /// <param name="coords"></param>
        public void HightlightSquare(Coords coords)
        {
            Square targetSquare = SquareList.Find(x => x.Coords == coords);
            targetSquare.Hightlight(HightlightColor);
        }

        /// <summary>
        /// Hightlights specified squares with default color.
        /// </summary>
        /// <param name="list">List of coords to hightlight.</param>
        public void HightlightSquare(List<Coords> list)
        {
            foreach (Coords coords in list)
            {
                HightlightSquare(coords);
            }
        }

        /// <summary>
        /// Hightlights specified squares with given color.
        /// </summary>
        /// <param name="list">List of coords to hightlight.</param>
        public void HightlightSquare(List<Coords> list, Brush color)
        {
            foreach (Coords coords in list)
            {
                HightlightSquare(coords, color);
            }
        }

        /// <summary>
        /// Removes every hightlights from this board.
        /// </summary>
        public void RemoveHightlights()
        {
            foreach (Square sq in SquareList)
            {
                sq.ResetHightlight();
            }
        }

        /// <summary>
        /// Removes hightlight from specified coords.
        /// </summary>
        /// <param name="coords"></param>
        public void RemoveHightlights(Coords coords)
        {
            Square targetSquare = SquareList.Find(x => x.Coords == coords);
            targetSquare.ResetHightlight();
        }

        /// <summary>
        /// Draws a border of given color around specified square of chessboard.
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="color"></param>
        public void DrawBorder(Coords coords, Brush color)
        {
            Square targetSquare = SquareList.Find(x => x.Coords == coords);
            targetSquare.SetBorder(color);
        }

        /// <summary>
        /// Draws a border of default color (<see cref="DistinguishedBorderColor"/>) around specified square of chessboard.
        /// </summary>
        /// <param name="coords"></param>
        public void DrawBorder(Coords coords)
        {
            Square targetSquare = SquareList.Find(x => x.Coords == coords);
            targetSquare.SetBorder(DistinguishedBorderColor);
        }

        /// <summary>
        /// Removes border from every square on this board.
        /// </summary>
        public void RemoveBorder()
        {
            foreach (Square sq in SquareList)
            {
                sq.ResetBorder();
            }
        }

        /// <summary>
        /// Removes a border around the square specified by coords.
        /// </summary>
        /// <param name="coords"></param>
        public void RemoveBorder(Coords coords)
        {
            Square targetSquare = SquareList.Find(x => x.Coords == coords);
            targetSquare.ResetBorder();
        }

        //TEST
        public void SetColors()
        {
            Random k = new Random();
            Brush ls = new SolidColorBrush(new Color() { A = 100, R = (byte)k.Next(255), G = (byte)k.Next(255), B = (byte)k.Next(255) });
            Brush ds = new SolidColorBrush(new Color() { A = 100, R = (byte)k.Next(255), G = (byte)k.Next(255), B = (byte)k.Next(255) });

            Brush ar = new SolidColorBrush(new Color() { A = 100, R = (byte)k.Next(255), G = (byte)k.Next(255), B = (byte)k.Next(255) });

            Brush ah = new SolidColorBrush(new Color() { A = 100, R = (byte)k.Next(255), G = (byte)k.Next(255), B = (byte)k.Next(255) });

            LightSquareColor = ls;
            DarkSquareColor = ds;
            ArrowColor = ar;
            ArrowHoverColor = ah;
        }

        /// <summary>
        /// Places given <see cref="Piece"/> at specified <see cref="Coords"/>. Returns "true" if placed piece 
        /// replaced any other.
        /// </summary>
        /// <param name="coords">Coords of placed piece.</param>
        /// <param name="piece">Placed piece.</param>
        public bool PlacePiece(Coords coords, Piece piece)
        {
            Square targetSquare = SquareList.Find(x => x.Coords == coords);
            return targetSquare.PlacePiece(piece);
        }

        /// <summary>
        /// Places  given <see cref="Piece"/> at specified <see cref="Square"/>. Returns "true" if placed piece 
        /// replaced any other.
        /// </summary>
        /// <param name="square">Square on which to piece is placed.</param>
        /// <param name="piece">A piece to place.</param>
        public bool PlacePiece(Square square, Piece piece)
        {
            return square.PlacePiece(piece);
        }

        /// <summary>
        /// Determines whether there is any <see cref="Piece"/> on specified square (<see cref="Coords"/>).
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public bool IsPieceOnSquare(Coords coords)
        {
            Square targetSquare = SquareList.Find(x => x.Coords == coords);
            return targetSquare.HasPiece;
        }

        /// <summary>
        /// Determines whether there is a <see cref="Piece"/> on specified square of given side.
        /// </summary>
        /// <param name="coords">Coords which will be checked.</param>
        /// <param name="side">Side to check.</param>
        public bool IsPieceOnSquare(Coords coords, Side side)
        {
            Square targetSquare = SquareList.Find(x => x.Coords == coords);
            if (targetSquare.Piece?.Side == side)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Determines whether a specified <see cref="Piece"/> is on given square.
        /// </summary>
        /// <param name="coords">Coords where to look.</param>
        /// <param name="piece">A specified piece, e.g. white rook.</param>
        public bool IsPieceOnSquare(Coords coords, Piece piece)
        {
            Square targetSquare = SquareList.Find(x => x.Coords == coords);
            if (targetSquare.Piece != null)
            {
                if (targetSquare.Piece == piece)
                    return true;
                else
                    return false;
            }
            else
                return false;


        }

        /// <summary>
        /// Returns the <see cref="Piece"/> object from given <see cref="Coords"/>. Return NULL if there were no piece
        /// on specified coords.
        /// </summary>
        /// <param name="coords">Coords from where a <see cref="Piece"/> object has to be returned.</param>
        public Piece GetPiece(Coords coords)
        {
            Square targetSquare = SquareList.Find(x => x.Coords == coords);

            if (targetSquare.Piece != null)
            {
                return targetSquare.Piece;
            }
            else
                return null;
        }

        /// <summary>
        /// Returns currently selected piece. Return NULL if there is no piece on currently selected square.
        /// </summary>
        public Piece GetSelectedPiece()
        {
            if(SelectedSquare != null)
            {
                if(SelectedSquare.Piece != null)
                {
                    return SelectedSquare.Piece;
                }
            }
            return null;
        }

        /// <summary>
        /// Removes the piece from given <see cref="Coords"/>.
        /// </summary>
        /// <param name="coords">Given coords.</param>
        public void RemovePiece(Coords coords)
        {
            Square targetSquare = SquareList.Find(x => x.Coords == coords);
            targetSquare.RemovePiece();
        }

        /// <summary>
        /// Removes the piece from specified <see cref="Square"/>.
        /// </summary>
        /// <param name="square">Specified square.</param>
        public void RemovePiece(Square square)
        {
            square.RemovePiece();
        }

        /// <summary>
        /// Removes all pieces form the board.
        /// </summary>
        public void ClearBoard()
        {
            RemoveArrow();

            foreach (Square sq in SquareList)
            {
                sq.RemoveCheck();
                sq.ResetHightlight();
                sq.ResetBorder();
                sq.RemovePiece();
            }
        }

        /// <summary>
        /// Sets a specified <see cref="Square"/> to be in check. It has to have a <see cref="King"/> on it. Returns false
        /// if no changes were made. If check was set removes checks from any other squares of the chessboard.
        /// </summary>
        /// <param name="coords"></param>
        public bool SetCheckSquare(Coords coords)
        {
            Square targetSquare = SquareList.Find(x => x.Coords == coords);
            if (targetSquare.SetCheck(InCheckColor))
            {
                foreach (Square sq in SquareList)
                {
                    if (sq != targetSquare)
                        sq.RemoveCheck();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Searches for a <see cref="King"/> of given color and marks the <see cref="Square"/> underneath him as check 
        /// square.
        /// </summary>
        /// <param name="side"></param>
        public void SetCheck(Side side)
        {
            foreach (Square sq in SquareList)
            {
                if (sq.Piece is King && sq.Piece.Side == side)
                {
                    sq.SetCheck(InCheckColor);
                    break;
                }
            }
        }

        /// <summary>
        /// Removes check from any <see cref="Square"/>.
        /// </summary>
        public void RemoveChecks()
        {
            foreach (Square sq in SquareList)
            {
                sq.RemoveCheck();
            }
        }

        /// <summary>
        /// Marks the given <see cref="Square"/> as selected. Removes any other selections and hightlights.
        /// </summary>
        /// <param name="coords"></param>
        public void SelectSquare(Coords coords)
        {
            Deselect();
            RemoveHightlights();
            Square targetSquare = SquareList.Find(x => x.Coords == coords);
            SelectedSquare = targetSquare;
            targetSquare.Select(SelectionColor);
        }

        /// <summary>
        /// Removes selection from the board.
        /// </summary>
        public void Deselect()
        {
            SelectedSquare?.Deselect(InCheckColor);
            SelectedSquare = null;

        }

        /// <summary>
        /// Draws an arrow on the chessboard.
        /// </summary>
        /// <param name="startCoords">Coords of square where the arrow begins.</param>
        /// <param name="finishCoords">Coords of square to which the arrow points at.</param>
        public void DrawArrow(Coords startCoords, Coords finishCoords)
        {
            Square startSquare = SquareList.Find(x => x.Coords == startCoords);
            Square finishSquare = SquareList.Find(x => x.Coords == finishCoords);

            DrawArrow(startSquare, finishSquare);

        }

        /// <summary>
        /// Draws an arrow on the chessboard.
        /// </summary>
        /// <param name="startSquare">Square where arrow begins.</param>
        /// <param name="finishSquare">Square to which the arrow points at.</param>
        public void DrawArrow(Square startSquare, Square finishSquare)
        {
            if (startSquare != finishSquare)
            {
                Arrow arrow = new Arrow();
                arrow.StartSquare = startSquare;
                arrow.FinishSquare = finishSquare;

                arrow.HeadHeight = 10;
                arrow.HeadWidth = 20;

                arrow.Style = (Style)FindResource("arrowStyle");

                Point startPoint = startSquare.TransformToAncestor(InnerGrid).Transform(new Point(startSquare.ActualWidth / 2, startSquare.ActualHeight / 2));

                Point finishPoint = finishSquare.TransformToAncestor(InnerGrid).Transform(new Point(finishSquare.ActualWidth / 2, finishSquare.ActualHeight / 2));

                arrow.X1 = startPoint.X;
                arrow.Y1 = startPoint.Y;
                arrow.X2 = finishPoint.X;
                arrow.Y2 = finishPoint.Y;

                arrow.SetValue(Grid.ColumnSpanProperty, 8);
                arrow.SetValue(Grid.RowSpanProperty, 8);

                arrow.MouseLeftButtonDown += OnArrowLeftClick;
                arrow.MouseRightButtonDown += OnArrowRightClick;

                ArrowList.Add(arrow);
                InnerGrid.Children.Add(arrow); 
            }
        }

        /// <summary>
        /// Starts drawing an arrow.
        /// </summary>
        /// <param name="startCoord">Starting Square of drawn arrow.</param>
        public void InvokeArrow(Square startSquare)
        {
            IsDrawingArrow = true;
            ArrowInvokeSquare = startSquare;
        }

        /// <summary>
        /// Start drawing an arrow.
        /// </summary>
        /// <param name="startCoords">Starting coords of drawn arrow.</param>
        public void InvokeArrow(Coords startCoords)
        {
            Square startSquare = SquareList.Find(x => x.Coords == startCoords);

            InvokeArrow(startSquare);
        }

        /// <summary>
        /// Sumbits <see cref="Arrow"/> invoked by <see cref="InvokeArrow(Square)"/> method.
        /// </summary>
        /// <param name="submitSquare">A square to which arrow will be pointing at.</param>
        public void SubmitArrow(Square submitSquare)
        {

            if (IsDrawingArrow)
            {
                IsDrawingArrow = false;

                if (ArrowInvokeSquare != null)
                    DrawArrow(ArrowInvokeSquare.Coords, submitSquare.Coords);

                ArrowInvokeSquare = null; 
            }
        }

        /// <summary>
        /// Sumbits <see cref="Arrow"/> invoked by <see cref="InvokeArrow(Coords)"/> method.
        /// </summary>
        /// <param name="submitSquare">A coords to which arrow will be pointing at.</param>
        public void SubmitArrow(Coords submitCoords)
        {
            Square submitSquare = SquareList.Find(x => x.Coords == submitCoords);

            SubmitArrow(submitSquare);
        }

        /// <summary>
        /// Removes targeted arrow from chessboard and <see cref="ArrowList"/>.
        /// </summary>
        /// <param name="arrowToRemove"></param>
        public void RemoveArrow(Arrow arrowToRemove)
        {
            InnerGrid.Children.Remove(arrowToRemove);
            ArrowList.Remove(arrowToRemove);
        }

        /// <summary>
        /// Removes every arrow from the chessboard.
        /// </summary>
        public void RemoveArrow()
        {
            RemoveChildrenOfType<Arrow>(InnerGrid);
            ArrowList.Clear();

        }

        /// <summary>
        /// Returns first occurance of arrow currently pointed by a cursor. Returns null if no <see cref="Arrow"/>
        /// meets the conditions.
        /// </summary>
        public Arrow GetHoveredArrow()
        {
            foreach (Arrow arrow in ArrowList)
            {
                if(arrow.IsMouseOver)
                {
                    return arrow;
                }
            }

            return null;
        }

        #endregion

        #region ================================ PRIVATE METHODS =======================================================

        /// <summary>
        /// Places the margin letters and numbers at thier destinate position.
        /// </summary>
        void SetMarginLetters()
        {
            //LITERKI
            for (int i = 0; i < 8; i++)
            {
                TextBlock literkaGorna = new TextBlock();
                literkaGorna.Text = MarginLetters[i];
                literkaGorna.SetValue(Grid.ColumnProperty, i + 1);
                literkaGorna.SetValue(Grid.RowProperty, 0);
                literkaGorna.HorizontalAlignment = HorizontalAlignment.Center;
                literkaGorna.VerticalAlignment = VerticalAlignment.Center;

                Binding bindingFontUpper = new Binding("CoordsFontFamily");
                bindingFontUpper.Source = this;
                literkaGorna.SetBinding(FontFamilyProperty, bindingFontUpper);

                Binding bindingColorUpper = new Binding("CoordsColor");
                bindingColorUpper.Source = this;
                literkaGorna.SetBinding(ForegroundProperty, bindingColorUpper);

                OuterGrid.Children.Add(literkaGorna);

                TextBlock literkaDolna = new TextBlock();
                literkaDolna.Text = MarginLetters[i];
                literkaDolna.SetValue(Grid.ColumnProperty, i + 1);
                literkaDolna.SetValue(Grid.RowProperty, 9);
                literkaDolna.HorizontalAlignment = HorizontalAlignment.Center;
                literkaDolna.VerticalAlignment = VerticalAlignment.Center;

                Binding bindingFontLower = new Binding("CoordsFontFamily");
                bindingFontLower.Source = this;
                literkaDolna.SetBinding(FontFamilyProperty, bindingFontLower);

                Binding bindingColorLower = new Binding("CoordsColor");
                bindingColorLower.Source = this;
                literkaDolna.SetBinding(ForegroundProperty, bindingColorLower);

                OuterGrid.Children.Add(literkaDolna);
            }

            //CYFERKI
            for (int i = 0; i < 8; i++)
            {
                TextBlock cyferkaGorna = new TextBlock();
                cyferkaGorna.Text = MarginNumbers[i];
                cyferkaGorna.SetValue(Grid.ColumnProperty, 0);
                cyferkaGorna.SetValue(Grid.RowProperty, i + 1);
                cyferkaGorna.HorizontalAlignment = HorizontalAlignment.Center;
                cyferkaGorna.VerticalAlignment = VerticalAlignment.Center;

                Binding bindingFontUpper = new Binding("CoordsFontFamily");
                bindingFontUpper.Source = this;
                cyferkaGorna.SetBinding(FontFamilyProperty, bindingFontUpper);

                Binding bindingColorUpper = new Binding("CoordsColor");
                bindingColorUpper.Source = this;
                cyferkaGorna.SetBinding(ForegroundProperty, bindingColorUpper);

                OuterGrid.Children.Add(cyferkaGorna);

                TextBlock cyferkaDolna = new TextBlock();
                cyferkaDolna.Text = MarginNumbers[i];
                cyferkaDolna.SetValue(Grid.ColumnProperty, 9);
                cyferkaDolna.SetValue(Grid.RowProperty, i + 1);
                cyferkaDolna.HorizontalAlignment = HorizontalAlignment.Center;
                cyferkaDolna.VerticalAlignment = VerticalAlignment.Center;

                Binding bindingFontLower = new Binding("CoordsFontFamily");
                bindingFontLower.Source = this;
                cyferkaDolna.SetBinding(FontFamilyProperty, bindingFontLower);

                Binding bindingColorLower = new Binding("CoordsColor");
                bindingColorLower.Source = this;
                cyferkaDolna.SetBinding(ForegroundProperty, bindingColorLower);

                OuterGrid.Children.Add(cyferkaDolna);

            }
        }

        /// <summary>
        /// Inserts the 64 squares from <see cref="SquareList"/> into inner grid. Also inserts arrows.
        /// </summary>
        void PopulateBoard()
        {
           int index = 0;

           for (int i = 0; i < 8; i++)
           {
               for (int j = 0; j < 8; j++)
               {
                   SquareList[index].SetValue(Grid.ColumnProperty, j);
                   SquareList[index].SetValue(Grid.RowProperty, i);
                   InnerGrid.Children.Add(SquareList[index]);
                   index++;
                }
            }

            RefreshArrows(true);
            
        }

        /// <summary>
        /// Refreshes the properties of drawn arrows (<see cref="ArrowList"/>).
        /// </summary>
        /// <param name="addChildren">Determines whether this method only refreshes properties of arrows or if it 
        /// additionaly add them as a children of proper (InnerGrid) component.</param>
        void RefreshArrows(bool addChildren)
        {
            foreach (Arrow a in ArrowList)
            {
                if (addChildren)
                {
                    InnerGrid.Children.Add(a);
                    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                    UpdateLayout();
                    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                }


                Point startPoint = a.StartSquare.TransformToAncestor(InnerGrid).Transform(new Point(a.StartSquare.ActualWidth / 2, a.StartSquare.ActualHeight / 2));

                Point finishPoint = a.FinishSquare.TransformToAncestor(InnerGrid).Transform(new Point(a.FinishSquare.ActualWidth / 2, a.FinishSquare.ActualHeight / 2));

                a.X1 = startPoint.X;
                a.Y1 = startPoint.Y;
                a.X2 = finishPoint.X;
                a.Y2 = finishPoint.Y;
            }
        }

        /// <summary>
        /// Changes the information which shows the <see cref="ToolTip"/> of single <see cref="Square"/>.
        /// This method is called every time a mouse enters the <see cref="Square"/>.
        /// </summary>
        /// <param name="sq">An <see cref="Square"/> object to manipulate. </param>
        protected virtual void SetTooltip(Square sq)
        {
            string coords = sq.Coords.ToString();
            string piece;

            if (sq.HasPiece)
                piece = sq.Piece.ToString();
            else
                piece = "- - -";

            sq.ToolTip = $"Pole: {coords}\nBierka: {piece}";
        }

        /// <summary>
        /// Switches to <see cref="ViewSide"/> property to oposite value.
        /// </summary>
        void SwitchViewSide()
        {
            if (ViewSide == Side.White)
                ViewSide = Side.Black;
            else
                ViewSide = Side.White;
        }

        /// <summary>
        /// Removes letters (<see cref="TextBlock"/>s) from margin of the chessboard.
        /// </summary>
        [Obsolete("Check RemoveChildrenOfType.")]
        void RemoveMarginLetters()
        {
            for (int i = OuterGrid.Children.Count - 1; i >= 0; i--)
            {
                if (OuterGrid.Children[i] is TextBlock)
                    OuterGrid.Children.RemoveAt(i);
            }

        }

        /// <summary>
        /// Removes every element of type <see cref="T"/> that is a child of parentElement.
        /// </summary>
        /// <typeparam name="T">Type of removing elements.</typeparam>
        /// <param name="parentElement">Parent element whos children will be removed.</param>
        void RemoveChildrenOfType<T>(Panel parentElement)
        {
            for (int i = parentElement.Children.Count - 1; i >= 0; i--)
            {
                if (parentElement.Children[i] is T)
                    parentElement.Children.RemoveAt(i);
            }
        }

        /// <summary>
        /// Initializes 64 <see cref="Square"/>s of the chessboard. Fills the <see cref="SquareList"/>.
        /// </summary>
        void InitializeSquares()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Square sq = new Square(i, j);
                    SquareList.Add(sq); // w razie czego przerzucic to na spod !!!!!!!!!!!!!!!!!!!!!;

                    SetBindingForSquareColors(sq);
                    //SetBindingForStyle(sq);
                    SetBindingLink(sq, StyleProperty, this, "SquareStyle");

                    RegisterEventsForSquares(sq);
                }

            }
        }

        /// <summary>
        /// Registers event methods for given square.
        /// </summary>
        /// <param name="sq">Square for register.</param>
        void RegisterEventsForSquares(Square sq)
        {
            sq.Child.MouseEnter += OnSquareMouseEnter;
            sq.Child.MouseLeftButtonDown += OnSquareClick;
            sq.Child.MouseRightButtonDown += OnSquareRightClick;
        }

        /// <summary>
        /// Sets the DataBinding that is responsible for square Style. 
        /// </summary>
        /// <param name="sq">The <see cref="Square"/> that binding is destinate for.</param>
        [Obsolete]
        void SetBindingForStyle(Square sq)
        {
            Binding styleBinding = new Binding("SquareStyle");
            styleBinding.Source = this;
            sq.SetBinding(StyleProperty, styleBinding);
        }

        /// <summary>
        /// Sets default binding between objects.
        /// </summary>
        /// <param name="drainingElement">Element that gets value from another element.</param>
        /// <param name="drainingProperty">Property which will get the value.</param>
        /// <param name="givingElement">Element which is a source of binding value.</param>
        /// <param name="givingProperty">Source-Property for binding.</param>
        void SetBindingLink(FrameworkElement drainingElement,
                        DependencyProperty drainingProperty , 
                        object givingElement, 
                        string givingProperty)
        {
            Binding binding = new Binding(givingProperty);
            binding.Source = givingElement;
            drainingElement.SetBinding(drainingProperty, binding);
        }

        /// <summary>
        /// Sets the DataBinding that is responsible for square color. NOTE that this method is called within
        /// <see cref="InitializeSquares"/>.
        /// </summary>
        /// <param name="sq">The <see cref="Square"/> that binding is destinate for.</param>
        void SetBindingForSquareColors(Square sq)
        {
            Binding binding;

            if (sq.SquareColor == SquareColor.Light)
                binding = new Binding("LightSquareColor");
            else
                binding = new Binding("DarkSquareColor");

            binding.Source = this;
            sq.SetBinding(BackgroundProperty, binding);
        }

        #endregion

        #region ================================ EVENT METHODS ========================================================

        private void OnSquareClick(object sender, MouseButtonEventArgs e)
        {
            SquareLeftMouseButtonClick?.Invoke(this, new ChessBoardClickEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left, (Square)(sender as Grid).Parent));
        }

        private void OnSquareRightClick(object sender, MouseButtonEventArgs e)
        {
            SquareRightMouseButtonClick?.Invoke(this, new ChessBoardClickEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Right, (Square)(sender as Grid).Parent));
        }

        private void OnArrowLeftClick(object sender, MouseButtonEventArgs e)
        {
            ArrowLeftMouseButtonClick?.Invoke(this, new ChessBoardArrowClickEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Right, sender as Arrow));
        }

        private void OnArrowRightClick(object sender, MouseButtonEventArgs e)
        {
            ArrowRightMouseButtonClick?.Invoke(this, new ChessBoardArrowClickEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Right, sender as Arrow));
        }

        private void OnSquareMouseEnter(object sender, MouseEventArgs e)
        {
            SetTooltip((Square)(sender as Grid).Parent);
        }

        private void Component_Resize(object sender, SizeChangedEventArgs e)
        {
            RefreshArrows(false);
        }


        #endregion

        #region ================================== COMMANDS ==========================================================

        #endregion

        

    }
}
