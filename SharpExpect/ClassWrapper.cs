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

				// Call (which throws)
				il.Emit (OpCodes.Call, BadMatchInfo);

				il.MarkLabel(lblOk);
				il.Emit (OpCodes.Ldloc_0);
				il.Emit (OpCodes.Ret);
			}

			var createdType = builder.CreateType();

#if DEBUG
			//System.IO.File.Delete(assemblyName.Name + ".dll");
			//assemblyBuilder.Save (assemblyName.Name + ".dll");
#endif

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

		private static void EmitNullActualCheck (ILGenerator il, FieldInfo actual)
		{
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

		private static ConstructorBuilder EmitPrivateConstructor<T> (TypeBuilder typeBuilder, FieldInfo actual, FieldInfo inverted)
		{
			var cb = typeBuilder.DefineConstructor(
				MethodAttributes.Private | MethodAttributes.SpecialName,
				CallingConventions.HasThis,
				new[] { typeof(T), typeof(bool) });

			var il = cb.GetILGenerator();
			var lblReturn = il.DefineLabel();
			var fiNot = typeBuilder.BaseType.GetField ("Not", BindingFlags.Instance | BindingFlags.Public);

			// <invoke default constructor of the base class>
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Call, typeBuilder.BaseType.GetConstructor(new Type[0]));

			// this.actual = arg1;
			il.Emit (OpCodes.Ldarg_0);
			il.Emit (OpCodes.Ldarg_1);
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
