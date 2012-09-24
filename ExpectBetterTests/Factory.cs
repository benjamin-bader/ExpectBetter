using System;
using System.Text;

namespace ExpectBetterTests
{
    public static class Factory
    {
        private const string Alpha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Numeric = "1234567890";
        private const string Alphanumeric = Alpha + Numeric;

        [ThreadStatic]
        private static Random random;

        public static Random Random
        {
            get
            {
                if (random == null)
                    random = new Random();

                return random;
            }
        }

        public static string RandomString(int maxLength = 10, int minLength = 1)
        {
            var length = Random.Next(minLength, maxLength + 1);
            var sb = new StringBuilder(length);

            for (var i = 0; i < length; ++i)
            {
                sb.Append(Alphanumeric[random.Next(Alphanumeric.Length)]);
            }

            return sb.ToString();
        }
    }
}
