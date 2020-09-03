using System;

namespace FIL.Web.Admin.Providers
{
  public interface ILocalTimeProvider
  {
    DateTime GetLocalTime(DateTime dateTime, string timeZoneOffset);
  }

  public class LocalTimeProvider : ILocalTimeProvider
  {
    public LocalTimeProvider()
    {
    }

    public DateTime GetLocalTime(DateTime dateTime, string timeZoneOffset)
    {
      if (timeZoneOffset.Contains("+"))
      {
        var time = Convert.ToInt64(timeZoneOffset.Replace("+", ""));
        dateTime = dateTime.AddMinutes(+time);
      }
      else if (timeZoneOffset.Contains("-"))
      {
        var time = Convert.ToInt64(timeZoneOffset.Replace("-", ""));
        dateTime = dateTime.AddMinutes(-time);
      }
      return dateTime;
    }
  }
}
