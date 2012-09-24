﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using ExpectBetter;

namespace ExpectBetterTests
{
    [TestFixture]
    public class NumericMatcherTests
    {
        [Test]
        public void Byte_ToBeGreaterThan_WhenGreater_ReturnsTrue()
        {
            Expect.The((byte)1).ToBeGreaterThan(0);
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void Byte_ToBeGreaterThan_WhenNotGreater_Throws()
        {
            Expect.The((byte)0).ToBeGreaterThan(0);
        }

        [Test]
        public void Byte_ToBeLessThan_WhenLess_ReturnsTrue()
        {
            Expect.The((byte)0).ToBeLessThan(1);
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void Byte_ToBeLessThan_WhenNotLessThan_Throws()
        {
            Expect.The((byte)0).ToBeLessThan(0);
        }

        [Test]
        public void SByte_ToBeGreaterThan_WhenGreater_ReturnsTrue()
        {
            Expect.The((sbyte)1).ToBeGreaterThan(-1);
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void SByte_ToBeGreaterThan_WhenNotGreater_Throws()
        {
            Expect.The((sbyte)0).ToBeGreaterThan(0);
        }

        [Test]
        public void SByte_ToBeLessThan_WhenLess_ReturnsTrue()
        {
            Expect.The(sbyte.MinValue).ToBeLessThan(0);
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void SByte_ToBeLessThan_WhenNotLessThan_Throws()
        {
            Expect.The((sbyte)120).ToBeLessThan(6);
        }
    }
}