using System;
using System.Linq;
using System.Reflection;

using Castle.DynamicProxy;

namespace ExpectBetter.Codegen
{
    internal class MatcherInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var allowNullActuals = method.GetCustomAttributes(typeof (AllowNullActualAttribute), true).Length > 0;

            if (!allowNullActuals)
            {
                return interceptors;
            }

            return interceptors.Where(i => !(i is NullActualInterceptorBase)).ToArray();
        }
    }
}
