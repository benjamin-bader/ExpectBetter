using System;
using System.Collections.Generic;
using System.Linq;

using ExpectBetter.Matchers;

namespace ExpectBetter.Matchers
{
	public class BaseEnumerableMatcher<TEnumerable, TItem, M> : BaseObjectMatcher<TEnumerable, M>
		where TEnumerable : IEnumerable<TItem>
		where M : BaseEnumerableMatcher<TEnumerable, TItem, M>
	{
		public virtual bool ToBeEmpty()
		{
			return actual.Count() == 0;
		}

		public virtual bool ToContain(TItem expected)
		{
            var comparer = EqualityComparer<TItem>.Default;

            foreach (var element in actual)
            {
                if (comparer.Equals(element, expected))
                {
                    return true;
                }
            }

			return false;
		}

		public virtual bool ToContain(TItem expected, IEqualityComparer<TItem> comparer)
		{
			if (comparer == null)
			{
				comparer = EqualityComparer<TItem>.Default;
			}

			foreach (var element in actual)
			{
				if (comparer.Equals(element, expected))
				{
					return true;
				}
			}

			return false;
		}

        public virtual bool ToContainInOrder(IEnumerable<TItem> expected)
        {
            return ToContainInOrder(expected, EqualityComparer<TItem>.Default);
        }

		public virtual bool ToContainInOrder(IEnumerable<TItem> expected, IEqualityComparer<TItem> comparer)
		{
			IEnumerator<TItem> actualEnumerator = null, expectedEnumerator = null;

			try
			{
				actualEnumerator = actual.GetEnumerator();
				expectedEnumerator = expected.GetEnumerator();
				var hasMoreExpected = expectedEnumerator.MoveNext();

				if (comparer == null)
				{
					comparer = EqualityComparer<TItem>.Default;
				}
				
				while (hasMoreExpected && actualEnumerator.MoveNext())
				{
					if (comparer.Equals(actualEnumerator.Current, expectedEnumerator.Current))
					{
						hasMoreExpected = expectedEnumerator.MoveNext();
					}
				}

				return !hasMoreExpected;
			}
			finally
			{
				if (actualEnumerator != null)
				{
					actualEnumerator.Dispose();
				}

				if (expectedEnumerator != null)
				{
					expectedEnumerator.Dispose();
				}
			}
		}
	}
}

