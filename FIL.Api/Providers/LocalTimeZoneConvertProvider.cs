using FIL.Configuration;
using System;
using System.Linq;

namespace FIL.Api.Providers
{
    public interface ILocalTimeZoneConvertProvider
    {
        DateTime ConvertToLocal(DateTime startDateTime, string timeZoneOffset);
    }

    public class LocalTimeZoneConvertProvider : ILocalTimeZoneConvertProvider
    {
        private ISettings _settings;
        private readonly FIL.Logging.ILogger _logger;

        public LocalTimeZoneConvertProvider(
            FIL.Logging.ILogger logger,
            ISettings settings)
        {
            _settings = settings;
            _logger = logger;
        }

        public DateTime ConvertToLocal(DateTime startDateTime, string timeZoneOffset)
        {
            try
            {
                if (timeZoneOffset.Contains('+') || !timeZoneOffset.Contains('-'))
                {
                    var minutes = Convert.ToDouble(timeZoneOffset.Replace("+", ""));
                    startDateTime = startDateTime.AddMinutes(minutes);
                }
                else
                {
                    var minutes = Convert.ToDouble(timeZoneOffset.Replace("-", ""));
                    startDateTime = startDateTime.AddMinutes(-minutes);
                }
                return startDateTime;
            }
            catch (Exception e)
            {
                _logger.Log(FIL.Logging.Enums.LogCategory.Error, e);
                return startDateTime;
            }
        }
    }
}