using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;
using SharpRaven.Core;
using SharpRaven.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Logging.Targets
{
    [Target("Sentry")]
    public class SentryTarget : TargetWithLayout
    {
        private Dsn _dsn;
        private readonly Lazy<IRavenClient> _client;

        private static string[] _tags = { "Url", "SessionId", "UserId" };

        /// <summary>
        /// Map of NLog log levels to Raven/Sentry log levels
        /// </summary>
        protected static readonly IDictionary<LogLevel, ErrorLevel> LoggingLevelMap = new Dictionary<LogLevel, ErrorLevel>
        {
            {LogLevel.Debug,    ErrorLevel.Debug},
            {LogLevel.Error,    ErrorLevel.Error},
            {LogLevel.Fatal,    ErrorLevel.Fatal},
            {LogLevel.Info,     ErrorLevel.Info},
            {LogLevel.Trace,    ErrorLevel.Debug},
            {LogLevel.Warn,     ErrorLevel.Warning}
        };

        /// <summary>
        /// The DSN for the Sentry host
        /// </summary>
        [RequiredParameter]
        public string Dsn
        {
            get { return _dsn?.ToString(); }
            set { _dsn = new Dsn(value); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SentryTarget()
        {
            _client = new Lazy<IRavenClient>(() => new RavenClient(Dsn));
        }

        /// <summary>
        /// Writes logging event to the log target.
        /// </summary>
        /// <param name="logEvent">Logging event to be written out.</param>
        protected override void Write(LogEventInfo logEvent)
        {
            try
            {
                _client.Value.Logger = logEvent.LoggerName;

                var properties = logEvent.Properties.Where(p => p.Key.ToString() != "CustomVariables").GroupBy(p => _tags.Contains(p.Key.ToString()));
                var extras = properties.Where(p => !p.Key).SelectMany(p => p).ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());
                var tags = properties.Where(p => p.Key).SelectMany(p => p).ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());

                // If the log event did not contain an exception then we'll send a "Message" to Sentry
                if (logEvent.Exception == null)
                {
                    _client.Value.CaptureAsync(new SentryEvent(logEvent.FormattedMessage)
                    {
                        Level = LoggingLevelMap[logEvent.Level],
                        Extra = extras
                    }).RunSynchronously();
                }
                else if (logEvent.Exception != null)
                {
                    _client.Value.CaptureAsync(new SentryEvent(logEvent.Exception)
                    {
                        Level = LoggingLevelMap[logEvent.Level],
                        Extra = extras
                    }).RunSynchronously();
                }
            }
            catch (Exception ex)
            {
                InternalLogger.Error("SentryTarget Failover Logging Message: [{0}].", ex.Message);
            }
        }
    }
}