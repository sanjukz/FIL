using FIL.Logging.Targets;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;

namespace FIL.Logging
{
    public interface ILogFactory
    {
        ILogger GetLogger(Type t);

        ILogger GetLogger(string name);
    }

    public class LogFactory : ILogFactory
    {
        private readonly IConfiguration _configuration;

        public LogFactory(IConfiguration configuration)
        {
            _configuration = configuration;

            var loggingConfiguration = new LoggingConfiguration();

            AddTarget(loggingConfiguration, "file", _configuration["nlog:file"], GetFileTarget());
            AddTarget(loggingConfiguration, "sentry", _configuration["nlog:sentry"], GetSentryTarget());
            AddTarget(loggingConfiguration, "udp", _configuration["nlog:udp"], GetUdpTarget());

            LogManager.Configuration = loggingConfiguration;
            LogManager.ReconfigExistingLoggers();
#if DEBUG
            LogManager.ThrowExceptions = true;
#endif
        }

        public virtual ILogger GetLogger(Type t)
        {
            return new Logger(t);
        }

        public virtual ILogger GetLogger(string name)
        {
            return new Logger(name);
        }

        private void AddTarget(LoggingConfiguration config, string name, string level, Target target)
        {
            if (level != null)
            {
                var asyncEventTarget = WrapTargetWithAsync(target);
                config.AddTarget(name, asyncEventTarget);
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.FromString(level), asyncEventTarget));
            }
        }

        private static AsyncTargetWrapper WrapTargetWithAsync(Target target)
        {
            return new AsyncTargetWrapper
            {
                QueueLimit = 1000,
                OverflowAction = AsyncTargetWrapperOverflowAction.Discard,
                WrappedTarget = target
            };
        }

        private Target GetFileTarget()
        {
            return new FileTarget
            {
                FileName = "nlog.txt"
            };
        }

        private Target GetUdpTarget()
        {
            return new NetworkTarget
            {
                Address = "udp://127.0.0.1:49999"
            };
        }

        private Target GetSentryTarget()
        {
            return new SentryTarget
            {
                Dsn = _configuration["SENTRY_DSN"] ?? _configuration["nlogSentryUrl"],
                Layout = "${message}"
            };
        }
    }
}