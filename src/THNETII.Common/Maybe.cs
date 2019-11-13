using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace THNETII.Common
{
    /// <summary>
    /// Supports a value of any type that may be unassigned.
    /// </summary>
    public static class Maybe
    {
        /// <summary>
        /// Convenience Constructor for a <see cref="Maybe{T}"/> structure that supports
        /// type inference with the specified argument.
        /// </summary>
        /// <typeparam name="T">The underlying type of the new <see cref="Maybe{T}"/> structure.</typeparam>
        /// <param name="value">The value of the initialized <see cref="Maybe{T}"/> structure.</param>
        /// <returns>An initialized <see cref="Maybe{T}"/> structure that is initialized to the specified <paramref name="value"/>.</returns>
        public static Maybe<T> Create<T>(T value) => new Maybe<T>(value);

        private static readonly Type openMaybeType = typeof(Maybe<>);

        /// <summary>
        /// Compares two <see cref="Maybe{T}"/> values and returns an integer that indicates
        /// whether the first value precedes, follows, or occurs in the same position in the sort
        /// order as the second value.
        /// </summary>
        /// <typeparam name="T">The common underlying type of both <see cref="Maybe{T}"/> values.</typeparam>
        /// <param name="m1">The first value to compare.</param>
        /// <param name="m2">The second value to which the first value is compared.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The
        /// return value has these meanings:
        /// <list type="table">
        /// <listheader><term>Value</term><description>Meaning</description></listheader>
        /// <item><term>Less than zero</term><description><paramref name="m1"/> precedes <paramref name="m2"/> in the sort order.</description></item>
        /// <item><term><c>0</c> (zero)</term><description><paramref name="m1"/> occurs in the same position in the sort order as <paramref name="m2"/>.</description></item>
        /// <item><term>Greater than zero</term><description><paramref name="m1"/> follows <paramref name="m2"/> in the sort order.</description></item>
        /// </list>
        /// </returns>
        public static int Compare<T>(Maybe<T> m1, Maybe<T> m2)
        {
            if (m1.HasValue)
            {
                if (m2.HasValue)
                {
                    return m1.Value switch
                    {
                        IComparable<T> compT => compT.CompareTo(m2.Value),
                        IComparable compAny => compAny.CompareTo(m2.Value),
                        _ => 0,
                    };
                }
                return -1;
            }
            else if (m2.HasValue)
                return 1;
            return 0;
        }

        /// <summary>
        /// Checks the equality of two <see cref="Maybe{T}"/> structures.
        /// </summary>
        /// <typeparam name="T">The common underlying type of both <see cref="Maybe{T}"/> values.</typeparam>
        /// <param name="m1">A <see cref="Maybe{T}"/> structure to be checked for equality against the other specified value.</param>
        /// <param name="m2">A <see cref="Maybe{T}"/> structure to be checked for equality against the other specified value.</param>
        /// <returns>
        /// <see langword="true"/> if both <paramref name="m1"/> and <paramref name="m2"/> are unset, or set to the same underlying value;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Equals<T>(Maybe<T> m1, Maybe<T> m2) => m1 == m2;

        /// <summary>
        /// Gets the underlying type of a type reference for a <see cref="Maybe{T}"/> type.
        /// </summary>
        /// <param name="maybeType">A type reference for a <see cref="Maybe{T}"/> structure. This parameter must not be <see langword="null"/>.</param>
        /// <returns>
        /// A type reference for the underlying type of the specified <see cref="Maybe{T}"/> type,
        /// or <see langword="null"/> if no <see cref="Maybe{T}"/> was specified.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="maybeType"/> is <see langword="null"/>.</exception>
        public static Type? GetUnderlyingType(Type maybeType)
        {
            var mt = maybeType.ThrowIfNull(nameof(maybeType))
#if NETSTANDARD1_3
                .GetTypeInfo()
#endif
                ;
            if (mt.IsGenericType && maybeType.GetGenericTypeDefinition() == openMaybeType)
                return maybeType.GetGenericArguments()[0];
            return null;
        }
    }

    /// <summary>
    /// Represents a value of type <typeparamref name="T"/> which may or may not be assigned.
    /// </summary>
    /// <typeparam name="T">The underlying value type of the <see cref="Maybe{T}"/> generic type.</typeparam>
    /// <remarks>
    /// This type is similar to the <see cref="Nullable{T}"/> type, but also allows for marking a reference as unset.
    /// </remarks>
    public struct Maybe<T> : IEquatable<Maybe<T>>, IEquatable<T>
    {
        private static readonly string nullString = $"{null}";
#pragma warning disable CA1000 // Do not declare static members on generic types
        /// <summary>
        /// Gets a <see cref="Maybe{T}"/> value where the value of <typeparamref name="T"/> is unset.
        /// </summary>
        /// <value>The default value of the <see cref="Maybe{T}"/> structure.</value>
        public static Maybe<T> NoValue { get => default; }
#pragma warning restore CA1000 // Do not declare static members on generic types

        private T value;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Maybe{T}"/> has a valid value of its
        /// underlying type.
        /// </summary>
        /// <value><see langword="true"/> if a value of its underlying type has been assigned 
        /// to the <see cref="Maybe{T}"/>; <see langword="false"/> if the <see cref="Maybe{T}"/> 
        /// has no value assigned to it.
        /// </value>
        public bool HasValue { get; private set; }

        /// <summary>
        /// Gets or sets the underlying value of the <see cref="Maybe{T}"/> structure.
        /// </summary>
        /// <value>A value of the underlying type of the <see cref="Maybe{T}"/> structure.</value>
        /// <remarks>
        /// Setting the <see cref="Value"/> property will always set the <see cref="HasValue"/>
        /// property to <see langword="true"/>.
        /// </remarks>
        /// <exception cref="InvalidOperationException">The <see cref="HasValue"/> property is <see langword="false"/> while attempting to get the value of the <see cref="Value"/> property.</exception>
        public T Value
        {
            get
            {
                if (HasValue)
                    return value;
                throw new InvalidOperationException();
            }
            set
            {
                this.value = value;
                HasValue = true;
            }
        }

        /// <summary>
        /// Initializes a new <see cref="Maybe{T}"/> structure to the specified value.<br/>
        /// The <see cref="HasValue"/> property of the new structure will evaluate to <see langword="true"/>.
        /// </summary>
        /// <param name="value">The value of the initialized <see cref="Maybe{T}"/> structure.</param>
        public Maybe(T value) : this()
        {
            Value = value;
            HasValue = true;
        }

        /// <summary>
        /// Clears the assigned underlying value (if any) and sets the <see cref="HasValue"/>
        /// property to <see langword="false"/>.
        /// </summary>
        public void Clear()
        {
            value = default!;
            HasValue = false;
        }

        /// <summary>
        /// Determines whether the current instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns><see langword="true"/> if the current instance is equal to <paramref name="obj"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// <para>
        /// Equality between two <see cref="Maybe{T}"/> values is determined by satisfying any of the following criteria:
        /// <list type="number">
        /// <item><term>The <see cref="HasValue"/> of both <see cref="Maybe{T}"/> values returns <see langword="false"/>.</term></item>
        /// <item><term>The <see cref="HasValue"/> of both <see cref="Maybe{T}"/> values returns <see langword="true"/> and <br/>the <see cref="Value"/> property of both <see cref="Maybe{T}"/> is equal.</term></item>
        /// </list>
        /// </para>
        /// <para>
        /// Equality between a <see cref="Maybe{T}"/> and a value of type <typeparamref name="T"/> is commutative and determined by satisfying all of the following criteria:
        /// <list type="number">
        /// <item><term>The <see cref="HasValue"/> of the <see cref="Maybe{T}"/> value returns <see langword="true"/>.</term></item>
        /// <item><term>The <see cref="Value"/> property of the <see cref="Maybe{T}"/> value is equal to the value of type <typeparamref name="T"/>.</term></item>
        /// </list>
        /// </para>
        /// <para>
        /// If <typeparamref name="T"/> implements <see cref="IEquatable{T}"/>,
        /// <see cref="Value"/> is compared using the <see cref="IEquatable{T}.Equals(T)"/> method.
        /// Otherwise, <see cref="object.Equals(object)"/> is used.
        /// </para>
        /// </remarks>
        public override bool Equals(object obj)
        {
            return obj switch
            {
                Maybe<T> otherMaybe => Equals(otherMaybe),
                T otherValue => Equals(otherValue),
                _ => base.Equals(obj),
            };
        }

        /// <summary>
        /// Determines whether the current instance is equal to the specified value.
        /// </summary>
        /// <param name="otherValue">The value of type <typeparamref name="T"/> to compare to.</param>
        /// <returns><see langword="true"/> if the current instance has a value and that value is equal to <paramref name="otherValue"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// <para>
        /// Equality between a <see cref="Maybe{T}"/> and a value of type <typeparamref name="T"/> is commutative and determined by satisfying all of the following criteria:
        /// <list type="number">
        /// <item><term>The <see cref="HasValue"/> of the <see cref="Maybe{T}"/> value returns <see langword="true"/>.</term></item>
        /// <item><term>The <see cref="Value"/> property of the <see cref="Maybe{T}"/> value is equal to the value of type <typeparamref name="T"/>.</term></item>
        /// </list>
        /// </para>
        /// <para>
        /// If <typeparamref name="T"/> implements <see cref="IEquatable{T}"/>,
        /// <see cref="Value"/> is compared using the <see cref="IEquatable{T}.Equals(T)"/> method.
        /// Otherwise, <see cref="object.Equals(object)"/> is used.
        /// </para>
        /// </remarks>
        public bool Equals(T otherValue)
        {
            if (HasValue)
            {
                return (value is IEquatable<T> equatableValue)
                    ? equatableValue.Equals(otherValue)
                    : value?.Equals(otherValue)
                    ?? Equals(value, otherValue);
            }
            else
                return false;
        }

        /// <summary>
        /// Determines whether the current instance is equal to the specified <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="otherMaybe">The <see cref="Maybe{T}"/> to compare to.</param>
        /// <returns><see langword="true"/> if the current instance has a value and that value is equal to <paramref name="otherMaybe"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// <para>
        /// Equality between two <see cref="Maybe{T}"/> values is determined by satisfying any of the following criteria:
        /// <list type="number">
        /// <item><term>The <see cref="HasValue"/> of both <see cref="Maybe{T}"/> values returns <see langword="false"/>.</term></item>
        /// <item><term>The <see cref="HasValue"/> of both <see cref="Maybe{T}"/> values returns <see langword="true"/> and <br/>the <see cref="Value"/> property of both <see cref="Maybe{T}"/> is equal.</term></item>
        /// </list>
        /// </para>
        /// <para>
        /// If <typeparamref name="T"/> implements <see cref="IEquatable{T}"/>,
        /// <see cref="Value"/> is compared using the <see cref="IEquatable{T}.Equals(T)"/> method.
        /// Otherwise, <see cref="object.Equals(object)"/> is used.
        /// </para>
        /// </remarks>
        public bool Equals(Maybe<T> otherMaybe)
        {
            if (HasValue)
            {
                return otherMaybe.HasValue && ((value is IEquatable<T> equatableValue)
                    ? equatableValue.Equals(otherMaybe.value)
                    : value?.Equals(otherMaybe.value)
                    ?? Equals(value, otherMaybe.value));
            }
            else
                return !otherMaybe.HasValue;
        }

        /// <summary>
        /// Returns a hash code for the current instance.
        /// </summary>
        /// <returns><c>0</c> (zero) if <see cref="HasValue"/> is <see langword="false"/> or <see cref="Value"/> is <see langword="null"/>; otherwise, the value obtained by invoking <see cref="object.GetHashCode"/> on <see cref="Value"/>.</returns>
        public override int GetHashCode() => HasValue ? (value?.GetHashCode() ?? 0) : 0;

        /// <summary>
        /// Retrieves the assigned underlying value of the <see cref="Maybe{T}"/> structure
        /// or the default value of the underlying type.
        /// </summary>
        /// <returns>
        /// The value of the <see cref="Value"/> property if the <see cref="HasValue"/>
        /// property is <see langword="true"/>; otherwise the default value of the underlying
        /// type.
        /// </returns>
        public T GetValueOrDefault() => GetValueOrDefault(default!);

        /// <summary>
        /// Retrieves the assigned underlying value of the <see cref="Maybe{T}"/> structure
        /// or the specified default value.
        /// </summary>
        /// <param name="default">The value to return if the <see cref="HasValue"/> is <see langword="false"/>.</param>
        /// <returns>
        /// The value of the <see cref="Value"/> property if the <see cref="HasValue"/>
        /// property evaluates to <see langword="true"/>; otherwise the value of the <paramref name="default"/>
        /// parameter.
        /// </returns>
        public T GetValueOrDefault(T @default) => HasValue ? Value : @default;

        /// <summary>
        /// Returns a <see cref="string"/> representing the current <see cref="Maybe{T}"/> value.
        /// </summary>
        /// <returns>
        /// <list type="table">
        /// <listheader><term><see cref="HasValue"/></term><description>Return Value</description></listheader>
        /// <item>
        /// <term><see langword="true"/></term>
        /// <description>
        /// <list type="bullet">
        /// <item><term>The empty string (<c>&quot;&quot;</c>) if <typeparamref name="T"/> is a reference type and <see cref="Value"/> is <see langword="null"/>.</term></item>
        /// <item><term>The return value from invoking <see cref="object.ToString"/> on <see cref="Value"/>.</term></item>
        /// </list>
        /// </description>
        /// </item>
        /// <item>
        /// <term><see langword="false"/></term>
        /// <description>The string <c>Maybe&lt;<typeparamref name="T"/>&gt;.<see cref="NoValue"/></c>, with <typeparamref name="T"/> replaced by the string representation of the type <typeparamref name="T"/>.</description>
        /// </item>
        /// </list>
        /// </returns>
        public override string ToString()
        {
            return HasValue
                ? (value is object obj && obj is null) ? nullString : value!.ToString()
                : $"{nameof(Maybe<T>)}<{typeof(T)}>.{nameof(NoValue)}";
        }

        /// <summary>
        /// Determines whether two <see cref="Maybe{T}"/> values are equal.
        /// </summary>
        /// <param name="left">The conversion tuple instance on the left side of the operator.</param>
        /// <param name="right">The conversion tuple instance on the right side of the operator.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// <para>
        /// Equality between two <see cref="Maybe{T}"/> values is determined by satisfying any of the following criteria:
        /// <list type="number">
        /// <item><term>The <see cref="HasValue"/> of both <see cref="Maybe{T}"/> values returns <see langword="false"/>.</term></item>
        /// <item><term>The <see cref="HasValue"/> of both <see cref="Maybe{T}"/> values returns <see langword="true"/> and <br/>the <see cref="Value"/> property of both <see cref="Maybe{T}"/> is equal.</term></item>
        /// </list>
        /// </para>
        /// <para>
        /// If <typeparamref name="T"/> implements <see cref="IEquatable{T}"/>,
        /// <see cref="Value"/> is compared using the <see cref="IEquatable{T}.Equals(T)"/> method.
        /// Otherwise, <see cref="object.Equals(object)"/> is used.
        /// </para>
        /// </remarks>
        /// <seealso cref="Equals(Maybe{T})"/>
        public static bool operator ==(Maybe<T> left, Maybe<T> right)
            => left.Equals(right);

        /// <summary>
        /// Determines whether two <see cref="Maybe{T}"/> values are not equal.
        /// </summary>
        /// <param name="left">The conversion tuple instance on the left side of the operator.</param>
        /// <param name="right">The conversion tuple instance on the right side of the operator.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// <para>
        /// Inequality between two <see cref="Maybe{T}"/> values is determined by satisfying any of the following criteria:
        /// <list type="number">
        /// <item><term>The <see cref="HasValue"/> property of one <see cref="Maybe{T}"/> value is <see langword="true"/> while the other is <see langword="false"/>.</term></item>
        /// <item><term>The <see cref="HasValue"/> of both <see cref="Maybe{T}"/> values returns <see langword="true"/> and <br/>the <see cref="Value"/> property of of one <see cref="Maybe{T}"/> is not equal to the other.</term></item>
        /// </list>
        /// </para>
        /// <para>
        /// If <typeparamref name="T"/> implements <see cref="IEquatable{T}"/>,
        /// <see cref="Value"/> is compared using the <see cref="IEquatable{T}.Equals(T)"/> method.
        /// Otherwise, <see cref="object.Equals(object)"/> is used.
        /// </para>
        /// </remarks>
        /// <seealso cref="Equals(Maybe{T})"/>
        public static bool operator !=(Maybe<T> left, Maybe<T> right)
            => !left.Equals(right);

        /// <summary>
        /// Determines whether a <see cref="Maybe{T}"/> value is equal to a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="maybe">The <see cref="Maybe{T}"/> comparand.</param>
        /// <param name="value">The comparand of type <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="maybe"/> has a value and that value is equal to <paramref name="value"/>.</returns>
        /// <remarks>
        /// <para>
        /// Equality between a <see cref="Maybe{T}"/> and a value of type <typeparamref name="T"/> is commutative and determined by satisfying all of the following criteria:
        /// <list type="number">
        /// <item><term>The <see cref="HasValue"/> of the <see cref="Maybe{T}"/> value returns <see langword="true"/>.</term></item>
        /// <item><term>The <see cref="Value"/> property of the <see cref="Maybe{T}"/> value is equal to the value of type <typeparamref name="T"/>.</term></item>
        /// </list>
        /// </para>
        /// <para>
        /// If <typeparamref name="T"/> implements <see cref="IEquatable{T}"/>,
        /// <see cref="Value"/> is compared using the <see cref="IEquatable{T}.Equals(T)"/> method.
        /// Otherwise, <see cref="object.Equals(object)"/> is used.
        /// </para>
        /// </remarks>
        /// <seealso cref="Equals(T)"/>
        public static bool operator ==(Maybe<T> maybe, T value)
            => maybe.Equals(value);

        /// <summary>
        /// Determines whether a <see cref="Maybe{T}"/> value is equal to a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="maybe">The <see cref="Maybe{T}"/> comparand.</param>
        /// <param name="value">The comparand of type <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="maybe"/> has a value and that value is equal to <paramref name="value"/>.</returns>
        /// <remarks>
        /// <para>
        /// Equality between a <see cref="Maybe{T}"/> and a value of type <typeparamref name="T"/> is commutative and determined by satisfying all of the following criteria:
        /// <list type="number">
        /// <item><term>The <see cref="HasValue"/> of the <see cref="Maybe{T}"/> value returns <see langword="true"/>.</term></item>
        /// <item><term>The <see cref="Value"/> property of the <see cref="Maybe{T}"/> value is equal to the value of type <typeparamref name="T"/>.</term></item>
        /// </list>
        /// </para>
        /// <para>
        /// If <typeparamref name="T"/> implements <see cref="IEquatable{T}"/>,
        /// <see cref="Value"/> is compared using the <see cref="IEquatable{T}.Equals(T)"/> method.
        /// Otherwise, <see cref="object.Equals(object)"/> is used.
        /// </para>
        /// </remarks>
        /// <seealso cref="Equals(T)"/>
        public static bool operator ==(T value, Maybe<T> maybe)
            => maybe.Equals(value);

        /// <summary>
        /// Determines whether a <see cref="Maybe{T}"/> value is not equal to a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="maybe">The <see cref="Maybe{T}"/> comparand.</param>
        /// <param name="value">The comparand of type <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="maybe"/> does not have a value or if the value is not equal to <paramref name="value"/>.</returns>
        /// <remarks>
        /// <para>
        /// Inequality between a <see cref="Maybe{T}"/> and a value of type <typeparamref name="T"/> is commutative and determined by satisfying any of the following criteria:
        /// <list type="number">
        /// <item><term>The <see cref="HasValue"/> of the <see cref="Maybe{T}"/> value returns <see langword="false"/>.</term></item>
        /// <item><term>The <see cref="Value"/> property of the <see cref="Maybe{T}"/> value is not equal to the value of type <typeparamref name="T"/>.</term></item>
        /// </list>
        /// </para>
        /// <para>
        /// If <typeparamref name="T"/> implements <see cref="IEquatable{T}"/>,
        /// <see cref="Value"/> is compared using the <see cref="IEquatable{T}.Equals(T)"/> method.
        /// Otherwise, <see cref="object.Equals(object)"/> is used.
        /// </para>
        /// </remarks>
        /// <seealso cref="Equals(T)"/>
        public static bool operator !=(Maybe<T> maybe, T value)
            => !maybe.Equals(value);

        /// <summary>
        /// Determines whether a <see cref="Maybe{T}"/> value is not equal to a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="maybe">The <see cref="Maybe{T}"/> comparand.</param>
        /// <param name="value">The comparand of type <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="maybe"/> does not have a value or if the value is not equal to <paramref name="value"/>.</returns>
        /// <remarks>
        /// <para>
        /// Inequality between a <see cref="Maybe{T}"/> and a value of type <typeparamref name="T"/> is commutative and determined by satisfying any of the following criteria:
        /// <list type="number">
        /// <item><term>The <see cref="HasValue"/> of the <see cref="Maybe{T}"/> value returns <see langword="false"/>.</term></item>
        /// <item><term>The <see cref="Value"/> property of the <see cref="Maybe{T}"/> value is not equal to the value of type <typeparamref name="T"/>.</term></item>
        /// </list>
        /// </para>
        /// <para>
        /// If <typeparamref name="T"/> implements <see cref="IEquatable{T}"/>,
        /// <see cref="Value"/> is compared using the <see cref="IEquatable{T}.Equals(T)"/> method.
        /// Otherwise, <see cref="object.Equals(object)"/> is used.
        /// </para>
        /// </remarks>
        /// <seealso cref="Equals(T)"/>
        public static bool operator !=(T value, Maybe<T> maybe)
            => !maybe.Equals(value);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA2225 // Operator overloads have named alternates
        /// <summary>
        /// Implicityly casts a value of type <typeparamref name="T"/> to a <see cref="Maybe{T}"/> value.
        /// </summary>
        /// <param name="value">The value of type <typeparamref name="T"/> to cast.</param>
        /// <returns>A <see cref="Maybe{T}"/> whose <see cref="HasValue"/> property is <see langword="true"/> and whose <see cref="Value"/> is equal to <paramref name="value"/>.</returns>
        /// <seealso cref="Maybe.Create{T}(T)"/>
        public static implicit operator Maybe<T>(T value)
            => new Maybe<T>(value);

        /// <summary>
        /// Casts the <see cref="Maybe{T}"/> value to a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="maybe">The <see cref="Maybe{T}"/> to cast.</param>
        /// <returns>The <see cref="Value"/> of <paramref name="maybe"/>.</returns>
        /// <exception cref="InvalidCastException"><paramref name="maybe"/> does not have a value. <see cref="Exception.InnerException"/> contains the <see cref="InvalidOperationException"/> thrown by accessing the <see cref="Value"/> property of <paramref name="maybe"/>.</exception>
        public static explicit operator T(Maybe<T> maybe)
        {
            try { return maybe.Value; }
            catch (InvalidOperationException invOpExcept)
            { throw new InvalidCastException(null, invOpExcept); }
        }
#pragma warning restore CA2225 // Operator overloads have named alternates
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
