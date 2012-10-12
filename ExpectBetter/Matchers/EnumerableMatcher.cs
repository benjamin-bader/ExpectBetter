using System.Collections.Generic;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes test methods on objects implementing
    /// <see cref="IEnumerable&lt;T&gt;"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of element contained in the enumerable being tested.
    /// </typeparam>
    public class EnumerableMatcher<T> : BaseEnumerableMatcher<IEnumerable<T>, T, EnumerableMatcher<T>>
    {
    }
}
