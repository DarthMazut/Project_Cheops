using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cheops.AI
{
    /// <summary>
    /// Represents a logical chessboard. Holds the informations about current game state.  
    /// </summary>
    public class Board : ICloneable
    {



        #region ======================================= VARIABLES =========================================================

        public List<Board> _possibleBoardsList = new List<Board>();

        public static ulong WhiteToMoveHashValue;
        public static ulong BlackToMoveHashValue;
        public static ulong WhiteKingCastleHashValue;
        public static ulong BlackKingCastleHashValue;
        public static ulong WhiteQueenCastleHashValue;
        public static ulong BlackQueenCastleHashValue;
        public static ulong[] EnPassantSquaresHash = new ulong[64];



        #endregion

        #region ======================================= PROPERTIES ========================================================

        /// <summary>
        /// Determines the side which has a right to move in current state.
        /// </summary>
        public Side SideToMove { get; private set; } = Side.White;

        /// <summary>
        /// Represents the number of moves from last capture or pawn move. Check "50 move rule" in chess.
        /// </summary>
        public int NoCaptureOrPawnMove { get; set; } = 0;

        /// <summary>
        /// Holds a number of moves made since the beginning of the game.NOTE that 1 move = 2 ply. 
        /// </summary>
        public int MoveCount { get; set; } = 1;

        /// <summary>
        /// Holds a number of plys made since the beginning of the game.
        /// </summary>
        public int PlyCount { get; set; } = 0;

        /// <summary>
        /// Determines whether white has a right to castle king-side.
        /// </summary>
        public bool CanWhiteCastleKingSide { get; set; } = true;

        /// <summary>
        /// Determines whether white has a right to castle queen-side.
        /// </summary>
        public bool CanWhiteCastleQueenSide { get; set; } = true;

        /// <summary>
        /// Determines whether black has a right to castle king-side.
        /// </summary>
        public bool CanBlackCastleKingSide { get; set; } = true;

        /// <summary>
        /// Determines whether black has a right to castle queen-side.
        /// </summary>
        public bool CanBlackCastleQueenSide { get; set; } = true;

        /// <summary>
        /// Holds the coords of suqare that can be attacked with EnPassant maneuver.  
        /// </summary>
        public ExtandCoords? EnPassantSquare { get; set; } = null;

        /// <summary>
        /// Holds the coords of suqare where is a pawn that can be captured with EnPassant maneuver.
        /// </summary>
        public ExtandCoords? EnPassantPawn { get; set; } = null;

        /// <summary>
        /// Determines whether king of side to move is in check.
        /// </summary>
        public bool IsKingInCheck { get; set; } = false;

        //public GameState GameState { get; private set; } = GameState.Opening;

        ///// <summary>
        ///// Represents the current arrangement of pieces on  64 squares board.
        ///// </summary>
        //public char[] PieceArrangement64 { get; set; } // Wygenerowac te wartosc z tablic 120 przy kazdym gecie.

        private char[] PieceArrangement64;

        public char[] PieceArrangement
        {
            get
            {
                return Helper.Array120To64(PieceArrangement120);
            }
        }


        /// <summary>
        /// Represents the current arrangement of piece on 120 squares board.
        /// </summary>
        public char[] PieceArrangement120 { get; set; }

        /// <summary>
        /// Represent a start square of move which lead to current position.
        /// </summary>
        public ExtandCoords? StartSquare { get; set; } = null;

        /// <summary>
        /// Represent a finish square of move which lead to current position.
        /// </summary>
        public ExtandCoords? FinishSquare { get; set; } = null;

        /// <summary>
        /// Determines whether curren position has arised as a result of pawn promotion.
        /// </summary>
        public bool IsPromotionPosition { get; set; } = false;

        /// <summary>
        /// Holds white pieces.
        /// </summary>
        public List<Piece> WhitePiecesList { get; set; } = new List<Piece>();

        /// <summary>
        /// Holds black pieces.
        /// </summary>
        public List<Piece> BlackPiecesList { get; set; } = new List<Piece>();

        /// <summary>
        /// Holds the board state from previous move.
        /// </summary>
        public List<Board> PreviousBoardList { get; set; } = new List<Board>();

        /// <summary>
        /// Holds a sign of piece which was make after promoting move.
        /// </summary>
        public char PromotionSign { get; set; } = '\0';

        /// <summary>
        /// Holds a value returned by an evaluation function druing tree exploration.
        /// </summary>
        public double EvaluatedValue { get; set; } = 0;

        /// <summary>
        /// Holds a list of performed moves in string format.
        /// </summary>
        public List<string> Path { get; set; } = new List<string>();

        /// <summary>
        /// Determines whether curren position has arised as a result of piece capture.
        /// </summary>
        public bool IsCapturePosition { get; set; } = false;

        /// <summary>
        /// Holds a Zorbist Key Hashing corresponding to current position. Zero means na hash value was assigned OK?
        /// </summary>
        public ulong HashValue { get; set; } = 0;

        #endregion

        #region ================================== DEPENDENCY PROPERTIES ==================================================

        #endregion

        #region ==================================== EVENTS & DELEGATES ===================================================

        public event EventHandler MovePerformed;

        protected virtual void OnMovePerformed(EventArgs e)
        {
            MovePerformed?.Invoke(this, e);
        }

        #endregion

        #region ================================ CONSTRUCTOR / DESTRUCTOR =================================================

        /// <summary>
        /// Creates new instance of <see cref="Board"/> class. The board will represent the starting position. 
        /// </summary>
        public Board()
        {
            PieceArrangement64 = new char[64]
            {
                'r','n','b','q','k','b','n','r',
                'p','p','p','p','p','p','p','p',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                'P','P','P','P','P','P','P','P',
                'R','N','B','Q','K','B','N','R',
            };

            PieceArrangement120 = Helper.Array64To120(PieceArrangement64);
            FillPiecesLists();

        }

        /// <summary>
        /// Creates new instance of <see cref="Board"/> class based on given <see cref="Board"/> object. New instance
        /// atumatically has switched side of move. Move and ply count are incremented. <see cref="NoCaptureOrPawnMove"/>
        /// is also incremented. EnPassant and check information are set to null. Castle rights  and piece
        /// arrangement (120) are copied.
        /// </summary>
        /// <param name="board">Base board, from which the proper data is copied.</param>
        public Board(Board board)
        {
            HashValue = board.HashValue;

            if (board.SideToMove == Side.White)
            {
                SideToMove = Side.Black;
                MoveCount = board.MoveCount;

                HashValue ^= WhiteToMoveHashValue;
                HashValue ^= BlackToMoveHashValue;
            }
            else
            {
                SideToMove = Side.White;
                MoveCount = board.MoveCount + 1;

                HashValue ^= BlackToMoveHashValue;
                HashValue ^= WhiteToMoveHashValue;
            }
            
            NoCaptureOrPawnMove = board.NoCaptureOrPawnMove + 1;
            PlyCount = board.PlyCount + 1;
            
            CanBlackCastleKingSide = board.CanBlackCastleKingSide;
            CanBlackCastleQueenSide = board.CanBlackCastleQueenSide;
            CanWhiteCastleKingSide = board.CanWhiteCastleKingSide;
            CanWhiteCastleQueenSide = board.CanWhiteCastleQueenSide;

            EnPassantSquare = null;
            EnPassantPawn = null;

            IsKingInCheck = false;
            IsPromotionPosition = false;
            PromotionSign = '\0';
            IsCapturePosition = false;

            PieceArrangement120 = new char[120];
            Array.Copy(board.PieceArrangement120, PieceArrangement120, 120);

            StartSquare = null;
            FinishSquare = null;

            WhitePiecesList = board.WhitePiecesList.Clone();
            BlackPiecesList = board.BlackPiecesList.Clone();

            if(board.EnPassantSquare != null)
            {
                HashValue ^= EnPassantSquaresHash[(int)Helper.ExtandCoordsToCoords((ExtandCoords)board.EnPassantSquare)];
            }

        }

        #endregion

        #region ====================================== PUBLIC METHODS =====================================================

        /// <summary>
        /// Sets the board in starting position.
        /// </summary>
        public void ResetBoard()
        {
            PieceArrangement64 = new char[]
            {
                'r','n','b','q','k','b','n','r',
                'p','p','p','p','p','p','p','p',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                'P','P','P','P','P','P','P','P',
                'R','N','B','Q','K','B','N','R',
            };

            PieceArrangement120 = Helper.Array64To120(PieceArrangement64);

            SideToMove = Side.White;
            NoCaptureOrPawnMove = 0;
            MoveCount = 1;
            PlyCount = 0;
            CanBlackCastleKingSide = true;
            CanBlackCastleQueenSide = true;
            CanWhiteCastleKingSide = true;
            CanWhiteCastleQueenSide = true;
            EnPassantSquare = null;
            EnPassantPawn = null;
            IsPromotionPosition = false;
            PromotionSign = '\0';
            Path.Clear();
            EvaluatedValue = 0;
            IsCapturePosition = false;

            StartSquare = null;
            FinishSquare = null;
            IsKingInCheck = false;

            GenerateHashValue();

            PreviousBoardList.Clear();

            PieceArrangement120 = Helper.Array64To120(PieceArrangement64);
            FillPiecesLists();
            GenerateBoards();

        }

        /// <summary>
        /// Checks whether given FEN-string is valid, then sets the position from it and returns "true". 
        /// If give FEN-string is invalid return "false".
        /// </summary>
        public bool SetPositionFromFEN(string FEN)
        {
            string output;
            if (Helper.IsFenValid(FEN, out output))
            {
                ClearBoard();
                StartSquare = null;
                FinishSquare = null;
                IsPromotionPosition = false;
                PromotionSign = '\0';
                Path.Clear();

                string[] FENwords = FEN.Split(' ');
                SetFENPieces(FENwords[0]);
                SetFENSideToMove(FENwords[1]);
                SetFENCastlingRights(FENwords[2]);
                SetFENEnPassant(FENwords[3]);

                NoCaptureOrPawnMove = int.Parse(FENwords[4]);
                MoveCount = int.Parse(FENwords[5]);

                if (SideToMove == Side.White)
                    PlyCount = MoveCount * 2 - 1;
                else
                    PlyCount = MoveCount * 2;

                
                PieceArrangement120 = Helper.Array64To120(PieceArrangement64);
                FillPiecesLists();

                if (IsKingSquareAttacked(SideToMove))
                    IsKingInCheck = true;

                GenerateHashValue();
                GenerateBoards();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Returns FEN-string generated from the current position.
        /// </summary>
        public string ExportFEN()
        {
            string Output = "";
            PieceArrangement64 = Helper.Array120To64(PieceArrangement120);

            Output += ExportFENPieces();
            Output += " ";
            Output += ExportFENSide();
            Output += " ";
            Output += ExportFENCastleRights();
            Output += " ";
            Output += ExportFENEnPassant();
            Output += " ";
            Output += NoCaptureOrPawnMove.ToString();
            Output += " ";
            Output += MoveCount.ToString();

            return Output;
        }

        /// <summary>
        /// Determines whether king of specified side is under attack.
        /// </summary>
        /// <param name="side">Side of king that is to check.</param>
        public bool IsKingSquareAttacked(Side side)
        {
            int kingSquare = FindKingSquare(side);

            if (IsAttackByPawn(side, kingSquare))
                return true;
            if (IsAttackedByKing(side, kingSquare))
                return true;
            if (IsAttackedByKnight(side, kingSquare))
                return true;
            if (IsAttackedByBishopOrQueen(side, kingSquare))
                return true;
            if (IsAttackedByRookOrQueen(side, kingSquare))
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether specified square is under attack from point of view of given side.
        /// </summary>
        /// <param name="side">Point of view.</param>
        public bool IsSquareAttack(ExtandCoords square, Side side)
        {

            if (IsAttackByPawn(side, (int)square))
                return true;
            if (IsAttackedByKing(side, (int)square))
                return true;
            if (IsAttackedByKnight(side, (int)square))
                return true;
            if (IsAttackedByBishopOrQueen(side, (int)square))
                return true;
            if (IsAttackedByRookOrQueen(side, (int)square))
                return true;

            return false;
        }

        /// <summary>
        /// Determines how many pieces is attacking specified suqare.
        /// </summary>
        /// <param name="square">Square to examine.</param>
        /// <param name="side">Side under attack.</param>
        public int CountSquareAttackers(ExtandCoords square, Side side)
        {
            int attackers = 0;

            attackers += CountPawnAttackers(side, (int)square);
            attackers += CountKnightAttackers(side, (int)square);
            attackers += CountKingAttackers(side, (int)square);
            attackers += CountBishopOrQueenAttackers(side, (int)square);
            attackers += CountRookOrQueenAttackers(side, (int)square);

            return attackers;

        }

        /// <summary>
        /// Checks whether move between given coords is legal. If it is returns "true" and perform this move.
        /// </summary>
        /// <param name="startCoords">Starting position of the move.</param>
        /// <param name="finishCoords">Finish position on the move</param>
        /// <param name="additionalInformation">Holds additional information about the move e.g. promotion piece type.</param>
        public bool MakeMove(Coords startCoords, Coords finishCoords, char additionalInformation)
        {
            ExtandCoords GivenStart = Helper.CoordsToExtandCoords(startCoords);
            ExtandCoords GivenFinish = Helper.CoordsToExtandCoords(finishCoords);

            foreach (Board possibleBoard in _possibleBoardsList)
            {
                if(possibleBoard.StartSquare == GivenStart && possibleBoard.FinishSquare == GivenFinish)
                {
                    if(possibleBoard.IsPromotionPosition)
                    {
                        if(additionalInformation == possibleBoard.PieceArrangement120[(int)possibleBoard.FinishSquare])
                        {
                            FillPreviousBoard();
                            GetValuesFromBoard(possibleBoard);
                            GenerateBoards();
                            OnMovePerformed(new EventArgs());
                            return true;
                        }  
                    }
                    else
                    {
                        FillPreviousBoard();
                        GetValuesFromBoard(possibleBoard);
                        GenerateBoards();
                        OnMovePerformed(new EventArgs());
                        return true;
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// Searches for a last position in history list and apply it to a game.
        /// </summary>
        public void UndoMove()
        {
            GetValuesFromBoard(PreviousBoardList[PreviousBoardList.Count-1]);
            GenerateBoards();
            PreviousBoardList.RemoveAt(PreviousBoardList.Count-1);
        }

        /// <summary>
        /// Set the values of this object instance based on given <see cref="Board"/> object. 
        /// </summary>
        /// <param name="boardToCopycat">Board instance form where the values will be taken.</param>
        public void GetValuesFromBoard(Board boardToCopycat)
        {
            SideToMove = boardToCopycat.SideToMove;
            MoveCount = boardToCopycat.MoveCount;
            NoCaptureOrPawnMove = boardToCopycat.NoCaptureOrPawnMove;
            PlyCount = boardToCopycat.PlyCount;

            CanBlackCastleKingSide = boardToCopycat.CanBlackCastleKingSide;
            CanBlackCastleQueenSide = boardToCopycat.CanBlackCastleQueenSide;
            CanWhiteCastleKingSide = boardToCopycat.CanWhiteCastleKingSide;
            CanWhiteCastleQueenSide = boardToCopycat.CanWhiteCastleQueenSide;

            EnPassantSquare = boardToCopycat.EnPassantSquare;
            EnPassantPawn = boardToCopycat.EnPassantPawn;

            IsKingInCheck = boardToCopycat.IsKingInCheck;
            IsPromotionPosition = boardToCopycat.IsPromotionPosition;
            IsCapturePosition = boardToCopycat.IsCapturePosition;

            PieceArrangement120 = new char[120];
            Array.Copy(boardToCopycat.PieceArrangement120, PieceArrangement120, 120);

            StartSquare = boardToCopycat.StartSquare;
            FinishSquare = boardToCopycat.FinishSquare;

            WhitePiecesList = boardToCopycat.WhitePiecesList.Clone();
            BlackPiecesList = boardToCopycat.BlackPiecesList.Clone();

            PromotionSign = boardToCopycat.PromotionSign;
            //Path = string.Copy(boardToCopycat.Path);
            Path = new List<string>(boardToCopycat.Path);

            HashValue = boardToCopycat.HashValue;
        }

        /// <summary>
        /// IClonable interface implementation. Possible boards list and previous boards list are not cloned.
        /// </summary>
        public object Clone()
        {
            Board boardToClone = new Board();
            boardToClone.GetValuesFromBoard(this);
            return boardToClone;
        }

        /// <summary>
        /// Fill <see cref="_possibleBoardsList"/> with positions that are possible to achieve from current position.
        /// </summary>
        public void GenerateBoards()
        {
            _possibleBoardsList.Clear();

            if (SideToMove == Side.White)
            {
                foreach (Piece piece in WhitePiecesList)
                {
                    _possibleBoardsList.AddRange(piece.GenerateBoards(this));
                }
            }
            else
            {
                foreach (Piece piece in BlackPiecesList)
                {
                    _possibleBoardsList.AddRange(piece.GenerateBoards(this));
                }
            }

            foreach (Board addedBoard in _possibleBoardsList)
            {
                addedBoard.Path = new List<string>(Path);
                if(addedBoard.PromotionSign == '\0')
                addedBoard.Path.Add($"{addedBoard.StartSquare} --> {addedBoard.FinishSquare}    ");
                else
                    addedBoard.Path.Add($"{addedBoard.StartSquare} --> {addedBoard.FinishSquare} = {addedBoard.PromotionSign}");
            }

        }

        /// <summary>
        /// Generate adequate zrobist key based on current position.
        /// </summary>
        public void GenerateHashValue()
        {
            HashValue = 0;

            for (int i = 0; i < 64; i++)
            {
                if (PieceArrangement[i] != '0')
                {
                    switch (PieceArrangement[i])
                    {
                        case 'P':
                            HashValue ^= Pawn.WhiteHashValues[i];
                            break;
                        case 'p':
                            HashValue ^= Pawn.BlackHashValues[i];
                            break;
                        case 'N':
                            HashValue ^= Knight.WhiteHashValues[i];
                            break;
                        case 'n':
                            HashValue ^= Knight.BlackHashValues[i];
                            break;
                        case 'B':
                            HashValue ^= Bishop.WhiteHashValues[i];
                            break;
                        case 'b':
                            HashValue ^= Bishop.BlackHashValues[i];
                            break;
                        case 'R':
                            HashValue ^= Rook.WhiteHashValues[i];
                            break;
                        case 'r':
                            HashValue ^= Rook.BlackHashValues[i];
                            break;
                        case 'Q':
                            HashValue ^= Queen.WhiteHashValues[i];
                            break;
                        case 'q':
                            HashValue ^= Queen.BlackHashValues[i];
                            break;
                        case 'K':
                            HashValue ^= King.WhiteHashValues[i];
                            break;
                        case 'k':
                            HashValue ^= King.BlackHashValues[i];
                            break;
                    }
                }
            }

            if (SideToMove == Side.White)
                HashValue ^= WhiteToMoveHashValue;
            else
                HashValue ^= BlackToMoveHashValue;

            if (CanWhiteCastleKingSide)
                HashValue ^= WhiteKingCastleHashValue;
            if (CanBlackCastleKingSide)
                HashValue ^= BlackKingCastleHashValue;
            if (CanWhiteCastleQueenSide)
                HashValue ^= WhiteQueenCastleHashValue;
            if (CanBlackCastleQueenSide)
                HashValue ^= BlackQueenCastleHashValue;

            if(EnPassantSquare != null)
            {
                HashValue ^= EnPassantSquaresHash[(int)Helper.ExtandCoordsToCoords((ExtandCoords)EnPassantSquare)];
            }

        }

        #endregion

        #region ====================================== PRIVATE METHODS ====================================================

        void SetFENPieces(string firstWord)
        {
            int pos = 0;

            foreach (char znak in firstWord)
            {
                if (char.IsNumber(znak))
                {
                    pos += (int)char.GetNumericValue(znak);
                }
                else if (znak == '/')
                {
                    continue;
                }
                else
                {
                    PieceArrangement64[pos] = znak;
                    pos++;
                }
            }
        }

        void SetFENSideToMove(string secondWord)
        {
            if (secondWord == "w")
                SideToMove = Side.White;
            else
                SideToMove = Side.Black;
        }

        void SetFENCastlingRights(string thirdWord)
        {
            CanWhiteCastleKingSide = false;
            CanWhiteCastleQueenSide = false;
            CanBlackCastleKingSide = false;
            CanBlackCastleQueenSide = false;

            foreach (char znak in thirdWord)
            {
                if (znak == 'K') CanWhiteCastleKingSide = true;
                if (znak == 'Q') CanWhiteCastleQueenSide = true;
                if (znak == 'k') CanBlackCastleKingSide = true;
                if (znak == 'q') CanBlackCastleQueenSide = true;
            }
        }

        void SetFENEnPassant(string fourthWord)
        {
            if (fourthWord == "-")
            {
                EnPassantSquare = null;
                EnPassantPawn = null;
            }   
            else
            {
                EnPassantSquare = (ExtandCoords)Enum.Parse(typeof(ExtandCoords), fourthWord.ToUpper());
                if (EnPassantSquare.ToString().EndsWith("3"))
                    EnPassantPawn = EnPassantSquare - 10;
                else
                    EnPassantPawn = EnPassantSquare + 10;
            }
               
        }

        void ClearBoard()
        {
            for (int i = 0; i < PieceArrangement64.Length; i++)
            {
                PieceArrangement64[i] = '0';
            }
        }

        string ExportFENPieces()
        {
            string FEN = "";
            int rzad = 0;
            int przeskocz = 0;

            for (int i = 0; i < PieceArrangement64.Length; i++)
            {
                if (PieceArrangement64[i] != '0')
                {
                    if (przeskocz != 0)
                    {
                        FEN += przeskocz.ToString();
                        przeskocz = 0;
                    }

                    FEN += PieceArrangement64[i];

                    if (rzad == 7)
                    {
                        if (i != (int)Coords.H1) FEN += "/";
                        rzad = -1;
                        przeskocz = 0;
                    }

                }
                else
                {
                    przeskocz++;
                    if (rzad == 7)
                    {
                        FEN += przeskocz.ToString();
                        if (i != (int)Coords.H1) FEN += "/";
                        rzad = -1;
                        przeskocz = 0;
                    }

                }

                rzad++;
            }

            return FEN;
        }

        string ExportFENSide()
        {
            return SideToMove.ToString().ToLower()[0].ToString();
        }

        string ExportFENCastleRights()
        {
            string CastleRights = "";

            if (CanWhiteCastleKingSide) CastleRights += "K";
            if (CanWhiteCastleQueenSide) CastleRights += "Q";
            if (CanBlackCastleKingSide) CastleRights += "k";
            if (CanBlackCastleQueenSide) CastleRights += "q";

            if (CastleRights == "")
                CastleRights = "-";

            return CastleRights;
        }

        string ExportFENEnPassant()
        {
            if (EnPassantSquare != null)
                return EnPassantSquare.ToString().ToLower();
            else
                return "-";
        }

        void FillPiecesLists()
        {
            WhitePiecesList.Clear();
            BlackPiecesList.Clear();

            for (int i = 0; i < 120; i++)
            {
                if(PieceArrangement120[i] != '0' && PieceArrangement120[i] != '+')
                {
                    switch (PieceArrangement120[i])
                    {
                        case 'P':
                            WhitePiecesList.Add(new Pawn((ExtandCoords)i, Side.White));
                            break;
                        case 'p':
                            BlackPiecesList.Add(new Pawn((ExtandCoords)i, Side.Black));
                            break;
                        case 'N':
                            WhitePiecesList.Add(new Knight((ExtandCoords)i, Side.White));
                            break;
                        case 'n':
                            BlackPiecesList.Add(new Knight((ExtandCoords)i, Side.Black));
                            break;
                        case 'B':
                            WhitePiecesList.Add(new Bishop((ExtandCoords)i, Side.White));
                            break;
                        case 'b':
                            BlackPiecesList.Add(new Bishop((ExtandCoords)i, Side.Black));
                            break;
                        case 'R':
                            WhitePiecesList.Add(new Rook((ExtandCoords)i, Side.White));
                            break;
                        case 'r':
                            BlackPiecesList.Add(new Rook((ExtandCoords)i, Side.Black));
                            break;
                        case 'Q':
                            WhitePiecesList.Add(new Queen((ExtandCoords)i, Side.White));
                            break;
                        case 'q':
                            BlackPiecesList.Add(new Queen((ExtandCoords)i, Side.Black));
                            break;
                        case 'K':
                            WhitePiecesList.Add(new King((ExtandCoords)i, Side.White));
                            break;
                        case 'k':
                            BlackPiecesList.Add(new King((ExtandCoords)i, Side.Black));
                            break;
                    }               
                }
            }
        } 

        int FindKingSquare(Side side)
        {
            Piece targetPiece;
            if (side == Side.White)
                targetPiece = WhitePiecesList.Find(x => x.Sign == 'K');
            else
                targetPiece = BlackPiecesList.Find(x => x.Sign == 'k');

            return (int)targetPiece.ExtandCoords;
        }

        /// <summary>
        /// @@@@@@ UWAGA!!! Parametr "side" określa stronę która jest atakowana, a nie tę która atakuje! @@@@@@
        /// </summary>
        bool IsAttackByPawn(Side side, int squareNumber120)
        {
            if (side == Side.White)
            {
                if (PieceArrangement120[squareNumber120 - 11] == 'p')
                    return true;
                if (PieceArrangement120[squareNumber120 - 9] == 'p')
                    return true;
            }   
            else
            {
                if (PieceArrangement120[squareNumber120 + 11] == 'P')
                    return true;
                if (PieceArrangement120[squareNumber120 + 9] == 'P')
                    return true;
            }
            return false;
        }

        /// <summary>
        /// @@@@@@ UWAGA!!! Parametr "side" określa stronę która jest atakowana, a nie tę która atakuje! @@@@@@
        /// </summary>
        bool IsAttackedByKing(Side side, int squareNumber120)
        {
            char attackingSign;

            if (side == Side.White)
                attackingSign = 'k';
            else
                attackingSign = 'K';

            if (PieceArrangement120[squareNumber120 - 11] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 - 10] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 - 9] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 - 1] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 + 1] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 + 9] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 + 10] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 + 11] == attackingSign)
                return true;


            return false;
        }

        /// <summary>
        /// @@@@@@ UWAGA!!! Parametr "side" określa stronę która jest atakowana, a nie tę która atakuje! @@@@@@
        /// </summary>
        bool IsAttackedByKnight(Side side, int squareNumber120)
        {
            char attackingSign;

            if (side == Side.White)
                attackingSign = 'n';
            else
                attackingSign = 'N';

            if (PieceArrangement120[squareNumber120 - 19] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 - 8] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 - 12] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 - 21] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 + 19] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 + 8] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 + 12] == attackingSign)
                return true;
            if (PieceArrangement120[squareNumber120 + 21] == attackingSign)
                return true;


            return false;
        }

        /// <summary>
        /// @@@@@@ UWAGA!!! Parametr "side" określa stronę która jest atakowana, a nie tę która atakuje! @@@@@@
        /// </summary>
        bool IsAttackedByBishopOrQueen(Side side, int squareNumber120)
        {
            char attackingSign1;
            char attackingSign2;

            if (side == Side.White)
            {
                attackingSign1 = 'b';
                attackingSign2 = 'q';
            }   
            else
            {
                attackingSign1 = 'B';
                attackingSign2 = 'Q';
            }

            int[] liczby = { -11, -9, 9, 11 };

            foreach (int liczba in liczby)
            {
                for (int i = squareNumber120+liczba; i < 120; i += liczba)
                {
                    if (PieceArrangement120[i] == '+') // OFF BOARD
                        break;
                    if (PieceArrangement120[i] == '0') // EMPTY
                        continue;
                    if (char.IsUpper(PieceArrangement120[i]) && side == Side.White) // OWN PIECE WHITE
                        break;
                    if (char.IsLower(PieceArrangement120[i]) && side == Side.Black) // OWN PIECE BLACK
                        break;
                    if (PieceArrangement120[i] == attackingSign1 || PieceArrangement120[i] == attackingSign2) // ENYMY BISHOP
                        return true;                                                                          // OR QUEEN
                    break; //ELSE

                }
            }
            return false;
        }

        /// <summary>
        /// @@@@@@ UWAGA!!! Parametr "side" określa stronę która jest atakowana, a nie tę która atakuje! @@@@@@
        /// </summary>
        bool IsAttackedByRookOrQueen(Side side, int squareNumber120)
        {
            char attackingSign1;
            char attackingSign2;

            if (side == Side.White)
            {
                attackingSign1 = 'r';
                attackingSign2 = 'q';
            }
            else
            {
                attackingSign1 = 'R';
                attackingSign2 = 'Q';
            }

            int[] liczby = { -1, -10, 1, 10 };

            foreach (int liczba in liczby)
            {
                for (int i = squareNumber120+liczba; i < 120; i += liczba)
                {
                    if (PieceArrangement120[i] == '+') // OFF BOARD
                        break;
                    if (PieceArrangement120[i] == '0') // EMPTY
                        continue;
                    if (char.IsUpper(PieceArrangement120[i]) && side == Side.White) // OWN PIECE WHITE
                        break;
                    if (char.IsLower(PieceArrangement120[i]) && side == Side.Black) // OWN PIECE BLACK
                        break;
                    if (PieceArrangement120[i] == attackingSign1 || PieceArrangement120[i] == attackingSign2) // ENYMY BISHOP
                        return true;                                                                          // OR QUEEN
                    break; //ELSE

                }
            }
            return false;
        }

        //=======================================================================================================

        int CountPawnAttackers(Side side, int squareNumber120)
        {
            int attackers = 0;

            if (side == Side.White)
            {
                if (PieceArrangement120[squareNumber120 - 11] == 'p')
                    attackers++;
                if (PieceArrangement120[squareNumber120 - 9] == 'p')
                    attackers++;
            }
            else
            {
                if (PieceArrangement120[squareNumber120 + 11] == 'P')
                    attackers++;
                if (PieceArrangement120[squareNumber120 + 9] == 'P')
                    attackers++;
            }
            return attackers;
        }

        int CountKnightAttackers(Side side, int squareNumber120)
        {
            int attackers = 0;
            char attackingSign;

            if (side == Side.White)
                attackingSign = 'n';
            else
                attackingSign = 'N';

            if (PieceArrangement120[squareNumber120 - 19] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 - 8] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 - 12] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 - 21] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 + 19] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 + 8] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 + 12] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 + 21] == attackingSign)
                attackers++;

            return attackers;
        }

        int CountKingAttackers(Side side, int squareNumber120)
        {
            int attackers = 0;
            char attackingSign;

            if (side == Side.White)
                attackingSign = 'k';
            else
                attackingSign = 'K';

            if (PieceArrangement120[squareNumber120 - 11] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 - 10] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 - 9] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 - 1] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 + 1] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 + 9] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 + 10] == attackingSign)
                attackers++;
            if (PieceArrangement120[squareNumber120 + 11] == attackingSign)
                attackers++;

            return attackers;
        }

        int CountBishopOrQueenAttackers(Side side, int squareNumber120)
        {
            int attackers = 0;
            char attackingSign1;
            char attackingSign2;

            if (side == Side.White)
            {
                attackingSign1 = 'b';
                attackingSign2 = 'q';
            }
            else
            {
                attackingSign1 = 'B';
                attackingSign2 = 'Q';
            }

            int[] liczby = { -11, -9, 9, 11 };

            foreach (int liczba in liczby)
            {
                for (int i = squareNumber120 + liczba; i < 120; i += liczba)
                {
                    if (PieceArrangement120[i] == '+') // OFF BOARD
                        break;
                    if (PieceArrangement120[i] == '0') // EMPTY
                        continue;
                    if (char.IsUpper(PieceArrangement120[i]) && side == Side.White) // OWN PIECE WHITE
                        break;
                    if (char.IsLower(PieceArrangement120[i]) && side == Side.Black) // OWN PIECE BLACK
                        break;
                    if (PieceArrangement120[i] == attackingSign1 || PieceArrangement120[i] == attackingSign2) // ENYMY BISHOP
                        attackers++;                                                                          // OR QUEEN
                    break; //ELSE

                }
            }
            return attackers;
        }

        int CountRookOrQueenAttackers(Side side, int squareNumber120)
        {
            int attackers = 0;
            char attackingSign1;
            char attackingSign2;

            if (side == Side.White)
            {
                attackingSign1 = 'r';
                attackingSign2 = 'q';
            }
            else
            {
                attackingSign1 = 'R';
                attackingSign2 = 'Q';
            }

            int[] liczby = { -1, -10, 1, 10 };

            foreach (int liczba in liczby)
            {
                for (int i = squareNumber120 + liczba; i < 120; i += liczba)
                {
                    if (PieceArrangement120[i] == '+') // OFF BOARD
                        break;
                    if (PieceArrangement120[i] == '0') // EMPTY
                        continue;
                    if (char.IsUpper(PieceArrangement120[i]) && side == Side.White) // OWN PIECE WHITE
                        break;
                    if (char.IsLower(PieceArrangement120[i]) && side == Side.Black) // OWN PIECE BLACK
                        break;
                    if (PieceArrangement120[i] == attackingSign1 || PieceArrangement120[i] == attackingSign2) // ENYMY BISHOP
                        attackers++;                                                                          // OR QUEEN
                    break; //ELSE

                }
            }
            return attackers;
        }

        void FillPreviousBoard()
        {
            PreviousBoardList.Add(new Board());

            PreviousBoardList[PreviousBoardList.Count-1].SideToMove = SideToMove;
            PreviousBoardList[PreviousBoardList.Count-1].MoveCount = MoveCount;
            PreviousBoardList[PreviousBoardList.Count-1].NoCaptureOrPawnMove = NoCaptureOrPawnMove;
            PreviousBoardList[PreviousBoardList.Count-1].PlyCount = PlyCount;
            
            PreviousBoardList[PreviousBoardList.Count-1].CanBlackCastleKingSide = CanBlackCastleKingSide;
            PreviousBoardList[PreviousBoardList.Count-1].CanBlackCastleQueenSide = CanBlackCastleQueenSide;
            PreviousBoardList[PreviousBoardList.Count-1].CanWhiteCastleKingSide = CanWhiteCastleKingSide;
            PreviousBoardList[PreviousBoardList.Count - 1].CanWhiteCastleQueenSide = CanWhiteCastleQueenSide;
            
            PreviousBoardList[PreviousBoardList.Count-1].EnPassantSquare = EnPassantSquare;
            PreviousBoardList[PreviousBoardList.Count-1].EnPassantPawn = EnPassantPawn;
            
            PreviousBoardList[PreviousBoardList.Count-1].IsKingInCheck = IsKingInCheck;
            PreviousBoardList[PreviousBoardList.Count - 1].IsPromotionPosition = IsPromotionPosition;
            PreviousBoardList[PreviousBoardList.Count - 1].PromotionSign = PromotionSign;
            PreviousBoardList[PreviousBoardList.Count - 1].IsCapturePosition = IsCapturePosition;

            PreviousBoardList[PreviousBoardList.Count - 1].PieceArrangement120 = new char[120];
            Array.Copy(PieceArrangement120, PreviousBoardList[PreviousBoardList.Count - 1].PieceArrangement120, 120);

            PreviousBoardList[PreviousBoardList.Count - 1].StartSquare = StartSquare;
            PreviousBoardList[PreviousBoardList.Count - 1].FinishSquare = FinishSquare;

            PreviousBoardList[PreviousBoardList.Count - 1].WhitePiecesList = WhitePiecesList.Clone();
            PreviousBoardList[PreviousBoardList.Count - 1].BlackPiecesList = BlackPiecesList.Clone();

            PreviousBoardList[PreviousBoardList.Count - 1].HashValue = HashValue;
        }

        #endregion

        #region ======================================= EVENT METHODS =====================================================

        #endregion

        #region ========================================= COMMANDS ========================================================

        #endregion

















    }
}
