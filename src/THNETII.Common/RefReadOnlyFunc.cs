using System;

namespace THNETII.Common;

/// <summary>
/// Encapsulates a method that has no parameters and returns a read-only reference to
/// a value of the type specified by the <typeparamref name="TResult"/> parameter.
/// </summary>
/// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
/// <returns>A read-only reference to the return value of the method that this delegate encapsulates.</returns>
/// <seealso cref="Func{TResult}"/>
/// <seealso cref="RefFunc{TResult}"/>
public delegate ref readonly TResult RefReadOnlyFunc<TResult>();
