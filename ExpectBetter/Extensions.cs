using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpectBetter
{
	public static class Extensions
	{
		/// <summary>
		/// Cycle the specified collection.
		/// </summary>
		/// <description>
		/// Creates an infinite sequence of elements from a given collection, repeating
		/// elements while preserving their order.
		/// </description>
		/// <remarks>
		/// <para>
		/// This method attempts to avoid repeated IEnumerable iterations;
		/// when given an array or an IList&lt;T&gt;, a more efficient
		/// generator is used.
		/// Be aware that a bona-fide Enumerable may be enumerated any
		/// numer of times.
		/// </para>
		/// </remarks>
		/// <param name='collection'>
		/// The collection to be cycled.
		/// </param>
		/// <typeparam name='T'>
		/// The type of element contained in the collection.
		/// </typeparam>
		public static IEnumerable<T> Cycle<T>(this IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}

			if (collection is T[])
			{
				var array = (T[]) collection;

				if (array.Length == 0)
				{
					throw new ArgumentException("Can't make a cycle from a zero-length array.");
				}

				var i = 0;
				var len = array.Length;

				while (true)
				{
					yield return array[i];
					i = (i + 1) % len;
				}
			}
			else if (collection is IList<T>)
			{
				var list = (IList<T>) collection;

				if (list.Count == 0)
				{
					throw new ArgumentException("Can't make a cycle from a zero-length list.");
				}

				var i = 0;
				var len = list.Count;

				while (true)
				{
					yield return list[i];
					i = (i + 1) % len;
				}
			}
			else
			{
				while (true)
				{
					foreach (var item in collection)
					{
						yield return item;
					}
				}
			}
		}

		/// <summary>
		/// Partition the specified collection using the given predicate.
		/// </summary>
		/// <param name='collection'>
		/// The collection to be partitioned.
		/// </param>
		/// <param name='predicate'>
		/// The predicate with which to partition.
		/// </param>
		/// <typeparam name='T'>
		/// The type of element contained in the given collection.
		/// </typeparam>
		/// <returns>
		/// A tuple containing two enumerations, one containing elements for
		/// which <paramref name="predicate"/> returned <see langword="true"/>,
		/// the second containing elements for which it returned <see langword="false"/>.
		/// </returns>			
		public static Tuple<IEnumerable<T>, IEnumerable<T>> Partition<T>(this IEnumerable<T> collection, Predicate<T> predicate)
		{
			var trues = new List<T>();
			var falses = new List<T>();

			foreach (var element in collection)
			{
				if (predicate(element))
				{
					trues.Add(element);
				}
				else
				{
					falses.Add(element);
				}
			}

			return Tuple.Create(trues.AsEnumerable(), falses.AsEnumerable());
		}

		/// <summary>
		/// Combines two collections such that the elements of the second are placed
		/// between the elements of the first, preserving order.
		/// </summary>
		/// <remarks>
		/// The second collection is cycled - if it is too short, its elements are repeated.
		/// </remarks>
		/// <param name='collection'>
		/// The collection into which to interject elements.
		/// </param>
		/// <param name='interjection'>
		/// The collection containing elements to interject.
		/// </param>
		/// <typeparam name='T'>
		/// The type of element contained in the resulting collection.
		/// </typeparam>
		public static IEnumerable<T> Interject<T>(this IEnumerable<T> collection, IEnumerable<T> interjection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}

			if (interjection == null)
			{
				throw new ArgumentNullException("other");
			}

			var thisIter = collection.GetEnumerator();
			var thatIter = interjection.Cycle().GetEnumerator();
			var hasYielded = false;

			while (thisIter.MoveNext())
			{
				if (hasYielded)
				{
					thatIter.MoveNext();
					yield return thatIter.Current;
				}

				yield return thisIter.Current;
				hasYielded = true;
			}
		}
	}
}

