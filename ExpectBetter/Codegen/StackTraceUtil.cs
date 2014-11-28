using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExpectBetter.Codegen
{
    internal static class StackTraceUtil
    {
        #if NET_40
        private static readonly FieldInfo fiStackTrace = typeof(Exception).GetField("_stackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);
#endif

        internal static void FixStackTrace(Exception ex, int framesToSkip = 1)
        {
#if NET_40
            // TODO(ben): Find a cross-platform way to do this.  Per-platform projects should do it.
            //var trace = new StackTrace(ex, framesToSkip, true);
            //fiStackTrace.SetValue(ex, trace.ToString());
#elif NET_20

#elif MONO

#endif
        }
    }
}
