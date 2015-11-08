using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitsOfPi
{
    /// <summary>
    /// An implementation of the Bailey-Borwein-Plouffe formula for calculating hexadecimal
    /// digits of pi.
    /// https://en.wikipedia.org/wiki/Bailey%E2%80%93Borwein%E2%80%93Plouffe_formula
    /// </summary>
    public static class DigitCalculator
    {
        private const int DigitsPerSum = 8;
        private const double Epsilon = 1e-17;

        /// <summary>
        /// Returns a range of hexadecimal digits of pi.
        /// </summary>
        /// <param name="start">The starting location of the range.</param>
        /// <param name="count">The number of digits to return.</param>
        /// <returns>An array containing the hexadecimal digits.</returns>
        public static byte[] GetDigits(int start, int count)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            byte[] digits = new byte[count];
            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                if(i % DigitCalculator.DigitsPerSum == 0)
                {
                    sum = 4 * DigitCalculator.Sum(1, start)
                        - 2 * DigitCalculator.Sum(4, start)
                        - DigitCalculator.Sum(5, start)
                        - DigitCalculator.Sum(6, start);

                    start += DigitCalculator.DigitsPerSum;
                }

                sum = 16 * (sum - Math.Floor(sum));
                digits[i] = (byte)sum;
            }

            return digits;
        }

        /// <summary>
        /// Returns the sum of 16^(n - k)/(8 * k + m) from 0 to k.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double Sum(int m, int n)
        {
            double sum = 0;
            int d = m;
            int power = n;

            while (true)
            {
                double term;

                if(power > 0)
                    term = (double)DigitCalculator.HexExponentModulo(power, d) / d;
                else
                {
                    term = Math.Pow(16, power) / d;
                    if (term < DigitCalculator.Epsilon)
                        break;
                }

                sum += term;
                power--;
                d += 8;
            }

            return sum;
        }

        /// <summary>
        /// Return 16^p mod m.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private static int HexExponentModulo(int p, int m)
        {
            int power = 1;
            while (power * 2 <= p)
                power *= 2;

            int result = 1;

            while (power > 0)
            {
                if (p >= power)
                {
                    result *= 16;
                    result %= m;
                    p -= power;
                }

                power /= 2;

                if (power > 0)
                {
                    result *= result;
                    result %= m;
                }
            }

            return result;
        }
    }
}
