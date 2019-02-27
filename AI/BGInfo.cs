using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cheops.AI;

namespace Cheops.AI
{
    public class BGInfo
    {
        // string algorithmName, SearchAlgorithm algorithm, Board board, int depth

        public BestMoveInfo bmi { get; set; }

        public bool IteDeepning { get; set; }

        public string AlgorithmName { get; set; }

        public SearchTree.SearchAlgorithm Algorithm { get; set; }

        public ChessBoardComponent.SmartBoard Board { get; set; }

        public int Depth { get; set; }

        public BGInfo()
        {

        }

        public BGInfo(string algorithmName, SearchTree.SearchAlgorithm algorithm, ChessBoardComponent.SmartBoard board, int depth, bool iteDeppning)
        {
            AlgorithmName = algorithmName;
            Algorithm = algorithm;
            Board = board;
            Depth = depth;
            IteDeepning = iteDeppning;
        }
    }
}
