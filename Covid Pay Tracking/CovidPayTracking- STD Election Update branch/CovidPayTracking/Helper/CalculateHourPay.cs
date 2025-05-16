using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidPayTracking.Helper
{
    public static class CalculateHourPay
    {
        /// <summary>
        /// This calculates hour difference and updates if need to modify
        /// </summary>
        /// <param name="newHour">New hour</param>
        /// <param name="oldHour">Old Hour</param>
        public static string CalculateHour(this string newHour, string oldHour)
        {
            if (!string.IsNullOrEmpty(newHour))
            {
                newHour = (Convert.ToDecimal(oldHour) + Convert.ToDecimal(newHour)).ToString();
            }
            else
            {
                newHour = oldHour;
            }
            return newHour;
        }

        public static string CalculateDiffHours(string prevHour, string fte)
        {
            string diffHour = "";
            if (!string.IsNullOrEmpty(fte) && !string.IsNullOrEmpty(prevHour))
            {
                diffHour = (Convert.ToDecimal(prevHour) - Convert.ToDecimal(fte)).ToString();
            }
            return diffHour;
        }
    }

    public static class StringValidation{
        public static int? ConvertStringToInt(this string currentID)
        {
            int? intValue = null;
            if (currentID != null && currentID != "")
            {
                intValue = int.TryParse(currentID, out int i) ? i : 0;
            }
            return intValue;
        }

        public static decimal ValidateToDecimal(this string strValue)
        {
            decimal decValue = 0m;
            if (strValue != null && strValue != "")
            {
                decValue = decimal.TryParse(strValue, out decimal i) ? i : 0m;
            }
            return decValue;
        }

        public static double ValidateToDouble(this string strValue)
        {
            double dblValue = 0D;
            if (strValue != null && strValue != "")
            {
                dblValue = double.TryParse(strValue, out double i) ? i : 0D;
            }
            return dblValue;
        }

        public static int ValidateEmployeeID(this string strValue)
        {
            int intVal = 0;
            bool result = int.TryParse(strValue, out intVal);
            if (!result)
            {
                var applog = new ApplicationExceptionLog();
                applog.SaveEmployeeIDException(strValue);
            }
            return intVal;
        }
    }
}