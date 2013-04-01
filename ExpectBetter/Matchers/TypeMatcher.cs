using System;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes expectations on <see cref="Type"/> objects.
    /// </summary>
    public class TypeMatcher : BaseObjectMatcher<Type, TypeMatcher>
    {
        /// <summary>
        /// Sets a custom description of the actual type being tested.
        /// </summary>
        protected override void Initialize()
        {
            actualDescription = actual.FullName;
        }

        /// <summary>
        /// Expect the actual type to derive from
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The class or interface type expected to be implemented or inherited
        /// by the actual.
        /// </typeparam>
        public virtual bool ToInheritFrom<T>()
        {
            var expected = typeof (T);
            expectedDescription = expected.FullName;
            return expected.IsAssignableFrom(actual) && actual != expected;
        }

        /// <summary>
        /// Expect the actual type to derive from the
        /// <paramref name="expected"/> type.
        /// </summary>
        /// <param name="expected">
        /// The class or interface type expected to be implemented or inherited
        /// by the actual.
        /// </param>
        public virtual bool ToInheritFrom(Type expected)
        {
            expectedDescription = expected.FullName;
            return expected.IsAssignableFrom(actual) && actual != expected;
        }

        /// <summary>
        /// Expect the type to be generic.
        /// </summary>
        public virtual bool ToBeGeneric()
        {
            return actual.IsGenericType;
        }

        /// <summary>
        /// Expect the type to be open, i.e. to be generic without containing
        /// generic parameters.
        /// </summary>
        /// <returns></returns>
        public virtual bool ToBeAGenericTypeDefinition()
        {
            return actual.IsGenericTypeDefinition;
        }
    }
}
