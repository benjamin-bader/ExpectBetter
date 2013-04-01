using Castle.DynamicProxy;
using ExpectBetter.Codegen;

namespace ExpectBetter
{
    /// <summary>
    /// Exposes common configuration methods.
    /// </summary>
    public class Expectations
    {
        private static readonly ProxyGenerator ProxyGenerator;
        private static readonly ProxyGenerationOptions ProxyGenerationOptions;

        static Expectations()
        {
            ProxyGenerator = new ProxyGenerator();
            ProxyGenerationOptions = new ProxyGenerationOptions
            {
                Hook = new MatcherProxyGenerationHook(),
                Selector = new MatcherInterceptorSelector()
            };
        }

        /// <summary>
        /// Wraps a matcher in a proxy that checks assertion results, formats
        /// error messages, and performs other housekeeping.
        /// </summary>
        /// <typeparam name="TActual">
        /// The type of object tested by the wrapped matcher.
        /// </typeparam>
        /// <typeparam name="TMatcher">
        /// The type of matcher to be wrapped.
        /// </typeparam>
        /// <param name="actual">
        /// The actual value being tested.
        /// </param>
        /// <returns>
        /// Returns a wrapped matcher that tests the <paramref name="actual"/>
        /// value given.
        /// </returns>
        public static TMatcher Wrap<TActual, TMatcher>(TActual actual)
            where TMatcher : BaseMatcher<TActual, TMatcher>
        {
            var matcher = (TMatcher) ProxyGenerator.CreateClassProxy(
                typeof (TMatcher),
                ProxyGenerationOptions,
                new NullActualInterceptor<TActual, TMatcher>(),
                new ErrorInterceptor<TActual, TMatcher>());
            
            var inverted = (TMatcher) ProxyGenerator.CreateClassProxy(
                typeof (TMatcher),
                ProxyGenerationOptions,
                new NullActualInterceptor<TActual, TMatcher>(),
                new ErrorInterceptor<TActual, TMatcher>(),
                new InversionInterceptor<TActual, TMatcher>());

            matcher.Not = inverted;
            matcher.actual = actual;

            inverted.Not = matcher;
            inverted.actual = actual;
            inverted.inverted = true;

            matcher.InvokeInitializer();
            inverted.InvokeInitializer();

            return matcher;
        }

    }
}
