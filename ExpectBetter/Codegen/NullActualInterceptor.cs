using System;

using Castle.DynamicProxy;

namespace ExpectBetter.Codegen
{
    /// <summary>
    /// Implements checking for unexpected null actuals.
    /// </summary>
    /// <typeparam name="TActual"></typeparam>
    /// <typeparam name="TMatcher"></typeparam>
    internal class NullActualInterceptor<TActual, TMatcher> : NullActualInterceptorBase, IInterceptor
        where TMatcher : BaseMatcher<TActual, TMatcher>
    {
        public void Intercept(IInvocation invocation)
        {
            var matcher = (TMatcher) invocation.Proxy;
            
            if (ReferenceEquals(matcher.actual, null))
            {
                try
                {
                    Errors.IllegalNullActual();
                }
                catch (Exception ex)
                {
                    StackTraceUtil.FixStackTrace(ex);
                    throw;
                }
            }

            invocation.Proceed();
        }
    }

    /// <summary>
    /// A convenience class for type testing at runtime.
    /// </summary>
    public class NullActualInterceptorBase
    {
        
    }
}
