using FIL.Contracts.Exceptions;
using FIL.Logging.Enums;
using FIL.Logging.Models;
using Newtonsoft.Json;
using NLog;
using NLog.Common;
using System;
using System.Collections.Generic;

namespace FIL.Logging
{
    public interface ILogger
    {
        Guid Log(LogCategory logCategory, string message, Dictionary<string, object> variables = null);

        Guid Log(LogCategory logCategory, Exception ex);

        Guid Log(ILoggable loggable);

        void Flush();
    }

    public class Logger : ILogger
    {
        private readonly NLog.Logger _logger;

        public Logger(Type type)
        {
            _logger = LogManager.GetLogger(type.AssemblyQualifiedName);
        }

        public Logger(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        public virtual Guid Log(LogCategory logCategory, string message, Dictionary<string, object> variables = null)
        {
            return Log(new Loggable
            {
                LogCategory = logCategory,
                Message = message,
                CustomVariables = variables
            });
        }

        public virtual Guid Log(LogCategory logCategory, Exception ex)
        {
            string message = string.Empty;
            if (!string.IsNullOrWhiteSpace(ex?.Message))
            {
                message = ex.Message;
            }

            return Log(new Loggable
            {
                LogCategory = logCategory,
                Message = message,
                Exception = ex
            });
        }

        public Guid Log(ILoggable loggable)
        {
            if (loggable.LogGuid == Guid.Empty)
            {
                loggable.LogGuid = Guid.NewGuid();
            }

            try
            {
                var logEvent = MapToLogEvent(loggable);
                _logger.Log(typeof(Logger), logEvent);
            }
            catch
            {
                // If logger throws then log to event log as a failover option.
                InternalLogger.Error("Failover Logging: Failed to log item [{0}].", loggable.Message);
#if DEBUG
                throw;
#endif
            }

            return loggable.LogGuid;
        }

        public void Flush()
        {
            LogManager.Flush();
        }

        private LogEventInfo MapToLogEvent(ILoggable loggable)
        {
            var customEx = loggable.Exception as CustomException;

            var logEventInfo = new LogEventInfo(MapLogLevel(loggable.LogCategory), _logger.Name, loggable.Message)
            {
                Exception = loggable.Exception
            };
            logEventInfo.Properties["LogGuid"] = loggable.LogGuid;

            if (loggable.SessionId.HasValue)
            {
                logEventInfo.Properties["SessionId"] = loggable.SessionId.Value;
            }

            if (loggable.UserId.HasValue)
            {
                logEventInfo.Properties["UserId"] = loggable.UserId.Value;
            }

            IDictionary<string, object> extendedProperties = new Dictionary<string, object>();

            if (customEx?.Variables != null)
            {
                foreach (var property in customEx.Variables)
                {
                    extendedProperties.Add(property.Key, property.Value);
                }
            }

            if (loggable.CustomVariables != null)
            {
                foreach (var property in loggable.CustomVariables)
                {
                    extendedProperties.Add(property.Key, property.Value);
                }
            }

            foreach (var property in extendedProperties)
            {
                logEventInfo.Properties[property.Key] = property.Value.ToString();
            }

            logEventInfo.Properties["CustomVariables"] = JsonConvert.SerializeObject(extendedProperties);

            return logEventInfo;
        }

        private LogLevel MapLogLevel(LogCategory logCategory)
        {
            switch (logCategory)
            {
                case LogCategory.Debug:
                    return LogLevel.Debug;

                case LogCategory.Error:
                    return LogLevel.Error;

                case LogCategory.Fatal:
                    return LogLevel.Fatal;

                case LogCategory.Info:
                    return LogLevel.Info;

                case LogCategory.Trace:
                    return LogLevel.Trace;

                case LogCategory.Warn:
                    return LogLevel.Warn;

                default:
                    return LogLevel.Info;
            }
        }
    }
}