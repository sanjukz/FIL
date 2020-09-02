using System;
using System.Collections.Generic;

namespace FIL.Api.Providers.EventManagement
{
    public interface ICommonUtilityProvider
    {
        DateTime GetUtcDate(DateTime dateTime, string localTime, string timeZoneOffset);

        IEnumerable<DateTime> EachDay(DateTime from, DateTime thru);
    }

    public class CommonUtilityProvider : ICommonUtilityProvider
    {
        public CommonUtilityProvider()
        {
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public DateTime GetUtcDate(DateTime dateTime, string localTime, string timeZoneOffset)
        {
            var dateTime1 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, Convert.ToInt32(localTime.Split(":")[0]), Convert.ToInt32(localTime.Split(":")[1]), 0);

            if (timeZoneOffset.Contains("+") || !timeZoneOffset.Contains("-"))
            {
                var time = Convert.ToInt64(timeZoneOffset.Replace("+", ""));
                dateTime1 = dateTime1.AddMinutes(-time);
            }
            else if (timeZoneOffset.Contains("-"))
            {
                var time = Convert.ToInt64(timeZoneOffset.Replace("-", ""));
                dateTime1 = dateTime1.AddMinutes(time);
            }
            return dateTime1;
        }
    }
}