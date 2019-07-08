using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using System;
using System.Collections;
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
                configurations.TryRemove(id, out var config);
                loggerFactories.TryRemove(id, out var logger);

                logger?.Dispose();
            }
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~LoggingEventSourceListener() => Dispose(false);

        private int Id => id != 0 ? id : lastId;

        private IConfiguration Configuration
        {
            get
            {
                if (configurations.TryGetValue(Id, out var config))
                    return config;
                return default;
            }
        }

        private ILoggerFactory LoggerFactory
        {
            get
            {
                if (loggerFactories.TryGetValue(Id, out var loggerFactory))
                    return loggerFactory;
                return default;
            }
        }

        protected override void OnEventSourceCreated(System.Diagnostics.Tracing.EventSource eventSource)
        {
            if (eventSource is null)
                return;
            var matchingConfigItem = Configuration?.GetSection("LogLevel")?.AsEnumerable(makePathsRelative: true).Select(configItem =>
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
                    logMessageFormat.Append(string.Join(", ", eventData.PayloadNames.Zip(eventData.Payload, (n, v) => new KeyValuePair<string, object>(n, v)).Select((kvp, i) =>
                    {
                        logMessageValues.Add(kvp.Value);
                        if (string.IsNullOrWhiteSpace(kvp.Key))
                            return "{Payload[" + i.ToString(System.Globalization.CultureInfo.InvariantCulture) + "]}";
                        return kvp.Key + ": {" + kvp.Key + "}";
                    })));
                    logMessageFormat.Append("]; ");
                }
                if (eventData.ActivityId != Guid.Empty)
                {
                    logMessageFormat.Append(nameof(eventData.ActivityId) + ": {" + nameof(eventData.ActivityId) + "}; ");
                    logMessageValues.Add(eventData.ActivityId);
                }
                if (eventData.RelatedActivityId != Guid.Empty)
                {
                    logMessageFormat.Append(nameof(eventData.RelatedActivityId) + ": {" + nameof(eventData.RelatedActivityId) + "}; ");
                    logMessageValues.Add(eventData.RelatedActivityId);
                }
                logMessageFormat.Append(nameof(eventData.Channel) + ": {" + nameof(eventData.Channel) + "}; ");
                logMessageValues.Add(eventData.Channel);
                logMessageFormat.Append(nameof(eventData.Keywords) + ": {" + nameof(eventData.Keywords) + "}; ");
                logMessageValues.Add(eventData.Keywords);
                logMessageFormat.Append(nameof(eventData.Opcode) + ": {" + nameof(eventData.Opcode) + "}; ");
                logMessageValues.Add(eventData.Opcode);
                logMessageFormat.Append(nameof(eventData.Tags) + ": {" + nameof(eventData.Tags) + "}; ");
                logMessageValues.Add(eventData.Tags);
                logMessageFormat.Append(nameof(eventData.Task) + ": {" + nameof(eventData.Task) + "}; ");
                logMessageValues.Add(eventData.Task);
                logMessageFormat.Append(nameof(eventData.Version) + ": {" + nameof(eventData.Version) + "}");
                logMessageValues.Add(eventData.Version);

                logger.Log(logLevel,
                    new EventId(eventData.EventId, eventData.EventName),
                    new FormattedLogValues(logMessageFormat.ToString(), logMessageValues.ToArray()),
                    null,
                    LogMessageFormatter
                    );
            }
        }

        private static string LogMessageFormatter(object instance, Exception exception)
        {
            var logMessage = (instance?.ToString()).NotNullOrWhiteSpace(otherwise: string.Empty);
            if (!(exception is null))
            {
                if (logMessage.Length > 0)
                    logMessage += Environment.NewLine;
                logMessage += exception.ToString();
            }
            return logMessage;
        }
    }
}
