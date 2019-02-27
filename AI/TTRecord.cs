using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheops.AI
{
    public class TTRecord
    {
        /// <summary>
        /// Określa głębokość na jakiej ruch został ondlaeziony.
        /// </summary>
        public int Depth { get; set; } = -1;

        /// <summary>
        /// Określa wartość punktową jaką funkcja ewaluacyjna nadała pozycji reprezentowanej przez niniejszy rekord.
        /// </summary>
        public double Score { get; set; } = 0;

        /// <summary>
        /// Określa typ wartości jaka jest przechowywana w atrybucie <see cref="Score"/>.
        /// </summary>
        public NodeType NodeType { get; set; }

        /// <summary>
        /// Określa współrzędne pola startowego najlepszego ruchu.
        /// </summary>
        public ExtandCoords? BestMoveStartSquare { get; set; } = null;

        /// <summary>
        /// Określa współrzędne pola końcowego najlepszego ruchu.
        /// </summary>
        public ExtandCoords? BestMoveFinishSquare { get; set; } = null;

        /// <summary>
        /// Klucz funkcji haszującej Zorbista.
        /// </summary>
        public ulong ZorbistKey { get; set; } = 0;

        /// <summary>
        /// Holds a list of performed moves in string format.
        /// </summary>
        public List<string> Path { get; set; } = new List<string>();

    }
}
