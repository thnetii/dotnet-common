using System;
using System.Diagnostics;

namespace THNETII.Diagnostics.DiagnosticSourceExtensions
{
    /// <summary>
    /// A wrapper around <see cref="DiagnosticSource.Write(string, object)"/> allowing
    /// extensions to condense the <c>if</c>-block combining calls to
    /// <see cref="DiagnosticSource.IsEnabled(string)"/> and <see cref="DiagnosticSource.Write(string, object)"/>.
    /// </summary>
    public readonly struct DiagnosticSourceWriter : IEquatable<DiagnosticSourceWriter>
    {
        private readonly DiagnosticSource diagnosticSource;

        /// <summary>
        /// Gets the name of the event being written.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a <see cref="DiagnosticSourceWriter"/> using the specified
        /// <see cref="DiagnosticSource"/> as the logger target and the specified
        /// name as the event name being logged.
        /// </summary>
        /// <param name="diagnosticSource">The <see cref="DiagnosticSource"/> to write to. Can be <see langword="null"/>.</param>
        /// <param name="name">The name of the event being written.</param>
        public DiagnosticSourceWriter(DiagnosticSource diagnosticSource, string name)
        {
            this.diagnosticSource = diagnosticSource;
            Name = name;
        }

        /// <summary>
        /// Calls <see cref="DiagnosticSource.Write(string, object)"/> on the
        /// <see cref="DiagnosticSource"/> instance this <see cref="DiagnosticSourceWriter"/>
        /// was initialized to, using <see cref="Name"/> as the first argument and
        /// the specified <see cref="object"/> as parameters.
        /// </summary>
        /// <param name="value">An object that represent the value being passed as a payload for the event.
        /// This is often an anonymous type which contains several sub-values.</param>
        /// <remarks>
        /// If <see cref="DiagnosticSourceWriter"/> was initialized with a <see langword="null"/> <see cref="DiagnosticSource"/>,
        /// this operation is a no-op.
        /// </remarks>
        /// <seealso cref="DiagnosticSource.Write(string, object)"/>
        public void Write(object value) => diagnosticSource?.Write(Name, value);

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case DiagnosticSourceWriter other:
                    return Equals(other);
                default:
                case null:
                    return false;
            }
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode() => diagnosticSource?.GetHashCode() ?? 0;

        /// <inheritdoc cref="M:System.ValueType.op_Equality(System.ValueType,System.ValueType)"/>
        public static bool operator ==(DiagnosticSourceWriter left, DiagnosticSourceWriter right) =>
            left.Equals(right);

        /// <inheritdoc cref="M:System.ValueType.op_Inequality(System.ValueType,System.ValueType)"/>
        public static bool operator !=(DiagnosticSourceWriter left, DiagnosticSourceWriter right) =>
            !left.Equals(right);

        /// <inheritdoc cref="IEquatable{DiagnosticSourceWriter}.Equals(DiagnosticSourceWriter)"/>
        public bool Equals(DiagnosticSourceWriter other) =>
            (diagnosticSource?.Equals(other.diagnosticSource) ?? false) &&
            string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }
}
