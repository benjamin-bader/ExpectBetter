using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace ExpectBetter
{
	public static class ClassWrapper
	{
		private const string AssemblyName = "ExpectBetterWrappers";
		private const string AssemblyFileName = AssemblyName + ".dll";
		private const string WrapperPrefix = "WrapperOf$";

		private const string BugReportMessage = "If you see this message, you have encounted a bug in ExpectBetterations.  Please report this error (and stack trace) to the developers at https://github.com/benjamin-bader/ExpectBetter.  Thanks, and we're sorry!";

        //private static Dictionary<Type, Func<object, object>> WrappedTypes = new Dictionary<Type, Func<object, object>>();
        private static Dictionary<Type, Type> WrappedTypes = new Dictionary<Type, Type>();
        private static Dictionary<Type, Func<object, object>> Constructors = new Dictionary<Type, Func<object, object>>();

		private static readonly AssemblyName assemblyName;
		internal static readonly AssemblyBuilder assemblyBuilder;
		internal static readonly ModuleBuilder moduleBuilder;

		private static readonly ConstructorInfo exnCtor = typeof(ExpectationException).GetConstructor(new[] { typeof(string) });
		private static readonly MethodInfo BadMatchInfo = typeof(ClassWrapper).GetMethod ("BadMatch", BindingFlags.Static | BindingFlags.Public);
		private static readonly MethodInfo GiveBugReportInfo = typeof(ClassWrapper).GetMethod("GiveBugReport", BindingFlags.Static | BindingFlags.Public);

		static ClassWrapper ()
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
		public static void SaveAssembly()
		{
			System.IO.File.Delete(AssemblyFileName);
			assemblyBuilder.Save(AssemblyFileName);
		}
#endif

		public static M Wrap<T, M> (T actual)
			where M : BaseMatcher<T, M>
		{
			var wrapper = RetrieveWrapper<T, M>();

            if (wrapper.ContainsGenericParameters)
            {
                // HACK wtf
                var parameters = typeof(T).GetGenericArguments();
                wrapper = wrapper.MakeGenericType(parameters);
            }

            return (M)Activator.CreateInstance(wrapper, new object[] { actual });
			//return (M) wrapper((object)actual);
		}

		public static void GiveBugReport(Exception ex)
		{
			throw new ExpectationException(BugReportMessage, ex);
		}

		public static void BadMatch (string actualDesc, string expectedDesc, bool inverted, object actual, string methodName, object[] expectedArgs)
		{
			actualDesc = "[" + (actualDesc ?? ToStringRespectingNulls(actual)) + "]";
			expectedDesc = expectedDesc ?? DescriptionOfExpected(expectedArgs);
			var message = new StringBuilder("Failure: ")
				.Append ("Expected ")
				.Append (actualDesc)
				.Append (inverted ? " not" : "")
				.Append (System.Text.RegularExpressions.Regex.Replace(methodName, "([A-Z])", " $1").ToLowerInvariant())
				.Append (" ")
				.Append (expectedDesc)
				.ToString();

			Console.WriteLine(message);

			throw new ExpectationException(message);
		}

		private static string DescriptionOfExpected (object[] expectedArgs)
		{
			return expectedArgs
				.Select (ToStringRespectingNulls)
				.Select (str => "[" + str + "]")
				.Interject(new[] { ", " })
				.Aggregate(new StringBuilder(), (sb, str) => sb.Append (str))
				.ToString();
		}

		private static string ToStringRespectingNulls (object obj)
		{
            return ReferenceEquals (obj, null)
				? "null"
				: obj.ToString ();
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
			var @base = typeof(M);
            Type result = null;
            //Func<object, object> result = null;

            if (@base.IsGenericType && !@base.IsGenericTypeDefinition)
            {
                @base = @base.GetGenericTypeDefinition();
            }

            lock (WrappedTypes)
			{
				if (WrappedTypes.TryGetValue (@base, out result))
				{
					return result;
				}
			}

			var completedNewType = GenerateWrapper<T>(@base);
            result = completedNewType; //GenerateFactoryFunction<T>(completedNewType);
			
			lock (WrappedTypes)
			{
				Type fn;
				if (WrappedTypes.TryGetValue(@base, out fn))
				{
					result = fn;
				}
				else
				{
					WrappedTypes[@base] = result;
				}
			}

            //assemblyBuilder.Save(AssemblyFileName);
			
			return result;
		}

		private static Type GenerateWrapper<T> (Type @base)
		{
			var builder = moduleBuilder.DefineType(
				WrapperPrefix + @base.Name,
				TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Public,
				@base);

            if (@base.IsGenericTypeDefinition)
            {
                var genericArgs = @base.GetGenericArguments();
                var typeNames = Array.ConvertAll(genericArgs, arg => arg.Name);
                var parameterBuilders = builder.DefineGenericParameters(typeNames);

                for (var i = 0; i < parameterBuilders.Length; ++i)
                {
                    var baseArg = genericArgs[i];
                    var argBuilder = parameterBuilders[i];
                    var interfacesAndBaseType = baseArg.GetGenericParameterConstraints().Partition(t => t.IsInterface);
                    var interfaces = interfacesAndBaseType.Item1.ToArray();
                    var baseType = interfacesAndBaseType.Item2.SingleOrDefault();

                    argBuilder.SetGenericParameterAttributes(baseArg.GenericParameterAttributes);

                    if (baseType != null)
                    {
                        argBuilder.SetBaseTypeConstraint(baseType);
                    }

                    if (interfaces.Length > 0)
                    {
                        argBuilder.SetInterfaceConstraints(interfaces);
                    }
                }
            }
			
			var actual = builder.BaseType.GetField ("actual", BindingFlags.Instance | BindingFlags.NonPublic);
			var inverted = builder.BaseType.GetField ("inverted", BindingFlags.Instance | BindingFlags.NonPublic);
			var privateCtor = EmitPrivateConstructor<T>(builder, actual, inverted);

			EmitPublicConstructor<T>(builder, privateCtor);

			foreach (var mi in @base.GetMethods (BindingFlags.Instance | BindingFlags.Public))
			{
				if (mi.IsSpecialName || typeof(object) == mi.DeclaringType)
				{
					continue;
				}
				
				if (typeof(bool) != mi.ReturnType)
				{
					var message = string.Format("Invalid return type '{0}' in method '{1}'", mi.ReturnType, mi.Name);
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
				Array.ConvertAll (parameters, p => p.ParameterType));
			
			if (mi.IsGenericMethod)
			{
				DefineGenericParameters(wrapper, mi);
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
			
			il.Emit (OpCodes.Ldarg_0);
			
			for (var i = 0; i < parameters.Length; ++i)
			{
				Console.WriteLine("Method {0}: loading arg {1}", mi.Name, i + 1);
				EmitLdArg(il, i + 1);
			}
			
			il.Emit (OpCodes.Call, mi);
			il.Emit (OpCodes.Stloc_0);
			il.Emit (OpCodes.Ldloc_0);
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ldfld, inverted);
			il.Emit (OpCodes.Xor);
			il.Emit (OpCodes.Brfalse_S, lblErr);
			
			il.Emit (OpCodes.Leave_S, lblOk);
			
			// Assert failed - now we throw ALL THE THINGS
			il.MarkLabel(lblErr);
			
			// First, marshal the params into an object array
			EmitLoadNumber (il, parameters.Length);
			il.Emit (OpCodes.Newarr, typeof(object));
			il.Emit (OpCodes.Stloc_1);
			
			for (var i = 0; i < parameters.Length; ++i)
			{
				il.Emit (OpCodes.Ldloc_1);
				EmitLoadNumber(il, i);
				il.Emit(OpCodes.Conv_I); // 'stelem' requires a nativeint index
                EmitLdArgAsObject(il, i + 1, parameters[i].ParameterType);
				//EmitLdArg (il, i + 1);
				//Box(il, parameters[i].ParameterType);
				il.Emit (OpCodes.Stelem, typeof(object));
			}
			
			// Next, push relevant args for BadMatch

			// Description of actual
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, @base.GetField("actualDescription", BindingFlags.Instance | BindingFlags.NonPublic));

			// Description of expected
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, @base.GetField("expectedDescription", BindingFlags.Instance | BindingFlags.NonPublic));

			// Inverted
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ldfld, inverted);
			
			// Actual
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ldflda, actual);
            il.Emit (OpCodes.Ldobj, actual.FieldType);
            il.Emit (OpCodes.Box, actual.FieldType);
			
			// Method name
			il.Emit (OpCodes.Ldstr, mi.Name);
			
			// Expected arg array
			il.Emit (OpCodes.Ldloc_1);
			
			// Call BadMatch (which throws)
			il.Emit (OpCodes.Call, BadMatchInfo);
			
			// catch InvalidProgramExceptions - these are bugs generated somewhere in this file.
			il.BeginCatchBlock(typeof(InvalidProgramException));
			il.Emit(OpCodes.Call, GiveBugReportInfo);
			
			// ditto for BadImageFormatExceptions
			il.BeginCatchBlock(typeof(BadImageFormatException));
			il.Emit(OpCodes.Call, GiveBugReportInfo);
			
			il.EndExceptionBlock();
			
			il.MarkLabel(lblOk);
			il.Emit (OpCodes.Ldc_I4_1); // 1 == true in IL
			il.Emit (OpCodes.Ret);
		}

		/// <summary>
		/// For a given type that has a constructor taking one argument of type
		/// <typeparamref name="T"/>, creates a <see cref="Func&lt;Object, Object&gt;"/>
		/// invoking said constructor and returns it.  
		/// </summary>
		/// <returns>
		/// The factory function.
		/// </returns>
		/// <param name='createdType'>
		/// The type for which to create a factory function.
		/// </param>
		/// <typeparam name='T'>
		/// The type of parameter consumed by the wrapped type's constructor.
		/// </typeparam>
		private static Func<object, object> GenerateFactoryFunction<T>(Type createdType)
		{
            var argType = typeof(T);

            if (argType.IsGenericType && !argType.IsGenericTypeDefinition)
            {
                argType = argType.GetGenericTypeDefinition();
            }

            var constructorInfo = createdType.GetConstructors().First();

			if (constructorInfo == null)
			{
				throw new ArgumentException("The created type must have a public constructor taking one argument of type T.");
			}

			var method = new DynamicMethod("construct", typeof(object), new[] { typeof(object) }, moduleBuilder);
			var mil = method.GetILGenerator();
			
			mil.Emit (OpCodes.Ldarg_0);
			mil.Emit (OpCodes.Unbox_Any, argType);
			mil.Emit (OpCodes.Newobj, constructorInfo);
			Box (mil, createdType);
			mil.Emit (OpCodes.Ret);
			
			return (Func<object, object>) method.CreateDelegate(typeof(Func<object, object>));
		}

		/// <summary>
		/// Replicates the base method's generic arguments and their constraints.
		/// </summary>
		/// <param name='wrapper'>
		/// The <see cref="MethodBuilder"/> constructing the override.
		/// </param>
		/// <param name='mi'>
		/// The <see cref="MethodInfo"/> representing the base method.
		/// </param>
		/// <exception cref="InvalidProgramException">
		/// Thrown when more than one base-type constraint is present on a parameter.
		/// This shouldn't happen, but malformed IL could theoretically cause this.
		/// </exception>
		private static void DefineGenericParameters(MethodBuilder wrapper, MethodInfo mi)
		{
			var arguments = mi.GetGenericArguments();
			var names = Array.ConvertAll(arguments, arg => arg.Name);
			var parameters = wrapper.DefineGenericParameters(names);

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

		/// <summary>
		/// Emits an instruction to load the argument from the given constant
		/// index, using the most efficient encoding.
		/// </summary>
		/// <param name='il'>
		/// The <see cref="ILGenerator"/> that should emit the instruction.
		/// </param>
		/// <param name='index'>
		/// The argument index.  For instance methods, an index of zero
		/// corresponds to the <see langword="this"/> keyword.
		/// </param>
		private static void EmitLdArg (ILGenerator il, int index)
		{
			switch (index)
			{
			case 0: il.Emit (OpCodes.Ldarg_0); break;
			case 1: il.Emit (OpCodes.Ldarg_1); break;
			case 2: il.Emit (OpCodes.Ldarg_2); break;
			case 3: il.Emit (OpCodes.Ldarg_3); break;
			default:
				if (index < 256)
				{
					il.Emit (OpCodes.Ldarg_S, index);
				}
				else
				{
					il.Emit (OpCodes.Ldarg, index);
				}
				break;
			}
		}

		/// <summary>
		/// Emits an instruction to convert the reference on top of the stack
		/// (of the given <paramref name="type"/>) to a reference of type
		/// <see cref="Object"/>.
		/// </summary>
		/// <param name='il'>
		/// The <see cref="ILGenerator"/> with which to emit the instruction.
		/// </param>
		/// <param name='type'>
		/// The type of the topmost reference on the stack.
		/// </param>
		private static void Box (ILGenerator il, Type type)
		{
            if (type.IsGenericParameter)
            {
                il.Emit(OpCodes.Box, type);
            }
            if (type.IsValueType)
			{
				il.Emit (OpCodes.Box, type);
			}
			else
			{
				il.Emit (OpCodes.Castclass, typeof(object));
			}
		}

        private static void EmitLdArgAsObject(ILGenerator il, int index, Type type)
        {
            if (type.IsGenericParameter)
            {
                Console.WriteLine("Loading arg {0} (type {1}) as object.", index, type.Name);
                EmitLdArgA(il, index);
                il.Emit(OpCodes.Ldobj, type);
                il.Emit(OpCodes.Box, type);
            }
            else if (type.IsValueType)
            {
                EmitLdArg(il, index);
                il.Emit(OpCodes.Box, type);
            }
            else
            {
                EmitLdArg(il, index);
                il.Emit(OpCodes.Castclass, typeof(object));
            }
        }

        private static void EmitLdArgA(ILGenerator il, int index)
        {
            if (index < 255)
            {
                il.Emit(OpCodes.Ldarga_S, index);
            }
            else
            {
                il.Emit(OpCodes.Ldarga, index);
            }
        }

		/// <summary>
		/// Emits an instruction to push a constant 32-bit integer on to the
		/// stack, using the most efficient encoding available.
		/// </summary>
		/// <param name='il'>
		/// The <see cref="ILGenerator"/> to emit the load instruction.
		/// </param>
		/// <param name='number'>
		/// The constant to be loaded.
		/// </param>
		private static void EmitLoadNumber (ILGenerator il, int number)
		{
			switch (number)
			{
			case -1: il.Emit(OpCodes.Ldc_I4_M1); return;
			case 0:  il.Emit(OpCodes.Ldc_I4_0); return;
			case 1:  il.Emit(OpCodes.Ldc_I4_1); return;
			case 2:  il.Emit(OpCodes.Ldc_I4_2); return;
			case 3:  il.Emit(OpCodes.Ldc_I4_3); return;
			case 4:  il.Emit(OpCodes.Ldc_I4_4); return;
			case 5:  il.Emit(OpCodes.Ldc_I4_5); return;
			case 6:  il.Emit(OpCodes.Ldc_I4_6); return;
			case 7:  il.Emit(OpCodes.Ldc_I4_7); return;
			case 8:  il.Emit(OpCodes.Ldc_I4_8); return;
			}

			if (number >= -128 && number <= 127)
			{
				il.Emit (OpCodes.Ldc_I4_S, (sbyte) number);
				return;
			}

			il.Emit (OpCodes.Ldc_I4, number);
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

			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ldfld, actual);
			il.Emit (OpCodes.Ldnull);
			il.Emit (OpCodes.Ceq);
			il.Emit (OpCodes.Brfalse, lblOk);

			il.Emit (OpCodes.Ldstr, "Actual cannot be null.");
			il.Emit (OpCodes.Newobj, exnCtor);
			il.Emit (OpCodes.Throw);

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
            var typeOfActual = typeof(T);

            if (typeOfActual.IsGenericType && !typeOfActual.IsGenericTypeDefinition)
            {
                typeOfActual = typeOfActual.GetGenericTypeDefinition();
            }

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
			il.Emit (OpCodes.Stfld, actual);

			// this.inverted = arg2;
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ldarg_2);
			il.Emit (OpCodes.Stfld, inverted);

			// if (!inverted)
			il.Emit (OpCodes.Ldarg_2);
			il.Emit (OpCodes.Brtrue_S, lblReturn);

			// this.Not = this(arg1, true);
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ldarg_1);
			il.Emit (OpCodes.Ldc_I4_1);
			il.Emit (OpCodes.Newobj, cb);
			il.Emit (OpCodes.Stfld, fiNot);

			il.MarkLabel(lblReturn);
			il.Emit (OpCodes.Ret);

			return cb;
		}

		private static ConstructorInfo EmitPublicConstructor<T>(TypeBuilder typeBuilder, ConstructorInfo privateCtor)
		{
            var typeOfActual = typeof(T);

            if (typeOfActual.IsGenericType && !typeOfActual.IsGenericTypeDefinition)
            {
                typeOfActual = typeOfActual.GetGenericTypeDefinition();
            }

			var cb = typeBuilder.DefineConstructor(
				MethodAttributes.Public | MethodAttributes.SpecialName,
				CallingConventions.HasThis,
				new[] { typeOfActual });

			var il = cb.GetILGenerator();
            
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ldarg_1);
			il.Emit (OpCodes.Ldc_I4_0);
			il.Emit (OpCodes.Call, privateCtor);
			il.Emit (OpCodes.Ret);

			return cb;
		}
	}
}
