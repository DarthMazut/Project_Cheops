using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.AI
{
    /// <summary>
    /// Represents a killer-move.
    /// </summary>
    public class KillerMove
    {
        public ExtandCoords? StartSquare { get; }

        public ExtandCoords? FinishSquare { get; }

        public char PromotionSign { get; }

        public KillerMove(ExtandCoords? startSquare, ExtandCoords? finishSquare, char promSign)
        {
            StartSquare = startSquare;
            FinishSquare = finishSquare;
            PromotionSign = promSign;
        }
    }
}
