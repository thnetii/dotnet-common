using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading;

using THNETII.Common;

namespace THNETII.Logging.EventSource
{
    public class LoggingEventSourceListener : EventListener
    {
        private static readonly ConcurrentDictionary<int, IConfiguration> configurations = new ConcurrentDictionary<int, IConfiguration>();
        private static readonly ConcurrentDictionary<int, ILoggerFactory> loggerFactories = new ConcurrentDictionary<int, ILoggerFactory>();
        private static int lastId = 0;
        private static readonly object ctorLock = new object();

        private readonly int id;
        private readonly ConcurrentDictionary<string, ILogger> loggers = new ConcurrentDictionary<string, ILogger>(StringComparer.OrdinalIgnoreCase);

        public static LoggingEventSourceListener Create(IConfiguration config, ILoggerFactory loggerFactory)
        {
            lock (ctorLock)
            {
                int newId = Interlocked.Increment(ref lastId);
                configurations[newId] = config;
                loggerFactories[newId] = loggerFactory;
                return new LoggingEventSourceListener(newId);
            }
        }

        private LoggingEventSourceListener(int id) : base()
        {
            this.id = id;
        }

        protected virtual void Dispose(bool disposing)
        {
            int id = Id;
            if (id != 0)
            {
                configurations.TryRemove(id, out _);
                loggerFactories.TryRemove(id, out _);
            }
        }

        public override void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~LoggingEventSourceListener()
        {
            Dispose(disposing: false);
        }

        private int Id => id != 0 ? id : lastId;

        private IConfiguration Configuration => configurations.TryGetValue(Id, out var config) ? config : default;

        private ILoggerFactory LoggerFactory => loggerFactories.TryGetValue(Id, out var loggerFactory) ? loggerFactory : default;

        protected override void OnEventSourceCreated(System.Diagnostics.Tracing.EventSource eventSource)
        {
            if (eventSource is null)
                return;
            var matchingConfigItem = Configuration?.GetSection("LogLevel")?
                .AsEnumerable(makePathsRelative: true).Select(configItem =>
            {
                if (Enum.TryParse(configItem.Value, out LogLevel logLevel) &&
                    (eventSource.Name?.Length ?? 0) > (configItem.Key?.Length ?? 0))
                {
                    var matchResult = new KeyValuePair<string, LogLevel>(configItem.Key, logLevel);
                    if (eventSource.Name.StartsWith(configItem.Key, StringComparison.OrdinalIgnoreCase))
                        return (configItem.Key, Value: logLevel, MatchClass: 0);
                    else
                    {
                        var nsName = eventSource.Name.Replace('-', '.');
                        if (nsName.StartsWith(configItem.Key, StringComparison.OrdinalIgnoreCase))
                            return (configItem.Key, Value: logLevel, MatchClass: 0);
                        var msNsName = "Microsoft." + configItem.Key;
                        if (nsName.StartsWith(msNsName, StringComparison.OrdinalIgnoreCase))
                            return (configItem.Key, Value: logLevel, MatchClass: 1);
                        if (string.Equals("Default", configItem.Key, StringComparison.OrdinalIgnoreCase))
                            return (configItem.Key, Value: logLevel, MatchClass: 2);
                    }
                }
                return (Key: null, Value: logLevel, MatchClass: int.MaxValue);
            }).Where(kvp => !(kvp.Key is null))
            .OrderBy(kvp => kvp.MatchClass).ThenByDescending(kvp => kvp.Key.Length)
            .FirstOrDefault();
            if (matchingConfigItem.HasValue && !string.IsNullOrWhiteSpace(matchingConfigItem.Value.Key))
            {
                EnableEvents(eventSource, matchingConfigItem.Value.Value.ToEventLevel());
            }
        }

        private ILogger CreateLogger(string category)
        {
            if (!string.IsNullOrWhiteSpace(category))
                return LoggerFactory?.CreateLogger(category);
            return null;
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData is null)
                return;
            var category = eventData.EventSource?.Name;
            var logger = loggers.GetOrAdd(category, CreateLogger);
            if (logger is null)
                return;

            var logLevel = eventData.Level.ToLogLevel();
            if (logger.IsEnabled(logLevel))
            {
                var logMessageFormat = new StringBuilder();
                var logMessageValues = new List<object>(9 + eventData.Payload.Count);
                if (!string.IsNullOrWhiteSpace(eventData.Message))
                {
                    logMessageFormat.Append("{" + nameof(eventData.Message) + "}");
                    logMessageValues.Add(eventData.Message);
                    var lastMessageChar = eventData.Message[eventData.Message.Length - 1];
                    if (char.IsPunctuation(lastMessageChar))
                        logMessageFormat.Append(' ');
                    else
                        logMessageFormat.Append("; ");
                }
                if (eventData.Payload.Count > 0)
                {
                    logMessageFormat.Append(nameof(eventData.Payload) + ": [");
                    logMessageFormat.Append(string.Join(", ", eventData.PayloadNames.Zip(eventData.Payload, (n, v) => (n, v)).Select((t, i) =>
                    {
                        (string name, object value) = t;
                        logMessageValues.Add(value);
                        if (string.IsNullOrWhiteSpace(name))
                            return FormattableString.Invariant($"{{Payload[{i}]}}");
                        return $"{name}: {{{name}}}";
                    })));
                    logMessageFormat.Append("]; ");
                }
                if (eventData.ActivityId != Guid.Empty)
                    LogMessageFormatAndAddValue(nameof(eventData.ActivityId), eventData.ActivityId);
                if (eventData.RelatedActivityId != Guid.Empty)
                    LogMessageFormatAndAddValue(nameof(eventData.RelatedActivityId), eventData.RelatedActivityId);
                LogMessageFormatAndAddValue(nameof(eventData.Channel), eventData.Channel);
                LogMessageFormatAndAddValue(nameof(eventData.Keywords), eventData.Keywords);
                LogMessageFormatAndAddValue(nameof(eventData.Opcode), eventData.Opcode);
                LogMessageFormatAndAddValue(nameof(eventData.Tags), eventData.Tags);
                LogMessageFormatAndAddValue(nameof(eventData.Task), eventData.Task);
                LogMessageFormatAndAddValue(nameof(eventData.Version), eventData.Version, final: true);

                logger.Log(logLevel,
                    new EventId(eventData.EventId, eventData.EventName),
                    new FormattedLogValues(logMessageFormat.ToString(), logMessageValues.ToArray()),
                    exception: null,
                    LogMessageFormatter
                    );

                void LogMessageFormatAndAddValue(string name, object value, bool final = false)
                {
                    logMessageFormat.Append($"{name}: {{{name}}}");
                    if (!final)
                        logMessageFormat.Append("; ");
                    logMessageValues.Add(value);
                }
            }
        }

        private static string LogMessageFormatter(object instance, Exception exception)
        {
            var logMessage = (instance?.ToString())
                .NotNullOrWhiteSpace(otherwise: string.Empty);
            if (exception is Exception)
            {
                return string.Join(Environment.NewLine,
                    logMessage,
                    exception.GetType().FullName,
                    exception.ToString()
                    );
            }
            return logMessage;
        }
    }
}
