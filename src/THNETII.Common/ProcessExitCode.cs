namespace THNETII.Common
{
    /// <summary>
    /// Convenience type that provides exit code constants for processes.
    /// </summary>
    /// <remarks>The exit codes defined in this class are equivalent to the exit codes defined in an ISO conforming native C runtime library.</remarks>
    public static class ProcessExitCode
    {
        /// <summary>
        /// A process exit code that indicates that the executing program completed successfully.
        /// </summary>
        /// <remarks>
        /// By convention, a return code of zero means that the program completed successfully. 
        /// <para>This constant mirrors the <c>EXIT_SUCCESS</c> constant commonly defined in the <c>stdlib.h</c> header file of a native C runtime library.</para>
        /// </remarks>
        public const int ExitSuccess = 0;

        /// <summary>
        /// A process exit code that indicates that the executing program terminated abnormally because of an error condition.
        /// </summary>
        /// <remarks>
        /// <para>This constant mirrors the <c>EXIT_FAILURE</c> constant commonly defined in the <c>stdlib.h</c> header file of a native C runtime library.</para>
        /// </remarks>
        public const int ExitFailure = 1;
    }
}
