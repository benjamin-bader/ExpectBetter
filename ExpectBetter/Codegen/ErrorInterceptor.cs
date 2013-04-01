using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

using Castle.DynamicProxy;

namespace ExpectBetter.Codegen
{
    internal class ErrorInterceptor<TActual, TMatcher> : IInterceptor
        where TMatcher : BaseMatcher<TActual, TMatcher>
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            var matcher = (TMatcher) invocation.InvocationTarget;
            var success = (bool) invocation.ReturnValue;

            if (success)
            {
                return;
            }

            try
            {
                Errors.BadMatch(
                    matcher.actualDescription,
                    matcher.expectedDescription,
                    matcher.inverted,
                    matcher.actual,
                    invocation.Method.Name,
                    invocation.Arguments);
            }
            catch (Exception ex)
            {
                StackTraceUtil.FixStackTrace(ex);
                throw;
            }
        }
    }
}
