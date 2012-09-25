using System;
using System.Reflection.Emit;

namespace ExpectBetter.Codegen
{
    /// <summary>
    /// Provides various extension methods on <see cref="ILGenerator"/> to
    /// assist in writing common CIL sequences.
    /// </summary>
    internal static class ILUtils
    {
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
        internal static void EmitLoadNumber(this ILGenerator il, int number)
        {
            switch (number)
            {
                case -1: il.Emit(OpCodes.Ldc_I4_M1); return;
                case 0: il.Emit(OpCodes.Ldc_I4_0); return;
                case 1: il.Emit(OpCodes.Ldc_I4_1); return;
                case 2: il.Emit(OpCodes.Ldc_I4_2); return;
                case 3: il.Emit(OpCodes.Ldc_I4_3); return;
                case 4: il.Emit(OpCodes.Ldc_I4_4); return;
                case 5: il.Emit(OpCodes.Ldc_I4_5); return;
                case 6: il.Emit(OpCodes.Ldc_I4_6); return;
                case 7: il.Emit(OpCodes.Ldc_I4_7); return;
                case 8: il.Emit(OpCodes.Ldc_I4_8); return;
            }

            if (number >= -128 && number <= 127)
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)number);
                return;
            }

            il.Emit(OpCodes.Ldc_I4, number);
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
        internal static void EmitLdArg(this ILGenerator il, int index)
        {
            switch (index)
            {
                case 0: il.Emit(OpCodes.Ldarg_0); break;
                case 1: il.Emit(OpCodes.Ldarg_1); break;
                case 2: il.Emit(OpCodes.Ldarg_2); break;
                case 3: il.Emit(OpCodes.Ldarg_3); break;
                default:
                    if (index < 256)
                    {
                        il.Emit(OpCodes.Ldarg_S, index);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldarg, index);
                    }
                    break;
            }
        }


        /// <summary>
        /// Emits an instruction to load the address of the argument at the
        /// given <paramref name="index"/> on to the stack, using the most
        /// efficient encoding available.
        /// </summary>
        /// <param name="il">
        /// The <see cref="ILGenerator"/> to emit the load instruction.
        /// </param>
        /// <param name="index">
        /// The index of the argument whose address is to be loaded.
        /// </param>
        internal static void EmitLdArgA(this ILGenerator il, int index)
        {
            if (index < 256)
            {
                il.Emit(OpCodes.Ldarga_S, index);
            }
            else
            {
                il.Emit(OpCodes.Ldarga, index);
            }
        }

        /// <summary>
        /// Emits a series of instructions which load an argument of the given
        /// <paramref name="type"/> at the given <paramref name="index"/> as an
        /// object reference (type 'O').
        /// </summary>
        /// <param name="il">
        /// The <see cref="ILGenerator"/> to emit the instructions.
        /// </param>
        /// <param name="index">
        /// The index of the argument to be loaded.
        /// </param>
        /// <param name="type">
        /// The type to be loaded.  May be a generic argument.
        /// </param>
        internal static void EmitLdArgAsObject(this ILGenerator il, int index, Type type)
        {
            if (type.IsGenericParameter)
            {
                il.EmitLdArgA(index);
                il.Emit(OpCodes.Ldobj, type);
                il.Emit(OpCodes.Box, type);   // nop on reference types
            }
            else if (type.IsValueType)
            {
                il.EmitLdArg(index);
                il.Emit(OpCodes.Box, type);
            }
            else
            {
                il.EmitLdArg(index);
                il.Emit(OpCodes.Castclass, typeof(object));
            }
        }
    }
}
