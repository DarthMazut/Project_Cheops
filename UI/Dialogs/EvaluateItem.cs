using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Cheops.UI.Dialogs
{
    /// <summary>
    /// This datatype is used for advanced evaluation <see cref="DataGrid"/> component.
    /// </summary>
    class EvaluateItem
    {
        /// <summary>
        /// Side for which the feature is measured.
        /// </summary>
        public Side? Side { get; set; }

        /// <summary>
        /// Name of measure feature of chess position.
        /// </summary>
        public string Feature { get; set; }

        /// <summary>
        /// A value of measured feature. (In case of a bool values: true-1; false-0)
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// A weight of feature importance measured in pawn value.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// A value recived as a result of <see cref="Value"/> nad <see cref="Weight"/> multiplication.
        /// </summary>
        public double FinalValue { get; }

        /// <summary>
        /// Default constructor for <see cref="EvaluateItem"/> class.
        /// </summary>
        /// <param name="side">Side for which the feature is measured.</param>
        /// <param name="feature">Name of measure feature of chess position.</param>
        /// <param name="value">A value of measured feature. (In case of a bool values: true-1; false-0)</param>
        /// <param name="weight">A weight of feature importance measured in pawn value.</param>
        public EvaluateItem(Side? side, string feature, double value, double weight)
        {
            Side = side;
            Feature = feature;
            Value = value;
            Weight = weight;
            FinalValue = Value * Weight;
        }

    }
}
