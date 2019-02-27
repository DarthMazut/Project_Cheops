using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.AI
{
    public class MinMaxInfo
    {
        public double Value;

        public ExtandCoords? BestStart;

        public ExtandCoords? BestFinish;

        public MinMaxInfo(double value, ExtandCoords? start, ExtandCoords? finish)
        {
            Value = value;
            BestStart = start;
            BestFinish = finish;
        }
    }
}
