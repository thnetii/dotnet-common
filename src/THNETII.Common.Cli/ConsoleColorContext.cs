using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace THNETII.Common.Cli
{
    /// <summary>
    /// Contains the state of current and original Console foreground and background color settings.
    /// <para>The context should be used in a <c>using</c>-block to ensure proper resetting of console colors.</para>
    /// </summary>
    /// <example>
    /// <code lang="CSharp">
    /// using System;
    /// using THNETII.Common.Cli;
    ///
    /// class Program
    /// {
    ///     static void Main(string[] args)
    ///     {
    ///         Console.WriteLine("This text is written normally.");
    ///         using (var context = new ConsoleColorContext(ConsoleColor.Red))
    ///         {
    ///             Console.WriteLine("This text is written using red foreground coloring.");
    ///         }
    ///         Console.WriteLine("This text is written normally again.");
    ///     }
    /// }
    /// </code>
    /// </example>
    public sealed class ConsoleColorContext : IDisposable
    {
        private int disposed;

        private readonly ConsoleColor? fgColor;
        private readonly ConsoleColor? bgColor;
        private readonly ConsoleColor? fgColorOriginal;
        private readonly ConsoleColor? bgColorOriginal;

        /// <summary>
        /// Whether this context is configured to set <see cref="Console.ForegroundColor"/> when <see cref="Set"/> is invoked.
        /// </summary>
        /// <value><c>true</c> if the context will set the foreground color; otherwise, <c>false</c>.</value>
        public bool ChangeForegroundColor => fgColor.HasValue;

        /// <summary>
        /// The foreground color to use while this context is active.
        /// </summary>
        /// <value>
        /// <para>
        /// If <see cref="ChangeForegroundColor"/> is <c>true</c>, the
        /// property value is the <see cref="ConsoleColor"/> value that is set
        /// as the foreground console color when <see cref="Set"/> is called.
        /// </para>
        /// <para>
        /// If <see cref="ChangeForegroundColor"/> is <c>false</c>, the current
        /// value of <see cref="Console.ForegroundColor"/> is returned.
        /// </para>
        /// </value>
        /// <exception cref="System.Security.SecurityException">The user does not have permission to get the current value of <see cref="Console.ForegroundColor"/>.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred while attempting to read the current value of <see cref="Console.ForegroundColor"/>.</exception>
        public ConsoleColor ForegroundColor => fgColor ?? Console.ForegroundColor;

        /// <summary>
        /// Whether this context is configured to reset <see cref="Console.ForegroundColor"/> to its original value when <see cref="Reset"/> is invoked.
        /// </summary>
        /// <value><c>true</c> if the context will reset the foreground color; otherwise, <c>false</c>.</value>
        public bool ResetForegroundColor => fgColorOriginal.HasValue;

        /// <summary>
        /// The foreground color to reset to when this context is disposed.
        /// </summary>
        /// <value>
        /// <para>
        /// If <see cref="ResetForegroundColor"/> is <c>true</c>, the
        /// property value is the original <see cref="ConsoleColor"/> value to which
        /// the foreground console color will be reset when <see cref="Reset"/> is called.
        /// </para>
        /// <para>
        /// If <see cref="ResetForegroundColor"/> is <c>false</c>, the current
        /// value of <see cref="Console.ForegroundColor"/> is returned.
        /// </para>
        /// </value>
        /// <exception cref="System.Security.SecurityException">The user does not have permission to get the current value of <see cref="Console.ForegroundColor"/>.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred while attempting to read the current value of <see cref="Console.ForegroundColor"/>.</exception>
        public ConsoleColor OriginalForegroundColor => fgColorOriginal ?? Console.ForegroundColor;

        /// <summary>
        /// Whether calling <see cref="Set"/> will change set the value of <see cref="Console.BackgroundColor"/>.
        /// </summary>
        /// <value><c>true</c> if the context will set the foreground color; otherwise, <c>false</c>.</value>
        public bool ChangeBackgroundColor => bgColor.HasValue;

        /// <summary>
        /// The background color to use while this context is active.
        /// </summary>
        /// <value>
        /// <para>
        /// If <see cref="ChangeBackgroundColor"/> is <c>true</c>, the
        /// property value is the <see cref="ConsoleColor"/> value that is set
        /// as the background console color when <see cref="Set"/> is called.
        /// </para>
        /// <para>
        /// If <see cref="ChangeBackgroundColor"/> is <c>false</c>, the current
        /// value of <see cref="Console.BackgroundColor"/> is returned.
        /// </para>
        /// </value>
        /// <exception cref="System.Security.SecurityException">The user does not have permission to get the current value of <see cref="Console.BackgroundColor"/>.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred while attempting to read the current value of <see cref="Console.BackgroundColor"/>.</exception>
        public ConsoleColor BackgroundColor => bgColor ?? Console.BackgroundColor;

        /// <summary>
        /// Whether this context is configured to reset <see cref="Console.BackgroundColor"/> to its original value when <see cref="Reset"/> is invoked.
        /// </summary>
        /// <value><c>true</c> if the context will reset the foreground color; otherwise, <c>false</c>.</value>
        public bool ResetBackgroundColor => bgColorOriginal.HasValue;

        /// <summary>
        /// The background color to reset to when this context is disposed.
        /// </summary>
        /// <value>
        /// <para>
        /// If <see cref="ResetForegroundColor"/> is <c>true</c>, the
        /// property value is the original <see cref="ConsoleColor"/> value to which
        /// the background console color will be reset when <see cref="Reset"/> is called.
        /// </para>
        /// <para>
        /// If <see cref="ResetForegroundColor"/> is <c>false</c>, the current
        /// value of <see cref="Console.BackgroundColor"/> is returned.
        /// </para>
        /// </value>
        /// <exception cref="System.Security.SecurityException">The user does not have permission to get the current value of <see cref="Console.ForegroundColor"/>.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred while attempting to read the current value of <see cref="Console.ForegroundColor"/>.</exception>
        public ConsoleColor OriginalBackgroundColor => bgColorOriginal ?? Console.BackgroundColor;

        /// <summary>
        /// Initializes a new Console Color context with the specified <see cref="ConsoleColor"/> values,
        /// and optionally applies the requested colors immediately.
        /// </summary>
        /// <param name="fgColor">The foreground console color to set, or <c>null</c> if the foreground color should not be modified.</param>
        /// <param name="bgColor">The background console color to set, or <c>null</c> if the background color should not be modified.</param>
        /// <param name="fgColorOriginal">The original foregorund console color to reset to when the context is disposed, or <c>null</c> if the foreground color should not be reset.</param>
        /// <param name="bgColorOriginal">The original background console color to reset to when the context is disposed, or <c>null</c> if the background color should not be reset.</param>
        /// <param name="set">If <c>true</c> (default), the context will be applied by calling <see cref="Set"/> immediately. If <c>false</c>, <see cref="Set"/> must be invoked manually at an appropiate time.</param>
        /// <exception cref="InvalidOperationException">
        /// The color specified for either the foreground color or the background color is not a valid member of <see cref="ConsoleColor"/>.
        /// The <see cref="Exception.InnerException"/> property contains the <see cref="ArgumentException"/> thrown from the set invocation to <see cref="Console.ForegroundColor"/> or <see cref="Console.BackgroundColor"/>.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">The user does not have permission to set the current value of either <see cref="Console.ForegroundColor"/> or <see cref="Console.BackgroundColor"/>.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred while attempting to write to the current value of either <see cref="Console.ForegroundColor"/> or <see cref="Console.BackgroundColor"/>.</exception>
        public ConsoleColorContext(
            ConsoleColor? fgColor,
            ConsoleColor? bgColor,
            ConsoleColor? fgColorOriginal,
            ConsoleColor? bgColorOriginal,
            bool set = true
            ) : base()
        {
            this.fgColor = fgColor;
            this.bgColor = bgColor;
            this.fgColorOriginal = fgColorOriginal;
            this.bgColorOriginal = bgColorOriginal;

            if (set)
                Set();
        }

        /// <summary>
        /// Initializes a new Console Color context with the specified <see cref="ConsoleColor"/> values,
        /// applying the requested colors immediately and configuring the context to reset the colors on disposal.
        /// </summary>
        /// <param name="fgColor">The foreground console color to set, or <c>null</c> if the foreground color should not be modified.</param>
        /// <param name="bgColor">The background console color to set, or <c>null</c> if the background color should not be modified.</param>
        /// <exception cref="System.Security.SecurityException">The user does not have permission to get or set the current value of either <see cref="Console.ForegroundColor"/> or <see cref="Console.BackgroundColor"/>.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred while attempting to read from or write to the current value of either <see cref="Console.ForegroundColor"/> or <see cref="Console.BackgroundColor"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// The color specified for either the foreground color or the background color is not a valid member of <see cref="ConsoleColor"/>.
        /// The <see cref="Exception.InnerException"/> property contains the <see cref="ArgumentException"/> thrown from the set invocation to <see cref="Console.ForegroundColor"/> or <see cref="Console.BackgroundColor"/>.
        /// </exception>
        public ConsoleColorContext(ConsoleColor? fgColor = default, ConsoleColor? bgColor = default)
            : this(fgColor, bgColor, fgColor.HasValue ? Console.ForegroundColor : default, bgColor.HasValue ? Console.BackgroundColor : default)
        { }

        /// <summary>
        /// Sets the foreground and background console colors according to values
        /// configured in the context.
        /// </summary>
        /// <remarks>This method can be called multiple times.</remarks>
        /// <exception cref="InvalidOperationException">
        /// The color specified for either the foreground color or the background color is not a valid member of <see cref="ConsoleColor"/>.
        /// The <see cref="Exception.InnerException"/> property contains the <see cref="ArgumentException"/> thrown from the set invocation to <see cref="Console.ForegroundColor"/> or <see cref="Console.BackgroundColor"/>.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">The user does not have permission to set the current value of either <see cref="Console.ForegroundColor"/> or <see cref="Console.BackgroundColor"/>.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred while attempting to write to the current value of either <see cref="Console.ForegroundColor"/> or <see cref="Console.BackgroundColor"/>.</exception>
        /// <seealso cref="ChangeForegroundColor"/>
        /// <seealso cref="ForegroundColor"/>
        /// <seealso cref="ChangeBackgroundColor"/>
        /// <seealso cref="BackgroundColor"/>
        public void Set()
        {
            if (fgColor.HasValue)
            {
                try { Console.ForegroundColor = fgColor.Value; }
                catch (ArgumentException argExcept)
                {
                    throw new InvalidOperationException(argExcept.Message, argExcept);
                }
            }
            if (bgColor.HasValue)
            {
                try { Console.BackgroundColor = bgColor.Value; }
                catch (ArgumentException argExcept)
                {
                    throw new InvalidOperationException(argExcept.Message, argExcept);
                }
            }
        }

        /// <summary>
        /// Resets the foreground and background console colors to the orginal values
        /// configured in the context.
        /// </summary>
        /// <remarks>This method can be called multiple times.</remarks>
        /// <exception cref="InvalidOperationException">
        /// The color specified for either the foreground color or the background color is not a valid member of <see cref="ConsoleColor"/>.
        /// The <see cref="Exception.InnerException"/> property contains the <see cref="ArgumentException"/> thrown from the set invocation to <see cref="Console.ForegroundColor"/> or <see cref="Console.BackgroundColor"/>.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">The user does not have permission to set the current value of either <see cref="Console.ForegroundColor"/> or <see cref="Console.BackgroundColor"/>.</exception>
        /// <exception cref="System.IO.IOException">An I/O error occurred while attempting to write to the current value of either <see cref="Console.ForegroundColor"/> or <see cref="Console.BackgroundColor"/>.</exception>
        /// <seealso cref="ResetForegroundColor"/>
        /// <seealso cref="OriginalForegroundColor"/>
        /// <seealso cref="ResetBackgroundColor"/>
        /// <seealso cref="OriginalBackgroundColor"/>
        public void Reset()
        {
            if (fgColorOriginal.HasValue)
                Console.ForegroundColor = fgColorOriginal.Value;
            if (bgColorOriginal.HasValue)
                Console.BackgroundColor = bgColorOriginal.Value;
        }

        /// <summary>
        /// Calls <see cref="Reset"/>, but is guaranteed to be executed only
        /// once. Subsequent calls to <see cref="Dispose"/> will have no effect.
        /// </summary>
        /// <seealso cref="Reset"/> 
        public void Dispose()
        {
            DisposeImpl();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object at garbage collection. If the context has already
        /// been disposed, this finalizer has no effect.
        /// </summary>
        [SuppressMessage("Performance", "CA1821: Remove empty Finalizers")]
        ~ConsoleColorContext() => DisposeImpl();

        private void DisposeImpl()
        {
            if (Interlocked.Exchange(ref disposed, 1) != 0)
                return;

            Reset();
        }
    }
}
