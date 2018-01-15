using Microsoft.Extensions.Logging;
using System.Diagnostics.Tracing;

namespace THNETII.Logging.EventSource
{
    public static class EventLevelExtensions
    {
        public static LogLevel ToLogLevel(this EventLevel eventLevel)
        {
            switch (eventLevel)
            {
                case EventLevel.Verbose: return LogLevel.Debug;
                case EventLevel.Informational: return LogLevel.Information;
                case EventLevel.Warning: return LogLevel.Warning;
                case EventLevel.Error: return LogLevel.Error;
                case EventLevel.Critical: return LogLevel.Critical;
                case EventLevel.LogAlways: return LogLevel.None;
                default:
                    return (LogLevel)((int)LogLevel.None - (int)eventLevel);
            }
        }

        public static EventLevel ToEventLevel(this LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug: return EventLevel.Verbose;
                case LogLevel.Information: return EventLevel.Informational;
                case LogLevel.Warning: return EventLevel.Warning;
                case LogLevel.Error: return EventLevel.Error;
                case LogLevel.Critical: return EventLevel.Critical;
                case LogLevel.None: return EventLevel.LogAlways;
                default:
                    return (EventLevel)((int)LogLevel.None - (int)logLevel);
            }
        }
    }
}
