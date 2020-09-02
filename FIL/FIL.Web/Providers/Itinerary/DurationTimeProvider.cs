using FIL.Logging;
using FIL.Configuration;
using System;
using System.Linq;
using static FIL.Web.Feel.Controllers.PlaceItinerarySearchController;
using System.Text.RegularExpressions;

namespace FIL.Web.Feel.Providers.Itinerary
{
    public interface IDurationTimeProvider
    {
        DurationModel DurationTime(Parent result);
    }

    public class DurationModel
    {
        public string DurationValue { get; set; }
        public int DurationTime { get; set; }
    }


    public class DurationTimeProvider : IDurationTimeProvider
    {
        private readonly FIL.Logging.ILogger _logger;
        public DurationTimeProvider(ILogger logger, ISettings settings
            )
        {
            _logger = logger;
        }

        string GetDurationTime(Parent result)
        {
            String durationValue;
            if (result.rows[0].elements[0].status == "ZERO_RESULTS")
            {
                durationValue = "0 min";
            }
            else
            {
                durationValue = result.rows[0].elements[0].duration.text;
            }
            return durationValue;
        }

        public DurationModel DurationTime(Parent result)
        {
            var durationValue = GetDurationTime(result);
            var durationTime = 0;
            try
            {
                if (durationValue.Split("days").Count() > 1)
                {
                    durationTime = ((int.Parse(durationValue.Split("days")[0]) * 60) * 24) + (int.Parse(durationValue.Split("days")[1].Split("hours")[0]) * 60);
                }
                if (durationValue.Split("day").Count() > 1)
                {
                    durationTime = ((int.Parse(durationValue.Split("day")[0]) * 60) * 24) + (int.Parse(durationValue.Split("day")[1].Split("hours")[0]) * 60);
                }
                else if (durationValue.Split("hours").Count() > 1)
                {
                    durationTime = (int.Parse(durationValue.Split("hours")[0]) * 60) + int.Parse(durationValue.Split("hours")[1].Split("mins")[0]);
                }
                else if (durationValue.Split("hour").Count() > 1)
                {
                    durationTime = (int.Parse(durationValue.Split("hour")[0]) * 60) + int.Parse(durationValue.Split("hour")[1].Split("mins")[0]);
                }
                else
                {
                    durationTime = int.Parse(Regex.Replace(durationValue.ToString(), "[^0-9]+", string.Empty));
                }
                return new DurationModel
                {
                    DurationTime = durationTime,
                    DurationValue = durationValue
                };
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return new DurationModel { };
            }
        }
    }
}
