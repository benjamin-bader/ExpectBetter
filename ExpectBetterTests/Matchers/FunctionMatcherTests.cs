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

        [Test, Throws]
        public void ActionMatcher_ToThrow_WhenNoExceptionThrown_Throws()
        {
            Expect.The(delegate { }).ToThrow<Exception>();
        }

        [Test, Throws]
        public void ActionMatcher_ToThrow_WhenOtherExceptionThrown_Throws()
        {
            Expect.The(delegate { throw new DivideByZeroException(); }).ToThrow<NotImplementedException>();
        }

        [Test, Throws]
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

        [Test, Throws]
        public void FunctionMatcher_ToThrow_WhenNoExceptionThrown_Throws()
        {
            Expect.The(() => 1).ToThrow<Exception>();
        }

        [Test, Throws]
        public void FunctionMatcher_ToThrow_WhenOtherExceptionThrown_Throws()
        {
            Expect.The<int>(delegate { throw new DivideByZeroException(); }).ToThrow<NotImplementedException>();
        }

        [Test, Throws]
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

        [Test]
        public void FunctionMatcher_ToReturn_Equatable_WhenEqual_ReturnsTrue()
        {
            Expect.The(() => "foo").ToReturn((IEquatable<string>) "foo");
        }

        [Test, Throws]
        public void FunctionMatcher_ToReturn_Equatable_WhenNotEqual_Throws()
        {
            Expect.The(() => "foo").ToReturn((IEquatable<string>) "bar");
        }

        [Test]
        public void FunctionMatcher_ToReturn_WithComparer_WhenEqual_ReturnsTrue()
        {
            Expect.The(() => "foo").ToReturn("FOO", StringComparer.OrdinalIgnoreCase);
        }

        [Test, Throws]
        public void FunctionMatcher_ToReturn_WithComparer_WhenNotEqual_Throws()
        {
            Expect.The(() => "foo").ToReturn("FOO", StringComparer.Ordinal);
        }
    }
}
