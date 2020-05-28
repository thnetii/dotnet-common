using System;

namespace THNETII.Common
{
    /// <summary>
    /// Encapsulates a method that has no parameters and returns a reference to
    /// a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    /// <seealso cref="Func{TResult}"/>
    /// <seealso cref="RefReadOnlyFunc{TResult}"/>
    public delegate ref TResult RefFunc<TResult>();
}
