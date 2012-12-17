using System;
using System.Collections.Generic;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes test methods on delegates of type <see cref="Func&lt;T&gt;"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The result type of the function being tested.
    /// </typeparam>
    public class FunctionMatcher<T> : BaseMatcher<Func<T>, FunctionMatcher<T>>
    {
        /// <summary>
        /// Expect the function to throw an exception deriving from
        /// <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">
        /// The type of the exception expected.
        /// </typeparam>
        /// <returns>
        /// Returns <see langword="true"/> if the expected exception is thrown,
        /// and <see langword="false"/> otherwise.
        /// </returns>
        public virtual bool ToThrow<TException>()
            where TException : Exception
        {
            expectedDescription = "an exception of type " + typeof(TException).FullName;

            try
            {
                actual();
                actualDescription = "a function that completed successfully";
            }
            catch (TException)
            {
                return true;
            }
            catch (Exception ex)
            {
                actualDescription = "a function that threw an exception of type " + ex.GetType().FullName;
            }

            return false;
        }

        /// <summary>
        /// Expect the function to return a value considered equal to the given
        /// <see cref="IEquatable&lt;T&gt;"/>.
        /// </summary>
        public virtual bool ToReturn(IEquatable<T> expected)
        {
            return ToReturnInternal(expected.Equals);
        }

        /// <summary>
        /// Expect the function to return a value considered equal to the
        /// <paramref name="expected"/> value by the given <paramref name="comparer"/>.
        /// </summary>
        public virtual bool ToReturn(T expected, IEqualityComparer<T> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            return ToReturnInternal(value => comparer.Equals(value, expected));
        }

        private bool ToReturnInternal(Predicate<T> predicate)
        {
            try
            {
                var returnValue = actual();
                actualDescription = ReferenceEquals(returnValue, null) ? "null" : returnValue.ToString();
                return predicate(returnValue);
            }
            catch (Exception ex)
            {
                actualDescription = "a thrown exception of type " + ex.GetType().FullName;
                return false;

            }
        }
    }

    /// <summary>
    /// Exposes test methods on delegates of type <see cref="Action"/>.
    /// </summary>
    public class ActionMatcher : BaseMatcher<Action, ActionMatcher>
    {
        /// <summary>
        /// Expect the function to throw an exception deriving from
        /// <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">
        /// The type of the exception expected.
        /// </typeparam>
        /// <returns>
        /// Returns <see langword="true"/> if the expected exception is thrown,
        /// and <see langword="false"/> otherwise.
        /// </returns>
        public virtual bool ToThrow<TException>()
            where TException : Exception
        {
            expectedDescription = "an exception of type " + typeof(TException).FullName;

            try
            {
                actual();
                actualDescription = "an action that completed successfully";
            }
            catch (TException)
            {
                return true;
            }
            catch (Exception ex)
            {
                actualDescription = "an action that threw an exception of type " + ex.GetType().FullName;
            }

            return false;
        }
    }
}
