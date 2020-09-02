using System;

namespace FIL.Api
{
    public interface IUtcTimeProvider
    {
        DateTime GetUtcTime(DateTime dateTime, string timeZoneOffset);
    }

    public class UtcTimeProvider : IUtcTimeProvider
    {
        public UtcTimeProvider()
        {
        }

        public DateTime GetUtcTime(DateTime dateTime, string timeZoneOffset)
        {
            if (timeZoneOffset.Contains("+"))
            {
                var time = Convert.ToInt64(timeZoneOffset.Replace("+", ""));
                dateTime = dateTime.AddMinutes(-time);
            }
            else if (timeZoneOffset.Contains("-"))
            {
                var time = Convert.ToInt64(timeZoneOffset.Replace("-", ""));
                dateTime = dateTime.AddMinutes(time);
            }
            return dateTime;
        }
    }
}