using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace codigoControlMSDOS
{
    /// <summary>
    /// For more information cf. http://en.wikipedia.org/wiki/Verhoeff_algorithm
    /// Dihedral Group stuff: http://en.wikipedia.org/wiki/Dihedral_group
    /// Dihedral Group order 10: http://mathworld.wolfram.com/DihedralGroupD5.html
    /// </summary>
    public static class Verhoeff
    {

        // The multiplication table
        static int[,] d = new int[,]
        {
            {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}, 
            {1, 2, 3, 4, 0, 6, 7, 8, 9, 5}, 
            {2, 3, 4, 0, 1, 7, 8, 9, 5, 6}, 
            {3, 4, 0, 1, 2, 8, 9, 5, 6, 7}, 
            {4, 0, 1, 2, 3, 9, 5, 6, 7, 8}, 
            {5, 9, 8, 7, 6, 0, 4, 3, 2, 1}, 
            {6, 5, 9, 8, 7, 1, 0, 4, 3, 2}, 
            {7, 6, 5, 9, 8, 2, 1, 0, 4, 3}, 
            {8, 7, 6, 5, 9, 3, 2, 1, 0, 4}, 
            {9, 8, 7, 6, 5, 4, 3, 2, 1, 0}
        };

        // The permutation table
        static int[,] p = new int[,]
        {
            {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}, 
            {1, 5, 7, 6, 2, 8, 3, 0, 9, 4}, 
            {5, 8, 0, 3, 7, 9, 6, 1, 4, 2}, 
            {8, 9, 1, 6, 0, 4, 3, 5, 2, 7}, 
            {9, 4, 5, 3, 1, 2, 6, 8, 7, 0}, 
            {4, 2, 8, 6, 5, 7, 3, 9, 0, 1}, 
            {2, 7, 9, 3, 8, 0, 6, 4, 1, 5}, 
            {7, 0, 4, 6, 9, 1, 3, 2, 5, 8}
        };

        // The inverse table
        static int[] inv = { 0, 4, 3, 2, 1, 5, 6, 7, 8, 9 };


        /// <summary>
        /// Validates that an entered number is Verhoeff compliant.
        /// NB: Make sure the check digit is the last one!
        /// </summary>
        /// <param name="num"></param>
        /// <returns>True if Verhoeff compliant, otherwise false</returns>
        public static bool validateVerhoeff(string num)
        {
            int c = 0;
            int[] myArray = StringToReversedIntArray(num);

            for (int i = 0; i < myArray.Length; i++)
            {
                c = d[c, p[(i % 8), myArray[i]]];
            }

            return c == 0;

        }

        /// <summary>
        /// For a given number generates a Verhoeff digit
        /// Append this check digit to num
        /// </summary>
        /// <param name="num"></param>
        /// <returns>Verhoeff check digit as string</returns>
        public static string generateVerhoeff(string num)
        {
            int c = 0;
            int[] myArray = StringToReversedIntArray(num);

            for (int i = 0; i < myArray.Length; i++)
            {
                c = d[c, p[((i + 1) % 8), myArray[i]]];
            }

            return inv[c].ToString();
        }

        /// <summary>
        /// Calculate a number of digits of Verhoeff 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="dig"></param>
        /// <returns>Verhoeff check digits as string</returns>
        public static string generateVerhoeff(string num, int dig)
        {
            return generateVerhoeff(num, dig, false);
        }
        /// <summary>
        /// Calculate a number of digits of Verhoeff 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="dig"></param>
        /// <param name="odig"></param>
        /// <returns>Verhoeff check digits as string</returns>
        public static string generateVerhoeff(string num, int dig, bool odig)
        {
            dig = dig <= 0 ? 1 : dig;
            string acc = num;
            string vdc = "";
            for (int i = 0; i < dig; i++)
            {
                string vdu = Verhoeff.generateVerhoeff(acc);
                vdc += vdu;
                acc += vdu;
            }

            return odig ? vdc : acc;
        }


        /// <summary>
        /// Converts a string to a reversed integer array.
        /// </summary>
        /// <param name="num"></param>
        /// <returns>Reversed integer array</returns>
        private static int[] StringToReversedIntArray(string num)
        {
            int[] myArray = new int[num.Length];

            for (int i = 0; i < num.Length; i++)
            {
                myArray[i] = int.Parse(num.Substring(i, 1));
            }

            Array.Reverse(myArray);

            return myArray;

        }
    }
}
