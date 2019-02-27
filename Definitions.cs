using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops
{
    /// <summary>
    /// Is a set of all 64 coords that are on a chessboard.
    /// </summary>
    public enum Coords
    {
        A8, B8, C8, D8, E8, F8, G8, H8,
        A7, B7, C7, D7, E7, F7, G7, H7,
        A6, B6, C6, D6, E6, F6, G6, H6,
        A5, B5, C5, D5, E5, F5, G5, H5,
        A4, B4, C4, D4, E4, F4, G4, H4,
        A3, B3, C3, D3, E3, F3, G3, H3,
        A2, B2, C2, D2, E2, F2, G2, H2,
        A1, B1, C1, D1, E1, F1, G1, H1

    };

    /// <summary>
    /// There are light and dark squares on a chessboard. 
    /// </summary>
    public enum SquareColor
    {
        Light,
        Dark
    }

    /// <summary>
    /// There are two sides that are playing the chess game.
    /// </summary>
    public enum Side
    {
        White,
        Black
    };

    /// <summary>
    /// There are six types of pieces.
    /// </summary>
    public enum PieceType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    };

    /// <summary>
    /// The are two castle types.
    /// </summary>
    public enum CastleType
    {
        KingSide,
        QueenSide
    };

    /// <summary>
    /// Coords used to work with 120 char array.
    /// </summary>
    public enum ExtandCoords
    {
        A8 = 21, B8, C8, D8, E8, F8, G8, H8,
        A7 = 31, B7, C7, D7, E7, F7, G7, H7,
        A6 = 41, B6, C6, D6, E6, F6, G6, H6,
        A5 = 51, B5, C5, D5, E5, F5, G5, H5,
        A4 = 61, B4, C4, D4, E4, F4, G4, H4,
        A3 = 71, B3, C3, D3, E3, F3, G3, H3,
        A2 = 81, B2, C2, D2, E2, F2, G2, H2,
        A1 = 91, B1, C1, D1, E1, F1, G1, H1
    }

    /// <summary>
    /// There are 3 stages of chess game.
    /// </summary>
    public enum GameStage
    {
        Opening,
        MidGame,
        Enidng
    };

    /// <summary>
    /// There are two agents in min-max search algorithm.
    /// </summary>
    public enum Agent
    {
        Maximizer,
        Minimizer
    };
    
    /// <summary>
    /// There are three types of node for TT.
    /// </summary>
    public enum NodeType
    {
        Exact,
        Maximizer,
        Minimizer
    };
}
