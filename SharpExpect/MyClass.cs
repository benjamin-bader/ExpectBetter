using System;

using SharpExpect.Matchers;

namespace SharpExpect
{
	public class Test
	{
		public void Do()
		{
//			var o = new object();

//			Expect.The (o).Not.ToBeNull ();
//			Expect.The ("foo").Not.ToBeNull ();
//			Expect.The ("foo").ToBeLongerThan("uh");
//			Expect.The<string>("foo").ToBeLessThan("zanzibar");
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

	public class Base
	{
		protected int actual;

		public void TestMe()
		{
			Console.WriteLine("Base");
		}
	}

	public class Derived : Base
	{
		public Derived (int a)
		{
			actual = a; 
		}

		new public void TestMe()
		{
			Console.WriteLine("Derived");
			base.TestMe();
		}
	}

	public class Driver
	{
		public static void Main(string[] args)
		{
			//var wrapped = ClassWrapper.Wrap<string, StringMatcher>("foo");

#if DEBUG
			// ClassWrapper.SaveAssembly();
#endif
//			wrapped.ToBeLongerThan("oo");
//			wrapped.ToContain("oo");
//			wrapped.Not.ToContain("bar");
//
//			Expect.The(() =>
//			{
//				throw new DivideByZeroException();
//				return 1;
//			}).ToThrow<DivideByZeroException>();

			try
			{
				Expect.The(() => { throw new InvalidOperationException(); }).ToThrow<InvalidCastException>();
			}
			catch (Exception ex)
			{
				Console.WriteLine("BOOM!  {0}", ex);
#if DEBUG
				ClassWrapper.SaveAssembly();
#endif
			}

			Base derived = new Derived(0);
			derived.TestMe();

			Console.WriteLine ("hello");
			Console.ReadKey();
		}
	}
}

