using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.AI
{
    public class HistoryMove
    {
        public ExtandCoords? StartSquare { get; set; } = null;

        public ExtandCoords? FinishSquare { get; set; } = null;

        public int Value { get; set; } = 0;

    }
}
