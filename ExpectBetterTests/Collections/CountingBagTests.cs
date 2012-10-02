using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using ExpectBetter;
using ExpectBetter.Collections;

namespace ExpectBetterTests.Collections
{
    [TestFixture]
    public class CountingBagTests
    {
        [Test]
        public void Bag_RecordsItemPresence()
        {
            var bag = new CountingBag<string>(new[] { "a", "a", "a", "b" }, StringComparer.Ordinal);

            Expect.The(bag).ToContain("a");
            Expect.The(bag).ToContain("b");
            Expect.The(bag).Not.ToContain("z");
        }

        [Test]
        public void Bag_UsesEqualityComparer()
        {
            var bag = new CountingBag<string>(new[] { "a" }, StringComparer.OrdinalIgnoreCase);
            
            Expect.The(bag.Count).ToEqual(1);
            Expect.The(bag).ToContain("a");
            Expect.The(bag).ToContain("A");
        }

        [Test]
        public void Count_ReportsNumberOfItemsIncludingDuplicates()
        {
            var bag = new CountingBag<string>(new[] { "a", "a" });
            Expect.The(bag.Count).ToEqual(2);
        }

        [Test]
        public void Add_AddsAnItem()
        {
            var bag = new CountingBag<string>(new[] { "a" });
            var originalCount = bag.Count;

            bag.Add("b");

            Expect.The(bag.Count).ToEqual(originalCount + 1);
        }

        [Test]
        public void Add_IncrementsDuplicateCount()
        {
            var bag = new CountingBag<string>(new[] { "zzz", "zzz", "foo" });
            var fooCount = bag.GetCountFor("foo");

            Expect.The(bag.Count).ToEqual(3);
            Expect.The(fooCount).ToEqual(1);

            bag.Add("foo");

            Expect.The(bag.GetCountFor("foo")).ToEqual(fooCount + 1);
        }

        [Test]
        public void Remove_DecrementsDuplicateCount()
        {
            var bag = new CountingBag<int>(new[] { int.MaxValue, int.MaxValue });
            var count = bag.GetCountFor(int.MaxValue);

            bag.Remove(int.MaxValue);
            
            Expect.The(bag.GetCountFor(int.MaxValue)).ToEqual(count - 1);
        }

        [Test]
        public void Remove_WhenItemIsRemoved_ReturnsTrue()
        {
            var bag = new CountingBag<int>(new[] { 0 });
            Expect.The(bag.Remove(0)).ToBeTrue();
        }

        [Test]
        public void Remove_WhenNoItemRemoved_ReturnsFalse()
        {
            var bag = new CountingBag<int>(new[] { 1 });
            Expect.The(bag.Remove(0)).ToBeFalse();
        }

        [Test]
        public void Remove_DecrementsTotalCount()
        {
            var bag = new CountingBag<int>(new[] { 1, 2, 3 });
            var count = bag.Count;

            bag.Remove(2);

            Expect.The(bag.Count).ToEqual(count - 1);
        }
    }
}
