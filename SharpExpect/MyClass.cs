using System;

using SharpExpect.Matchers;

namespace SharpExpect
{
	public class Test
	{
		public void Do()
		{
			var o = new object();

			Expect.The (o).Not.ToBeNull ();
			Expect.The ("foo").Not.ToBeNull ();
			Expect.The ("foo").ToBeLongerThan("uh");
			Expect.The<string>("foo").ToBeLessThan("zanzibar");
		}
	}

	public class ABC : StringMatcher
	{
		public override bool ToBeLongerThan (string e)
		{
			return base.ToBeLongerThan(e);
		}

		public override bool ToContain (string e, StringComparison c = StringComparison.OrdinalIgnoreCase)
		{
			var args = new object[2];
			args[0] = (object)e;
			args[1] = (object)c;

			return base.ToContain(e, c);
		}
	}

	public class Driver
	{
		private static void Demo<TExn>(Action action)
			where TExn : Exception
		{
			try
			{
				action();
				Console.WriteLine("No exception here!");
			}
			catch (TExn)
			{
				Console.WriteLine("Caught a specific exception");
			}
			catch (Exception)
			{
				Console.WriteLine("Caught a general exception");
			}
		}

		public static void Main(string[] args)
		{
			Demo<InvalidOperationException>(() => { });
			Demo<InvalidOperationException>(() => { throw new InvalidOperationException(); });
			Demo<InvalidOperationException>(() => { throw new DivideByZeroException(); });

			var wrapped = ClassWrapper.Wrap<string, StringMatcher>("foo");

#if DEBUG
			// ClassWrapper.SaveAssembly();
#endif
			wrapped.ToBeLongerThan("oo");
			wrapped.ToContain ("oo");
			wrapped.Not.ToContain("bar");

			Expect.The(() =>
			           {
							throw new DivideByZeroException();
							return 1;
						}).ToThrow<InvalidOperationException>();

			Console.WriteLine ("hello");
			Console.ReadKey();
		}
	}
}

