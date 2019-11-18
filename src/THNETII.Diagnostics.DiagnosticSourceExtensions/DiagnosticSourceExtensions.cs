using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace THNETII.Diagnostics.DiagnosticSourceExtensions
{
    /// <summary>
    /// Provides extension methods to implementations of the <see cref="DiagnosticSource"/> class.
    /// </summary>
    [SuppressMessage("Naming", "CA1724: The type name conflicts in whole or in part with the namespace name.", Justification = "Static extensions member class")]
    public static class DiagnosticSourceExtensions
    {
        /// <summary>
        /// Returns a nullable wrapper around the <see cref="DiagnosticSource"/>
        /// if the call to <see cref="DiagnosticSource.IsEnabled(string)"/> with
        /// the specified name returns <see langword="true"/>.
        /// <para>Use the null-conditional (<c>?.</c>) operator on the return value to call <see cref="DiagnosticSourceWriter.Write(object)"/>.</para>
        /// </summary>
        /// <param name="diagnosticSource">The diagnostic source to write to. Can be <see langword="null"/>.</param>
        /// <param name="name">The name of the event to write.</param>
        /// <returns>
        /// A <see cref="DiagnosticSourceWriter"/> using <paramref name="diagnosticSource"/> and <paramref name="name"/>.
        /// If <paramref name="diagnosticSource"/> is <see langword="null"/>, or if calling <see cref="DiagnosticSource.IsEnabled(string)"/> on <paramref name="diagnosticSource"/>
        /// with <paramref name="name"/> returns <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// This extension method serves as a convenience method to condense the otherwise recommended <c>if</c>-block for logging to a <see cref="DiagnosticSource"/> instance.
        /// <para>If <paramref name="diagnosticSource"/> is <see langword="null"/>, <see langword="null"/> is returned.</para>
        /// </remarks>
        public static DiagnosticSourceWriter? IfEnabled(this DiagnosticSource? diagnosticSource, string name)
        {
            if (diagnosticSource?.IsEnabled(name) ?? false == false)
                return null;
            return new DiagnosticSourceWriter(diagnosticSource, name);
        }

        /// <summary>
        /// Returns a nullable wrapper around the <see cref="DiagnosticSource"/>
        /// if the call to <see cref="DiagnosticSource.IsEnabled(string, object, object)"/> with
        /// the specified name returns <see langword="true"/>.
        /// <para>Use the null-conditional (<c>?.</c>) operator on the return value to call <see cref="DiagnosticSourceWriter.Write(object)"/>.</para>
        /// </summary>
        /// <param name="diagnosticSource">The diagnostic source to write to. Can be <see langword="null"/>.</param>
        /// <param name="name">The name of the event to write.</param>
        /// <param name="arg1">An object that represents the additional context for <see cref="DiagnosticSource.IsEnabled(string, object, object)"/>.</param>
        /// <param name="arg2">Optional. An object that represents the additional context for <see cref="DiagnosticSource.IsEnabled(string, object, object)"/>. <see langword="null"/> by default.</param>
        /// <returns>
        /// A <see cref="DiagnosticSourceWriter"/> using <paramref name="diagnosticSource"/> and <paramref name="name"/>.
        /// If <paramref name="diagnosticSource"/> is <see langword="null"/>, or if calling <see cref="DiagnosticSource.IsEnabled(string, object, object)"/> on <paramref name="diagnosticSource"/>
        /// with <paramref name="name"/> returns <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// This extension method serves as a convenience method to condense the otherwise recommended <c>if</c>-block for logging to a <see cref="DiagnosticSource"/> instance.
        /// <para>If <paramref name="diagnosticSource"/> is <see langword="null"/>, <see langword="null"/> is returned.</para>
        /// </remarks>
        /// <seealso cref="IfEnabled(DiagnosticSource, string)"/>
        public static DiagnosticSourceWriter? IfEnabled(this DiagnosticSource? diagnosticSource, string name, object arg1, object? arg2 = null)
        {
            if (diagnosticSource?.IsEnabled(name, arg1, arg2) ?? false == false)
                return null;
            return new DiagnosticSourceWriter(diagnosticSource, name);
        }
    }
}
