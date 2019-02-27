using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.AI
{
    /// <summary>
    /// Represent information gain by game tree search algorithm.
    /// </summary>
    public class BestMoveInfo
    {
        /// <summary>
        /// Holds a starting coords of best move choosen by search algorithm.
        /// </summary>
        public Coords StartSquare { get; set; }

        /// <summary>
        /// Holds a finish coords of best move choosen by search algorithm.
        /// </summary>
        public Coords FinishSquare { get; set; }

        /// <summary>
        /// Holds a time needed to find this move.
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// Holds a number of positions which needed evaluation in order to find this move.
        /// </summary>
        public int NumberOfPositionEvaluated { get; set; }

        /// <summary>
        /// Holds a number of alfa-beta cut-offs performed while finding this move.
        /// </summary>
        public int NumberOfAlfaBetaCutOffs { get; set; }

        /// <summary>
        /// Holds a number which represents the depth of performed search.
        /// </summary>
        public int SearchDepth { get; set; }

        /// <summary>
        /// Holds a sign of piece which will be choosen in case of promoting move.
        /// </summary>
        public char PromotionSign { get; set; }

        /// <summary>
        /// Holds a number of positions needed to be analysed in case of min-max algorithm full search.
        /// </summary>
        public ulong NumberOfEstimateMinMaxPositionCount { get; set; }

        /// <summary>
        /// Holds a number of cut off positions.
        /// </summary>
        public int NumberOfEvaluationReduction { get; set; }

        /// <summary>
        /// Holds a number of positions evaluated per secound.
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// Holds a name of algorithm which choosen this move.
        /// </summary>
        public string AlgorithmName { get; set; }

        /// <summary>
        /// Holds a percentage value of positions which was cut-off.
        /// </summary>
        public double SavedPercentage { get; set; }

        /// <summary>
        /// Holds a path to position choosen as best by algorithm search.
        /// </summary>
        public string Path { get; set; }
    }
}
