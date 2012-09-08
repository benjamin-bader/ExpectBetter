using System;
using NUnit.Framework;
using SharpExpect;

namespace SharpTests
{
	[TestFixture]
	public class FunctionMatcherTests
	{
		[Test]
		public void ActionMatcher_ToThrow_CatchesNamedExceptionTypes()
		{
			var result = Expect.The(delegate { throw new DivideByZeroException(); }).ToThrow<DivideByZeroException>();
			Assert.IsTrue(result);
		}

		[Test, ExpectedException(typeof(ExpectationException))]
		public void ActionMatcher_ToThrow_WhenNoExceptionThrown_Throws()
		{
			Expect.The(delegate { }).ToThrow<Exception>();
		}

		[Test, ExpectedException(typeof(ExpectationException))]
		public void ActionMatcher_ToThrow_WhenOtherExceptionThrown_Throws()
		{
			Expect.The(delegate { throw new DivideByZeroException(); }).ToThrow<NotImplementedException>();
		}

		[Test, ExpectedException(typeof(ExpectationException))]
		public void ActionMatcher_Not_ToThrow_WhenNamedExceptionThrown_Throws()
		{
			Expect.The(delegate { throw new DivideByZeroException(); }).Not.ToThrow<DivideByZeroException>();
		}

		[Test]
		public void ActionMatcher_Not_ToThrow_WhenOtherExceptionThrown_ReturnsTrue()
		{
			var result = Expect.The(delegate { throw new Exception(); }).Not.ToThrow<ApplicationException>();
			Assert.IsTrue(result);
		}

		[Test]
		public void ActionMatcher_Not_ToThrow_WhenNoExceptionThrown_ReturnsTrue()
		{
			var result = Expect.The(delegate { }).Not.ToThrow<Exception>();
			Assert.IsTrue(result);
		}

		[Test]
		public void FunctionMatcher_ToThrow_CatchesNamedExceptionTypes()
		{
			var result = Expect.The(delegate { throw new DivideByZeroException(); return 1; }).ToThrow<DivideByZeroException>();
			Assert.IsTrue(result);
		}
		
		[Test, ExpectedException(typeof(ExpectationException))]
		public void FunctionMatcher_ToThrow_WhenNoExceptionThrown_Throws()
		{
			Expect.The(() => 1).ToThrow<Exception>();
		}
		
		[Test, ExpectedException(typeof(ExpectationException))]
		public void FunctionMatcher_ToThrow_WhenOtherExceptionThrown_Throws()
		{
			Expect.The(delegate { throw new DivideByZeroException(); return 1; }).ToThrow<NotImplementedException>();
		}
		
		[Test, ExpectedException(typeof(ExpectationException))]
		public void FunctionMatcher_Not_ToThrow_WhenNamedExceptionThrown_Throws()
		{
			Expect.The(delegate { throw new DivideByZeroException(); return 1; }).Not.ToThrow<DivideByZeroException>();
		}
		
		[Test]
		public void FunctionMatcher_Not_ToThrow_WhenOtherExceptionThrown_ReturnsTrue()
		{
			var result = Expect.The(delegate { throw new Exception(); return 1; }).Not.ToThrow<ApplicationException>();
			Assert.IsTrue(result);
		}
		
		[Test]
		public void FunctionMatcher_Not_ToThrow_WhenNoExceptionThrown_ReturnsTrue()
		{
			var result = Expect.The(() => 1).Not.ToThrow<Exception>();
			Assert.IsTrue(result);
		}
	}
}

