using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cheops.AI;

namespace Cheops
{
    /// <summary>
    /// Contains handy methods including FEN-string validation and array convertions.
    /// </summary>
    public static class Helper
    {
        static char[] FENpieces = { 'P', 'p', 'N', 'n', 'B', 'b', 'R', 'r', 'Q', 'q', 'K', 'k' };

        public static MainWindow MainWindowHandle;

        /// <summary>
        /// Checks whether given FEN-string is valid. Returns "true" if it really is.
        /// </summary>
        /// <param name="FENstring">FEN-string to validate.</param>
        /// <param name="message">Outcome message.</param>
        public static bool IsFenValid(string FENstring, out string message)
        {
            string[] FENwords = FENstring.Split(' ');

            if (FENwords.Length != 6)
            {
                message = $"Niewlasciwy format ({FENwords.Length} slow; prawidlowa wartosc to 6).";
                return false;
            }

            if (!CheckFENFirstWord(FENwords[0], out message))
                return false;

            if (!CheckFENSecondWord(FENwords[1], out message))
                return false;

            if (!CheckFENThirdWord(FENwords[2], out message))
                return false;

            if (!CheckFENFourthWord(FENwords[3], out message))
                return false;

            if (!CheckFENFifthWord(FENwords[4], out message))
                return false;

            if (!CheckFENSixthWord(FENwords[5], out message))
                return false;

            message = "Sukces!";
            return true;


        }

        /// <summary>
        /// Returns char array of 120 elements based on 64 elements array. 
        /// </summary>
        /// <param name="board">Base 64 element array</param>
        public static char[] Array64To120(char[] baseArray)
        {
            char[] tablica = new char[120];

            for (int i = 0; i < 120; i++)
            {
                tablica[i] = '+';
            }

            int index = 21;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tablica[index] = baseArray[i * 8 + j];
                    if (j == 7) index += 3;
                    else index++;
                }
            }

