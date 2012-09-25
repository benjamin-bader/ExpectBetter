using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace ExpectBetter.Codegen
{
    internal static class ClassWrapper
    {
        private const string AssemblyName = "ExpectBetterWrappers";
        private const string AssemblyFileName = AssemblyName + ".dll";
        private const string WrapperPrefix = "WrapperOf$";

        private static Dictionary<Type, Type> WrappedTypes = new Dictionary<Type, Type>();
        
        private static readonly AssemblyName assemblyName;
        internal static readonly AssemblyBuilder assemblyBuilder;
        internal static readonly ModuleBuilder moduleBuilder;

        private static readonly MethodInfo IllegalNullActualInfo = typeof(Errors).GetMethod("IllegalNullActual", BindingFlags.Static | BindingFlags.Public);
        private static readonly MethodInfo BadMatchInfo = typeof(Errors).GetMethod("BadMatch", BindingFlags.Static | BindingFlags.Public);
        private static readonly MethodInfo GiveBugReportInfo = typeof(Errors).GetMethod("GiveBugReport", BindingFlags.Static | BindingFlags.Public);

        static ClassWrapper()
        {
#if DEBUG
            var assemblyBuilderAccess = AssemblyBuilderAccess.RunAndSave;
#else
			var assemblyBuilderAccess = AssemblyBuilderAccess.Run;

#endif
            assemblyName = new AssemblyName(AssemblyName);
            assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, assemblyBuilderAccess);
            moduleBuilder = assemblyBuilder.DefineDynamicModule(AssemblyName, AssemblyFileName);
        }

#if DEBUG
        internal static void SaveAssembly()
        {
            System.IO.File.Delete(AssemblyFileName);
            assemblyBuilder.Save(AssemblyFileName);
        }
#endif

        internal static M Wrap<T, M>(T actual)
            where M : BaseMatcher<T, M>
        {
            var wrapper = RetrieveWrapper<T, M>();

            if (wrapper.ContainsGenericParameters)
            {
                // HACK wtf - need to validate this!
                var parameters = typeof(T).GetGenericArguments();
                wrapper = wrapper.MakeGenericType(parameters);
            }

            return (M)Activator.CreateInstance(wrapper, new object[] { actual });
        }

        /// <summary>
        /// Returns a wrapper factory function for a given matcher type
        /// <typeparamref name="M"/>.
        /// </summary>
        /// <description>
        /// Wrappers are cached; if no cached wrapper exists, one is generated.
        /// </description>
        /// <returns>
        /// A <see cref="Func&lt;Object, Object&gt;"/> which, when invoked,
        /// creates an instance of the wrapped matcher with the given actual
        /// value.
        /// </returns>
        /// <typeparam name='T'>
        /// The type of the 'actual' value tested by matcher <typeparamref name="M"/>.
        /// </typeparam>
        /// <typeparam name='M'>
        /// The type of the matcher to be wrapped.  Must derive from
        /// <see cref="BaseMatcher"/>.
        /// </typeparam>
        private static Type RetrieveWrapper<T, M>()
            where M : BaseMatcher<T, M>
        {
            var @base = GenericDefinitionOf<M>();
            Type result = null;

            if (@base.IsGenericType && !@base.IsGenericTypeDefinition)
            {
                @base = @base.GetGenericTypeDefinition();
            }

            lock (WrappedTypes)
            {
                if (WrappedTypes.TryGetValue(@base, out result))
                {
                    return result;
                }
            }

            var completedNewType = GenerateWrapper<T>(@base);
            result = completedNewType;

            lock (WrappedTypes)
            {
                Type typ;
                if (WrappedTypes.TryGetValue(@base, out typ))
                {
                    result = typ;
                }
                else
                {
                    WrappedTypes[@base] = result;
                }
            }

            //assemblyBuilder.Save(AssemblyFileName);

            return result;
        }

        private static Type GenerateWrapper<T>(Type @base)
        {
            var builder = moduleBuilder.DefineType(
                WrapperPrefix + @base.Name,
                TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Public,
                @base);

            if (@base.IsGenericTypeDefinition)
            {
                var genericArgs = @base.GetGenericArguments();
                var parameterBuilders = builder.DefineGenericParameters(Array.ConvertAll(genericArgs, arg => arg.Name));

                DefineGenericArguments(genericArgs, parameterBuilders);
            }

            var actual = builder.BaseType.GetField("actual", BindingFlags.Instance | BindingFlags.NonPublic);
            var inverted = builder.BaseType.GetField("inverted", BindingFlags.Instance | BindingFlags.NonPublic);
            var privateCtor = EmitPrivateConstructor<T>(builder, actual, inverted);

            EmitPublicConstructor<T>(builder, privateCtor);

            foreach (var mi in @base.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                // Skip constructors and System.Object methods - anything else is fair game.
                if (mi.IsSpecialName || typeof(object) == mi.DeclaringType)
                {
                    continue;
                }

                if (typeof(bool) != mi.ReturnType)
                {
                    var message = string.Format("Invalid return type '{0}' in method '{1}' - must be System.Boolean.", mi.ReturnType, mi.Name);
                    throw new NotSupportedException(message);
                }

                if (!mi.IsVirtual)
                {
                    throw new NotSupportedException("Non-virtual public test method: " + mi.Name);
                }

                WrapTestMethod<T>(mi, builder, actual, inverted, @base);
            }

            return builder.CreateType();
        }

        private static void WrapTestMethod<T>(MethodInfo mi, TypeBuilder builder, FieldInfo actual, FieldInfo inverted, Type @base)
        {
            // The strategy here is to override each test method such that it
            // handles nullness properly and blows up when the test method fails.
            //
            // for each public method not declared in System.Object
            //   ensure it is virtual
            //   ensure it returns a bool
            //   match generic parameters & constraints, if present in base
            //   if base is annotated with [AllowNullActual] and actual is not a value type, emit a null check.
            //   call the base with the given arguments
            //   if (retVal ^ inverted) call ClassWrapper.BadMatch
            //   return retVal

            var parameters = mi.GetParameters();
            var wrapper = builder.DefineMethod(
                mi.Name,
                MethodAttributes.Public | MethodAttributes.Virtual,
                typeof(bool),
                Array.ConvertAll(parameters, p => p.ParameterType));
            
            if (mi.IsGenericMethod)
            {
                var genericArguments = mi.GetGenericArguments();
                var parameterBuilders = wrapper.DefineGenericParameters(genericArguments.Select(arg => arg.Name).ToArray());

                DefineGenericArguments(genericArguments, parameterBuilders);
            }

            var actualCanBeNull = mi.GetCustomAttributes(typeof(AllowNullActualAttribute), true).Length > 0;
            var il = wrapper.GetILGenerator();
            var lblOk = il.DefineLabel();
            var lblErr = il.DefineLabel();

            il.DeclareLocal(typeof(bool));      // loc.0
            il.DeclareLocal(typeof(object[]));  // loc.1

            if (!actualCanBeNull)
            {
                EmitNullActualCheck(il, actual);
            }

            il.BeginExceptionBlock();

            il.Emit(OpCodes.Ldarg_0);

            for (var i = 0; i < parameters.Length; ++i)
            {
                il.EmitLdArg(i + 1);
            }

            il.Emit(OpCodes.Call, mi);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, inverted);
            il.Emit(OpCodes.Xor);
            il.Emit(OpCodes.Brfalse_S, lblErr);

            il.Emit(OpCodes.Leave_S, lblOk);

            // Assert failed - now we throw ALL THE THINGS
            il.MarkLabel(lblErr);

            // First, marshal the params into an object array
            il.EmitLoadNumber(parameters.Length);
            il.Emit(OpCodes.Newarr, typeof(object));
            il.Emit(OpCodes.Stloc_1);

            for (var i = 0; i < parameters.Length; ++i)
            {
                il.Emit(OpCodes.Ldloc_1);
                il.EmitLoadNumber(i);
                il.Emit(OpCodes.Conv_I); // 'stelem' requires a nativeint index
                il.EmitLdArgAsObject(i + 1, parameters[i].ParameterType);
                il.Emit(OpCodes.Stelem, typeof(object));
            }

            // Next, push relevant args for BadMatch

            // Description of actual
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, @base.GetField("actualDescription", BindingFlags.Instance | BindingFlags.NonPublic));

            // Description of expected
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, @base.GetField("expectedDescription", BindingFlags.Instance | BindingFlags.NonPublic));

            // Inverted
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, inverted);

            // Actual
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, actual);
            il.Emit(OpCodes.Ldobj, actual.FieldType);
            il.Emit(OpCodes.Box, actual.FieldType);

            // Method name
            il.Emit(OpCodes.Ldstr, mi.Name);

            // Expected arg array
            il.Emit(OpCodes.Ldloc_1);

            // Call BadMatch (which throws)
            il.Emit(OpCodes.Call, BadMatchInfo);

            // catch InvalidProgramExceptions - these are bugs generated somewhere in this file.
            il.BeginCatchBlock(typeof(InvalidProgramException));
            il.Emit(OpCodes.Call, GiveBugReportInfo);

            // ditto for BadImageFormatExceptions
            il.BeginCatchBlock(typeof(BadImageFormatException));
            il.Emit(OpCodes.Call, GiveBugReportInfo);

            il.EndExceptionBlock();

            il.MarkLabel(lblOk);
            il.Emit(OpCodes.Ldc_I4_1); // 1 == true in IL
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Emits instructions verifying that the value stored in the given
        /// field <paramref name="actual"/> is not <see langword="null"/>.
        /// Emits nothing if the type of the given field is a value type.
        /// </summary>
        /// <param name='il'>
        /// The <see cref="ILGenerator"/> to emit the null check.
        /// </param>
        /// <param name='actual'>
        /// The <see cref="FieldInfo"/> whose value is to be verified.
        /// </param>
        private static void EmitNullActualCheck(ILGenerator il, FieldInfo actual)
        {
            // Value types can't be null, so bail if actual is one.
            if (actual.FieldType.IsValueType)
            {
                return;
            }

            var lblOk = il.DefineLabel();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, actual);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Brfalse, lblOk);

            il.Emit(OpCodes.Call, IllegalNullActualInfo);

            il.MarkLabel(lblOk);
        }

        /// <summary>
        /// Emits the private constructor, which does the work of setting up
        /// the actual, inverted, and Not fields of BaseMatcher.
        /// </summary>
        /// <description>
        /// In C#, the constructor would look like this:
        /// <code>
        /// private TypeName(T actual, bool inverted)
        ///     : base()
        /// {
        ///     this.actual = actual;
        ///     this.inverted = inverted;
        /// 
        ///     if (!inverted)
        ///     {
        ///         this.Not = new TypeName(actual, true);
        ///     }
        /// }
        /// </code>
        /// </description>
        /// <returns>
        /// The private constructor.
        /// </returns>
        /// <param name='typeBuilder'>
        /// Type builder.
        /// </param>
        /// <param name='actual'>
        /// Actual.
        /// </param>
        /// <param name='inverted'>
        /// Inverted.
        /// </param>
        /// <typeparam name='T'>
        /// The 1st type parameter.
        /// </typeparam>
        private static ConstructorBuilder EmitPrivateConstructor<T>(TypeBuilder typeBuilder, FieldInfo actual, FieldInfo inverted)
        {
            var typeOfActual = GenericDefinitionOf<T>();

            var cb = typeBuilder.DefineConstructor(
                MethodAttributes.Private | MethodAttributes.SpecialName,
                CallingConventions.HasThis,
                new[] { typeOfActual, typeof(bool) });

            var il = cb.GetILGenerator();
            var lblReturn = il.DefineLabel();
            var fiNot = typeBuilder.BaseType.GetField("Not", BindingFlags.Instance | BindingFlags.Public);
            var constructorInfo = typeBuilder.BaseType.GetConstructor(new Type[0]);

            // <invoke default constructor of the base class>
            if (constructorInfo != null)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, constructorInfo);
            }

            // this.actual = arg1;
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, actual);

            // this.inverted = arg2;
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Stfld, inverted);

            // if (!inverted)
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Brtrue_S, lblReturn);

            // this.Not = this(arg1, true);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Newobj, cb);
            il.Emit(OpCodes.Stfld, fiNot);

            il.MarkLabel(lblReturn);
            il.Emit(OpCodes.Ret);

            return cb;
        }

        /// <summary>
        /// Creates a public constructor on a dynamic type that takes one
        /// argument of the generic type definition of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the sole argument by the new constructor.
        /// </typeparam>
        /// <param name="typeBuilder">
        /// The type under construction.
        /// </param>
        /// <param name="privateCtor">
        /// The private constructor to which the new public constructor should
        /// delegate.
        /// </param>
        /// <returns>
        /// Returns a <see cref="ConstructorInfo"/> describing the new
        /// constructor.
        /// </returns>
        private static ConstructorInfo EmitPublicConstructor<T>(TypeBuilder typeBuilder, ConstructorInfo privateCtor)
        {
            var typeOfActual = GenericDefinitionOf<T>();

            var cb = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.SpecialName,
                CallingConventions.HasThis,
                new[] { typeOfActual });

            var il = cb.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Call, privateCtor);
            il.Emit(OpCodes.Ret);

            return cb;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of <typeparamref name="T"/>, retrieving
        /// the generic type definition of <typeparamref name="T"/> if applicable.
        /// </summary>
        /// <typeparam name="T">
        /// The type whose runtime representation is to be retrieved.
        /// </typeparam>
        /// <returns>
        /// Returns the generic definition of <typeparamref name="T"/>, or
        /// simply <typeparamref name="T"/> if it is not generic or already a
        /// generic type definition.
        /// </returns>
        private static Type GenericDefinitionOf<T>()
        {
            var type = typeof(T);

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                type = type.GetGenericTypeDefinition();
            }

            return type;
        }

        /// <summary>
        /// Applies generic constraints present on a list of generic
        /// <paramref name="arguments"/> to a list of paramter builders under
        /// construction.
        /// </summary>
        /// <param name='arguments'>
        /// The generic arguments whose constraints are to be copied.
        /// </param>
        /// <param name='parameters'>
        /// The parameter builders to which to apply the constraints.
        /// </param>
        private static void DefineGenericArguments(Type[] arguments, GenericTypeParameterBuilder[] parameters)
        {
            if (parameters.Length != arguments.Length)
            {
                throw new InvalidProgramException(string.Format("Created {0} generic parameters where {1} were expected.", parameters.Length, arguments.Length));
            }

            for (var i = 0; i < arguments.Length; ++i)
            {
                var arg = arguments[i];
                var p = parameters[i];
                var constraintsAndBaseType = arg.GetGenericParameterConstraints().Partition(t => t.IsInterface);
                var interfaces = constraintsAndBaseType.Item1.ToArray();
                var baseTypeArray = constraintsAndBaseType.Item2.ToArray();

                if (arg.GenericParameterAttributes != GenericParameterAttributes.None)
                {
                    p.SetGenericParameterAttributes(arg.GenericParameterAttributes);
                }

                if (interfaces.Length > 0)
                {
                    p.SetInterfaceConstraints(interfaces);
                }

                if (baseTypeArray.Length == 1)
                {
                    p.SetBaseTypeConstraint(baseTypeArray[0]);
                }
                else if (baseTypeArray.Length > 1)
                {
                    throw new InvalidProgramException("How can a type possibly have more than one base class constraint?");
                }
            }
        }
    }
}
