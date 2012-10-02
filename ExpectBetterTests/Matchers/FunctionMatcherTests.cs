using System;
using NUnit.Framework;
using ExpectBetter;

namespace ExpectBetterTests.Matchers
{
    [TestFixture]
    public class FunctionMatcherTests
    {
        [Test]
        public void ActionMatcher_ToThrow_CatchesNamedExceptionTypes()
        {
            Expect.The(delegate { throw new DivideByZeroException(); }).ToThrow<DivideByZeroException>();
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
            Expect.The(delegate { throw new Exception(); }).Not.ToThrow<ApplicationException>();
        }

        [Test]
        public void ActionMatcher_Not_ToThrow_WhenNoExceptionThrown_ReturnsTrue()
        {
            Expect.The(delegate { }).Not.ToThrow<Exception>();
        }

        [Test]
        public void FunctionMatcher_ToThrow_CatchesNamedExceptionTypes()
        {
            Expect.The<int>(delegate { throw new DivideByZeroException(); }).ToThrow<DivideByZeroException>();
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void FunctionMatcher_ToThrow_WhenNoExceptionThrown_Throws()
        {
            Expect.The(() => 1).ToThrow<Exception>();
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void FunctionMatcher_ToThrow_WhenOtherExceptionThrown_Throws()
        {
            Expect.The<int>(delegate { throw new DivideByZeroException(); }).ToThrow<NotImplementedException>();
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void FunctionMatcher_Not_ToThrow_WhenNamedExceptionThrown_Throws()
        {
            Expect.The<int>(delegate { throw new DivideByZeroException(); }).Not.ToThrow<DivideByZeroException>();
        }

        [Test]
        public void FunctionMatcher_Not_ToThrow_WhenOtherExceptionThrown_ReturnsTrue()
        {
            Expect.The<int>(delegate { throw new Exception(); }).Not.ToThrow<ApplicationException>();
        }

        [Test]
        public void FunctionMatcher_Not_ToThrow_WhenNoExceptionThrown_ReturnsTrue()
        {
            Expect.The(() => 1).Not.ToThrow<Exception>();
        }
    }
}