            return tablica;
        }

        /// <summary>
        /// Returns char array of 64 elements based on 120 elements array.
        /// </summary>
        /// <param name="baseArray">Base 120 elements array.</param>
        public static char[] Array120To64(char[] baseArray)
        {
            char[] tablica = new char[64];

            int index = 21;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tablica[i * 8 + j] = baseArray[index];
                    if (j == 7) index += 3;
                    else index++;
                }
            }
            return tablica;
        }

        /// <summary>
        /// Return <see cref="Coords"/> value based on given <see cref="ExtandCoords"/> value.
        /// </summary>
        /// <param name="extandCoords">Coordinates for 120 char array.</param>
        public static Coords ExtandCoordsToCoords(ExtandCoords extandCoords)
        {
            if ((int)extandCoords <= 28)
                return (Coords)extandCoords - 21;
            if ((int)extandCoords <= 38)
                return (Coords)extandCoords - 23;
            if ((int)extandCoords <= 48)
                return (Coords)extandCoords - 25;
            if ((int)extandCoords <= 58)
                return (Coords)extandCoords - 27;
            if ((int)extandCoords <= 68)
                return (Coords)extandCoords - 29;
            if ((int)extandCoords <= 78)
                return (Coords)extandCoords - 31;
            if ((int)extandCoords <= 88)
                return (Coords)extandCoords - 33;
            else
                return (Coords)extandCoords - 35;
        }

        /// <summary>
        /// Returns <see cref="ExtandCoords"/> value based on given <see cref="Coords"/> value.
        /// </summary>
        /// <param name="coords">Coordinates for standard 64 board square.</param>
        public static ExtandCoords CoordsToExtandCoords(Coords coords)
        {
            if ((int)coords <= 7)
                return (ExtandCoords)coords + 21;
            if ((int)coords <= 15)
                return (ExtandCoords)coords + 23;
            if ((int)coords <= 23)
                return (ExtandCoords)coords + 25;
            if ((int)coords <= 31)
                return (ExtandCoords)coords + 27;
            if ((int)coords <= 39)
                return (ExtandCoords)coords + 29;
            if ((int)coords <= 47)
                return (ExtandCoords)coords + 31;
            if ((int)coords <= 55)
                return (ExtandCoords)coords + 33;
            else
                return (ExtandCoords)coords + 35;
        }

        /// <summary>
        /// Returns <see cref="Coords"/> value based on given number (21-98)
        /// </summary>
        /// <param name="number">Number from 21 to 98</param>
        public static Coords IntToCoords(int number)
        {
            if (number <= 28)
                return (Coords)number - 21;
            if (number <= 38)
                return (Coords)number - 23;
            if (number <= 48)
                return (Coords)number - 25;
            if (number <= 58)
                return (Coords)number - 27;
            if (number <= 68)
                return (Coords)number - 29;
            if (number <= 78)
                return (Coords)number - 31;
            if (number <= 88)
                return (Coords)number - 33;
            else
                return (Coords)number - 35;
        }

        /// <summary>
        /// Returns number from (21-98) corresponding to given coords value.
        /// </summary>
        ///<param name="coords">Coordinates for standard 64 board square.</param>
        public static int CoordsToInt(Coords coords)
        {
            if ((int)coords <= 7)
                return (int)coords + 21;
            if ((int)coords <= 15)
                return (int)coords + 23;
            if ((int)coords <= 23)
                return (int)coords + 25;
            if ((int)coords <= 31)
                return (int)coords + 27;
            if ((int)coords <= 39)
                return (int)coords + 29;
            if ((int)coords <= 47)
                return (int)coords + 31;
            if ((int)coords <= 55)
                return (int)coords + 33;
            else
                return (int)coords + 35;
        }

        /// <summary>
        /// Returns a number from 0 to 63 based on a number from 0 - 119.
        /// </summary>
        /// <param name="int120">Number (0-119)</param>
        public static int Int120ToInt64(int int120)
        {
            if (int120 <= 28)
                return int120 - 21;
            if (int120 <= 38)
                return int120 - 23;
            if (int120 <= 48)
                return int120 - 25;
            if (int120 <= 58)
                return int120 - 27;
            if (int120 <= 68)
                return int120 - 29;
            if (int120 <= 78)
                return int120 - 31;
            if (int120 <= 88)
                return int120 - 33;
            else
                return int120 - 35;
        }

        /// <summary>
        /// Returns oposite <see cref="Side"/> to the given one.
        /// </summary>
        /// <param name="side">Side of which you want opposite side.</param>
        public static Side SwitchSide(Side side)
        {
            if (side == Side.White)
            {
                return Side.Black;
            }
            else
                return Side.White;
        }

        static bool CheckFENFirstWord(string firstWord, out string message)
        {
            int calosc = 0;
            int rzad = 0;
            int number = 0;
            int whiteKing = 0;
            int blackKing = 0;

            for (int i = 0; i < firstWord.Length; i++)
            {
                if (rzad > 8 || calosc > 64)
                {
                    message = "Przekroczono zakres rzedu lub 64 pol.";
                    return false;
                }
                else if (FENpieces.Contains(firstWord[i]))
                {
                    rzad++;
                    calosc++;
                    if (firstWord[i] == 'K') whiteKing++;
                    if (firstWord[i] == 'k') blackKing++;

                }
                else if (int.TryParse(firstWord[i].ToString(), out number))
                {
                    rzad += number;
                    calosc += number;
                }
                else if (firstWord[i] == '/')
                {
                    if (rzad == 8)
                        rzad = 0;
                    else
                    {
                        message = $"Niewlasciwa dlugosc rzedu!";
                        return false;
                    }

                }
                else
                {
                    message = $"Nieprawidlowy symbol! [{firstWord[i]}]";
                    return false;
                }

            }

            if (calosc != 64)
            {
                message = $"Uwzgledniono {calosc} pol! Prawidlowa liczba pol to 64.";
                return false;
            }

            if (whiteKing != 1 || blackKing != 1)
            {
                message = "Conajmniej jedna ze stron nie posiada dokladnie jednego krola!";
                return false;
            }

            message = "Sukces!";
            return true;
        }

        static bool CheckFENSecondWord(string secondWord, out string message)
        {
            if (secondWord != "b" && secondWord != "w")
            {
                message = "Stron na posunieciu jest niewlasciwie okreslona";
                return false;
            }
            message = "Sukces!";
            return true;

        }

        static bool CheckFENThirdWord(string thirdWord, out string message)
        {
            if (thirdWord.Length > 4)
            {
                message = "Niewlasciwy format pozwolenia na roszady.";
                return false;
            }

            if (thirdWord != "-")
            {
                foreach (char znak in thirdWord)
                {
                    if (znak != 'K' && znak != 'k' && znak != 'Q' && znak != 'q')
                    {
                        message = "Niewlasciwy znak w pozwoleniu na roszady.";
                        return false;
                    }

                }
            }
            message = "Sukces!";
            return true;
        }

        static bool CheckFENFourthWord(string fourthWord, out string message)
        {
            if (fourthWord != "-")
            {
                for (int i = 0; i < 64; i++)
                {
                    if (((Coords)i).ToString() == fourthWord.ToUpper())
                        break;
                    else if (i == 63)
                    {
                        message = $"{fourthWord} nie jest wlasciwym polem na szachownicy.";
                        return false;
                    }

                }
            }
            message = "Sukces!";
            return true;
        }

        static bool CheckFENFifthWord(string fifthWord, out string message)
        {
            int n;
            if (!int.TryParse(fifthWord, out n))
            {
                message = "Ilosc posuniec bez bicia ani ruchow pionem nie jest prawidlowa liczba!";
                return false;
            }
            message = "Sukces!";
            return true;
        }

        static bool CheckFENSixthWord(string sixthWord, out string message)
        {
            int n;
            if (!int.TryParse(sixthWord, out n))
            {
                message = "Ilosc ruchow nie jest prawidlowa liczba!";
                return false;
            }
            message = "Sukces!";
            return true;
        }


        //============================

        
    }
}
