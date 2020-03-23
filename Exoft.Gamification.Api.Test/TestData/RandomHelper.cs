using System;
using System.Text;

namespace Exoft.Gamification.Api.Test.TestData
{
    public static class RandomHelper
    {
        /// <summary>
        /// Returns random string.
        /// </summary>
        /// <param name="length">Length of the string.</param>
        /// <returns></returns>
        public static string GetRandomString(int length = 20)
        {
            var str_build = new StringBuilder();
            var random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }

            return str_build.ToString();
        }

        /// <summary>
        /// Returns random email.
        /// </summary>
        /// <param name="length">Length of the string.</param>
        /// <returns></returns>
        public static string GetRandomEmail(int length)
        {
            return $"{GetRandomString(length)}@gmail.com";
        }

        /// <summary>
        /// Returns random number from 0 to 100.
        /// </summary>
        /// <returns></returns>
        public static int GetRandomNumber(int maxNumber = 100)
        {
            Random random = new Random();
            return random.Next(0, maxNumber);
        }
    }
}
