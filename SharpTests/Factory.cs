using System;
using System.Text;

namespace SharpTests
{
	public static class Factory
	{
		[ThreadStatic]
		private static Random random = new Random();

		private const string Alpha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string Numeric = "1234567890";
		private const string Alphanumeric = Alpha + Numeric;

		public static string RandomString(int maxLength = 10, int minLength = 1)
		{
			var length = random.Next(minLength, maxLength + 1);
			var sb = new StringBuilder(length);

			for (var i = 0; i < length; ++i)
			{
				sb.Append(Alphanumeric[random.Next(Alphanumeric.Length)]);
			}

			return sb.ToString();
		}
	}
}

