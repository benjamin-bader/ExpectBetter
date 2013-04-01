using System;
using System.Reflection;

using Castle.DynamicProxy;

namespace ExpectBetter.Codegen
{
    internal class MatcherProxyGenerationHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {
            // No-op
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            // No-op
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return methodInfo.IsPublic
                && methodInfo.ReturnType == typeof (bool)
                && !methodInfo.Name.StartsWith("get_")
                && !methodInfo.Name.StartsWith("set_");
        }
    }
}
