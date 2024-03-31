using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace THNETII.Common;

/// <summary>
/// Provides extension methods for enumeration types that are annotated with
/// the <see cref="FlagsAttribute"/> attribute.
/// </summary>
public static class FlagsExtensions
{
    /// <summary>
    /// Determines whether any of the flags in a Flags enum value match the
    /// flags set in a comparison value.
    /// </summary>
    /// <typeparam name="T">An enumeration type. Typically a Type that is annotated with the <see cref="FlagsAttribute"/> attribute.</typeparam>
    /// <param name="flag">The value to check.</param>
    /// <param name="other">The comparison value to check against.</param>
    /// <returns>
    /// <see langword="true"/> if any bits in <paramref name="flag"/> and <paramref name="other"/> overlap.
    /// <see langword="false"/> if none overlap, or if any of the parameters has no bits set.
    /// </returns>
    /// <remarks>
    /// The <see cref="HasAnyFlag{T}(T, T)"/> method checks whether <em>any</em> bit is set in both <paramref name="flag"/> and <paramref name="other"/>.
    /// In contrast to the <see cref="Enum.HasFlag(Enum)"/> method, not <em>all</em> bits that are set in <paramref name="other"/> need to be set in <paramref name="flag"/>,
    /// a partial match will still return <see langword="true"/>.
    /// </remarks>
    /// <seealso cref="Enum.HasFlag(Enum)"/>
    public static bool HasAnyFlag<T>(this T flag, T other)
        where T : struct, Enum
        => FlagsTypeHelper<T>.HasCommonBits(flag, other);

    private static class FlagsTypeHelper<T> where T : struct, Enum
    {
        public static readonly Func<T, T, bool> HasCommonBits = GetHasCommonBits();

        private static Func<T, T, bool> GetHasCommonBits()
        {
            var baseType = Enum.GetUnderlyingType(typeof(T));
            if (baseType == typeof(int))
                return HasCommonBitsInt32;
            else if (baseType == typeof(uint))
                return HasCommonBitsUInt32;
            else if (baseType == typeof(long))
                return HasCommonBitsInt64;
            else if (baseType == typeof(ulong))
                return HasCommonBitsUInt64;
            else if (baseType == typeof(short))
                return HasCommonBitsInt16;
            else if (baseType == typeof(ushort))
                return HasCommonBitsUInt16;
            else if (baseType == typeof(byte))
                return HasCommonBitsUInt8;
            else if (baseType == typeof(sbyte))
                return HasCommonBitsInt8;

            // This should never happen, but if it does, we can still try
            // to compile a LINQ expression doing the same thing.
            return GetHasCommonBitsExpression(baseType);
        }

        private unsafe static bool HasCommonBitsInt32(T left, T right)
        {
            int* pLeft = (int*)Unsafe.AsPointer(ref left);
            int* pRight = (int*)Unsafe.AsPointer(ref right);
            return (*pLeft & *pRight) != 0;
        }

        private unsafe static bool HasCommonBitsUInt32(T left, T right)
        {
            uint* pLeft = (uint*)Unsafe.AsPointer(ref left);
            uint* pRight = (uint*)Unsafe.AsPointer(ref right);
            return (*pLeft & *pRight) != 0U;
        }

        private unsafe static bool HasCommonBitsInt64(T left, T right)
        {
            long* pLeft = (long*)Unsafe.AsPointer(ref left);
            long* pRight = (long*)Unsafe.AsPointer(ref right);
            return (*pLeft & *pRight) != 0L;
        }

        private unsafe static bool HasCommonBitsUInt64(T left, T right)
        {
            ulong* pLeft = (ulong*)Unsafe.AsPointer(ref left);
            ulong* pRight = (ulong*)Unsafe.AsPointer(ref right);
            return (*pLeft & *pRight) != 0UL;
        }

        private unsafe static bool HasCommonBitsInt16(T left, T right)
        {
            short* pLeft = (short*)Unsafe.AsPointer(ref left);
            short* pRight = (short*)Unsafe.AsPointer(ref right);
            return (*pLeft & *pRight) != 0;
        }

        private unsafe static bool HasCommonBitsUInt16(T left, T right)
        {
            ushort* pLeft = (ushort*)Unsafe.AsPointer(ref left);
            ushort* pRight = (ushort*)Unsafe.AsPointer(ref right);
            return ((uint)*pLeft & *pRight) != 0U;
        }
        private unsafe static bool HasCommonBitsInt8(T left, T right)
        {
            sbyte* pLeft = (sbyte*)Unsafe.AsPointer(ref left);
            sbyte* pRight = (sbyte*)Unsafe.AsPointer(ref right);
            return (*pLeft & *pRight) != 0;
        }

        private unsafe static bool HasCommonBitsUInt8(T left, T right)
        {
            byte* pLeft = (byte*)Unsafe.AsPointer(ref left);
            byte* pRight = (byte*)Unsafe.AsPointer(ref right);
            return ((uint)*pLeft & *pRight) != 0U;
        }

        private static Func<T, T, bool> GetHasCommonBitsExpression(Type baseType)
        {
            var flagType = typeof(T);
            var leftParam = Expression.Parameter(flagType);
            var rightParam = Expression.Parameter(flagType);
            var body = Expression.NotEqual(
                Expression.And(
                    Expression.Convert(leftParam, baseType),
                    Expression.Convert(rightParam, baseType)
                    ),
                Expression.Default(baseType)
                );
            var lambda = Expression.Lambda<Func<T, T, bool>>(body, leftParam, rightParam);
            return lambda.Compile();
        }
    }
}
