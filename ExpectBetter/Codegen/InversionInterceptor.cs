using Castle.DynamicProxy;

namespace ExpectBetter.Codegen
{
    internal class InversionInterceptor<TActual, TMatcher> : IInterceptor
        where TMatcher : BaseMatcher<TActual, TMatcher>
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
            var result = (bool)invocation.ReturnValue;
            invocation.ReturnValue = !result;
        }
    }
}
