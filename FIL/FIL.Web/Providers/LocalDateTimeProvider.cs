using FIL.Logging;
using FIL.Configuration;
using System;
using System.Linq;
using static FIL.Web.Feel.Controllers.PlaceItinerarySearchController;
using System.Text.RegularExpressions;

namespace FIL.Web.Feel.Providers
{
    public interface ILocalDateTimeProvider
    {
        DateTime GetLocalDateTime(DateTime utcDateTime);
    }

    public class LocalDateTimeProvider : ILocalDateTimeProvider
    {
        private readonly FIL.Logging.ILogger _logger;
        public LocalDateTimeProvider(ILogger logger, ISettings settings
            )
        {
            _logger = logger;
        }

        public DateTime GetLocalDateTime(DateTime utcDateTime)
        {
            try
            {
                TimeZoneInfo tzinfo = TimeZoneInfo.Local;
                var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime,
TimeZoneInfo.FindSystemTimeZoneById(tzinfo.StandardName));
                return localDateTime;
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return utcDateTime;
            }
        }
    }
}
