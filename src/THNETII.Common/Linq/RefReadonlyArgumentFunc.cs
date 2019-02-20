using System;

namespace THNETII.Common.Linq
{
    /// <summary>
    /// Encapsulates a method that has one ref-readonly parameter and
    /// returns a value of the type specified by the <typeparamref name="TResult"/>
    /// parameter.
    /// </summary>
    /// <typeparam name="T1">
    /// The type of the parameter of the method that this delegate encapsulates.
    /// Unlike the typeparameter in <see cref="Func{T, TResult}"/> this typeparameter
    /// is invariant in order to be able to accept the parameter using the ref readonly.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the return value of the method that this delegate encapsulates.
    /// This type parameter is contravariant.
    /// </typeparam>
    /// <param name="arg1"></param>
    /// <returns></returns>
    /// <seealso cref="Func{T1, TResult}"/>
    public delegate TResult RefReadonlyArgumentFunc<T1, out TResult>(in T1 arg1);
}
