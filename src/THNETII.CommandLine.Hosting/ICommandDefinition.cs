using System.CommandLine;

namespace THNETII.CommandLine.Hosting
{
    /// <summary>
    /// The base interface for an application specific definition of a
    /// command-line root command.
    /// </summary>
    public interface ICommandDefinition
    {
        /// <summary>
        /// Gets the root command for the application specific command-line.
        /// </summary>
        Command Command { get; }
    }
}
