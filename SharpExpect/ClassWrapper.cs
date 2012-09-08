using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace SharpExpect
{
	internal static class ClassWrapper
	{
		private const string AssemblyName = "SharpExpectationsWrappers";
		private const string AssemblyFileName = AssemblyName + ".dll";
		private const string WrapperSuffix = "<>Wrapper";

		private static Dictionary<Type, Func<object, object>> WrappedTypes = new Dictionary<Type, Func<object, object>>();

		private static readonly AssemblyName assemblyName;
		private static readonly AssemblyBuilder assemblyBuilder;
		private static readonly ModuleBuilder moduleBuilder;

		private static readonly ConstructorInfo exnCtor = typeof(ExpectationException).GetConstructor(new[] { typeof(string) });
		private static readonly MethodInfo BadMatchInfo = typeof(ClassWrapper).GetMethod ("BadMatch", BindingFlags.Static | BindingFlags.Public);

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
			var wrapper = GenerateWrapper<T, M>();

			return (M) wrapper((object)actual);
		}

		public static void BadMatch (bool inverted, object actual, string methodName, object[] expectedArgs)
		{
			var actualDesc =  "[" + ToStringRespectingNulls(actual) + "]";
			var expectedDesc = DescriptionOfExpected(expectedArgs);
			var message = new StringBuilder("Failure: ")
				.Append ("Expected ")
				.Append (actualDesc)
				.Append (inverted ? " not " : " ")
				.Append (System.Text.RegularExpressions.Regex.Replace(methodName, "([A-Z])", " $1").ToLowerInvariant())
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

		private static Func<object, object> GenerateWrapper<T, M> ()
		{
			Type @base = typeof(M);
			Func<object, object> result = null;

			lock (WrappedTypes) {
				if (WrappedTypes.TryGetValue (@base, out result)) {
					return result;
				}
			}

			var builder = moduleBuilder.DefineType(
				@base.Name + WrapperSuffix,
				TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Public,
				@base);
			
			var actual = builder.BaseType.GetField ("actual", BindingFlags.Instance | BindingFlags.NonPublic);
			var inverted = builder.BaseType.GetField ("inverted", BindingFlags.Instance | BindingFlags.NonPublic);
			var privateCtor = EmitPrivateConstructor<T>(builder, actual, inverted);

			EmitPublicConstructor<T>(builder, privateCtor);

			foreach (var mi in @base.GetMethods (BindingFlags.Instance | BindingFlags.Public))
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
				var locAssert = il.DeclareLocal(typeof(bool));
				var locArgs = il.DeclareLocal(typeof(object[]));

				if (!actualCanBeNull)
				{
					EmitNullActualCheck(il, actual);
				}

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
				il.Emit (OpCodes.Brtrue, lblOk);

				// Assert failed - now we throw ALL THE THINGS

				// First, marshal the params into an object array
				EmitLoadNumber (il, parameters.Length);
				il.Emit (OpCodes.Newarr, typeof(object));
				il.Emit (OpCodes.Stloc_1);

				for (var i = 0; i < parameters.Length; ++i)
				{
					il.Emit (OpCodes.Ldloc_1);
					EmitLoadNumber(il, i);
					EmitLdArg (il, i + 1);
					Box(il, parameters[i].ParameterType);
					il.Emit (OpCodes.Stelem_I4);
				}

				// Next, push relevant args for BadMatch

				// Inverted
				il.Emit (OpCodes.Ldarg_0);
				il.Emit (OpCodes.Ldfld, inverted);

				// Actual
				il.Emit (OpCodes.Ldarg_0);
				il.Emit (OpCodes.Ldfld, actual);

				// Method name
				il.Emit (OpCodes.Ldstr, mi.Name);

				// Expected arg array
				il.Emit (OpCodes.Ldloc_1);

				// Call BadMatch (which throws)
				il.Emit (OpCodes.Call, BadMatchInfo);

				il.MarkLabel(lblOk);
				il.Emit (OpCodes.Ldc_I4_1); // 1 == true in IL
				il.Emit (OpCodes.Ret);
			}

			var createdType = builder.CreateType();
			var method = new DynamicMethod("construct", typeof(object), new[] { typeof(object) }, moduleBuilder);
			var mil = method.GetILGenerator();

			mil.Emit (OpCodes.Ldarg_0);
			mil.Emit (OpCodes.Unbox_Any, typeof(T));
			mil.Emit (OpCodes.Newobj, createdType.GetConstructor(new[] { typeof(T) }));
			Box (mil, createdType);
			mil.Emit (OpCodes.Ret);

			result = (Func<object, object>) method.CreateDelegate(typeof(Func<object, object>));

			lock (WrappedTypes)
			{
				Func<object, object> fn;
				if (WrappedTypes.TryGetValue(@base, out fn))
				{
					result = fn;
				}
				else
				{
					WrappedTypes[@base] = result;
				}
			}

			return result;
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

				if ((arg.GenericParameterAttributes & GenericParameterAttributes.None) != GenericParameterAttributes.None)
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
			if (type.IsValueType)
			{
				il.Emit (OpCodes.Box, type);
			}
			else
			{
				il.Emit (OpCodes.Castclass, typeof(object));
			}
		}

		/// <summary>
		/// Emits an instruction to push a constant 32-bit integer on to the
		/// stack, using the most efficient encoding available.
		/// </summary>
		/// <param name='il'>
		/// Il.
		/// </param>
		/// <param name='number'>
		/// Number.
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

		private static ConstructorBuilder EmitPrivateConstructor<T>(TypeBuilder typeBuilder, FieldInfo actual, FieldInfo inverted)
		{
			var cb = typeBuilder.DefineConstructor(
				MethodAttributes.Private | MethodAttributes.SpecialName,
				CallingConventions.HasThis,
				new[] { typeof(T), typeof(bool) });

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

			// if (!inverted) {
			il.Emit (OpCodes.Ldarg_2);
			il.Emit (OpCodes.Brtrue_S, lblReturn);

			// this.Not = this(arg1, true);
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ldarg_1);
			il.Emit (OpCodes.Ldc_I4_1);
			il.Emit (OpCodes.Newobj, cb);
			il.Emit (OpCodes.Stfld, fiNot);

			// }
			il.MarkLabel(lblReturn);

			// return;
			il.Emit (OpCodes.Ret);

			return cb;
		}

		private static ConstructorInfo EmitPublicConstructor<T>(TypeBuilder typeBuilder, ConstructorInfo privateCtor)
		{
			var cb = typeBuilder.DefineConstructor(
				MethodAttributes.Public | MethodAttributes.SpecialName,
				CallingConventions.HasThis,
				new[] { typeof(T) });

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
