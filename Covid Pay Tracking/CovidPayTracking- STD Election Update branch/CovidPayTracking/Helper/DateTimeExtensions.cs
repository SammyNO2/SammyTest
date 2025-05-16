using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace CovidPayTracking.Helper
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        public static TimeSpan? ConvertTimespan(this string value)
        {
            TimeSpan? span = null;
            DateTime validDate;
            span = DateTime.TryParseExact(value, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out validDate) ? validDate.TimeOfDay : (TimeSpan?)null;
            return span;
        }

        public static string TimespanStringFormat(this TimeSpan? timespan)
        {
            //   return timespan.HasValue ? timespan.Value.ToString("hh:mm tt") : string.Empty;
            if (timespan.HasValue)
            {
                var dt = DateTime.Today.Add(timespan.Value);
                return dt.ToString("hh:mm tt");
            }
            else
            {
                return string.Empty;
            }
        }
    }
}