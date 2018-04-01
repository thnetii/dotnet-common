using System;
using System.Reflection;

namespace THNETII.Common
{
    /// <summary>
    /// Supports a value of any type that may be unassigned.
    /// </summary>
    public static class Maybe
    {
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
                    switch (m1.Value)
                    {
                        case IComparable<T> compT:
                            return compT.CompareTo(m2.Value);
                        case IComparable compAny:
                            return compAny.CompareTo(m2.Value);
                        default:
                            return 0;
                    }
                }
                return -1;
            }
            else if (m2.HasValue)
                return 1;
            return 0;
        }

        public static bool Equals<T>(Maybe<T> m1, Maybe<T> m2) => m1 == m2;

        public static Type GetUnderlyingType(Type maybeType)
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
        private bool hasValue;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Maybe{T}"/> has a valid value of its
        /// underlying type.
        /// </summary>
        /// <value><c>true</c> if a value of its underlying type has been assigned 
        /// to the <see cref="Maybe{T}"/>; <c>false</c> if the <see cref="Maybe{T}"/> 
        /// has no value assigned to it.
        /// </value>
        public bool HasValue => hasValue;

        /// <summary>
        /// Gets or sets the underlying value of the <see cref="Maybe{T}"/> structure.
        /// </summary>
        /// <value>A value of the underlying type of the <see cref="Maybe{T}"/> structure.</value>
        /// <remarks>
        /// Setting the <see cref="Value"/> property will always set the <see cref="HasValue"/>
        /// property to <c>true</c>.
        /// </remarks>
        /// <exception cref="InvalidOperationException">The <see cref="HasValue"/> property is <c>false</c> while attempting to get the value of the <see cref="Value"/> property.</exception>
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
                hasValue = true;
            }
        }

        /// <summary>
        /// Initializes a new <see cref="Maybe{T}"/> structure to the specified value.<br/>
        /// The <see cref="HasValue"/> property of the new structure will evaluate to <c>true</c>.
        /// </summary>
        /// <param name="value">The value of the initialized <see cref="Maybe{T}"/> structure.</param>
        public Maybe(T value) : this()
        {
            Value = value;
        }

        /// <summary>
        /// Clears the assigned underlying value (if any) and sets the <see cref="HasValue"/>
        /// property to <c>false</c>.
        /// </summary>
        public void Unset()
        {
            value = default;
            hasValue = false;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case Maybe<T> otherMaybe:
                    return Equals(otherMaybe);
                case T otherValue:
                    return Equals(otherValue);
                default:
                    return base.Equals(obj);
            }
        }

        /// <inheritdoc />
        public bool Equals(T otherValue)
        {
            if (HasValue)
            {
                if (value is IEquatable<T> equatableValue)
                    return equatableValue.Equals(otherValue);
                return value.Equals(otherValue);
            }
            else
                return false;
        }

        /// <inheritdoc />
        public bool Equals(Maybe<T> otherMaybe)
        {
            if (HasValue)
                return otherMaybe.HasValue && value.Equals(otherMaybe.value);
            else
                return !otherMaybe.HasValue;
        }

        /// <inheritdoc />
        public override int GetHashCode() => HasValue ? (value?.GetHashCode() ?? 0) : 0;

        /// <summary>
        /// Retrieves the assigned underlying value of the <see cref="Maybe{T}"/> structure
        /// or the default value of the underlying type.
        /// </summary>
        /// <returns>
        /// The value of the <see cref="Value"/> property if the <see cref="HasValue"/>
        /// property is <c>true</c>; otherwise the default value of the underlying
        /// type.
        /// </returns>
        public T GetValueOrDefault() => GetValueOrDefault(default);

        /// <summary>
        /// Retrieves the assigned underlying value of the <see cref="Maybe{T}"/> structure
        /// or the specified default value.
        /// </summary>
        /// <param name="default">The value to return if the <see cref="HasValue"/> is <c>false</c>.</param>
        /// <returns>
        /// The value of the <see cref="Value"/> property if the <see cref="HasValue"/>
        /// property evaluates to <c>true</c>; otherwise the value of the <paramref name="default"/>
        /// parameter.
        /// </returns>
        public T GetValueOrDefault(T @default) => HasValue ? Value : @default;

        /// <inheritdoc />
        public override string ToString()
        {
            return HasValue
                ? value.ToString()
                : $"{nameof(Maybe<T>)}<{typeof(T)}>.{nameof(NoValue)}";
        }

        /// <seealso cref="Equals(Maybe{T})"/>
        public static bool operator ==(Maybe<T> left, Maybe<T> right)
            => left.Equals(right);

        /// <seealso cref="Equals(Maybe{T})"/>
        public static bool operator !=(Maybe<T> left, Maybe<T> right)
            => !left.Equals(right);

        /// <seealso cref="Equals(T)"/>
        public static bool operator ==(Maybe<T> maybe, T value)
            => maybe.Equals(value);

        /// <seealso cref="Equals(T)"/>
        public static bool operator ==(T value, Maybe<T> maybe)
            => maybe.Equals(value);

        /// <seealso cref="Equals(T)"/>
        public static bool operator !=(Maybe<T> maybe, T value)
            => !maybe.Equals(value);

        /// <seealso cref="Equals(T)"/>
        public static bool operator !=(T value, Maybe<T> maybe)
            => !maybe.Equals(value);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA2225 // Operator overloads have named alternates
        public static implicit operator Maybe<T>(T value)
            => new Maybe<T>(value);

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
