using System;

namespace App.Core.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// DateTime minimum value for the SQLServer DateTime DataType
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime SqlDateTimeMinValue(this DateTime date)
        {
            return new DateTime(1753, 1, 1);
        }
        /// <summary>
        /// DateTime maximum value for the SQLServer DateTime DataType
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime SqlDateTimeMaxValue(this DateTime date)
        {
            return new DateTime(9999, 12, 31); 
        }
        /// <summary>
        /// Determines whether [is valid date time] [the specified date time].
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid date time] [the specified date time]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidDateTime(this DateTime dateTime)
        {
            return (dateTime != DateTime.MinValue && dateTime != DateTime.MaxValue);
        }

        /// <summary>
        /// Gets the null safe date time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static DateTime GetNullSafeDateTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value : DateTime.MinValue;
        }

        /// <summary>
        /// Gets the weeks in month.
        /// </summary>
        /// <returns></returns>
        public static int GetWeeksInMonth(this DateTime dateTime)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            DayOfWeek wkstart = dateTime.DayOfWeek;

            DateTime first = new DateTime(year, month, 1);
            int firstwkday = (int)first.DayOfWeek;
            int otherwkday = (int)wkstart;

            int offset = ((otherwkday + 7) - firstwkday) % 7;

            double weeks = (DateTime.DaysInMonth(year, month) - offset) / 7d;

            return (int)Math.Ceiling(weeks);
        }

        /// <summary>
        /// Gets the days in month.
        /// </summary>
        /// <returns></returns>
        public static int GetDaysInMonth(this DateTime dateTime)
        {
            return DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
        }

        /// <summary>
        /// Sets the dayof month.
        /// </summary>
        /// <returns></returns>
        public static DateTime SetDayofMonth(this DateTime dateTime, int year, int month, int day)
        {
            return DateTime.Parse(String.Format("{0}/{1}/{2}", month, day, year));
        }

        /// <summary>
        /// Get the first day of the month for
        /// any full date submitted
        /// </summary>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(this DateTime dateTime)
        {
            // Return the first day of the month
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// Get the first day of the month for a
        /// month passed by it's integer value
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="iMonth">The i month.</param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(this DateTime dateTime, int iMonth)
        {
            // Set return value to the last day of the month
            // for any date passed in to the method.
            // Create a datetime variable set to the passed in date
            dateTime = new DateTime(DateTime.Now.Year, iMonth, 1);

            // Return the first day of the month
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// Get the last day of the month for any full date
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            // Return the last day of the month
            return GetFirstDayOfMonth(dateTime).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Get the last day of a month expressed by it's
        /// integer value
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="iMonth">The i month.</param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(this DateTime dateTime, int iMonth)
        {
            // set return value to the last day of the month
            // for any date passed in to the method
            // create a datetime variable set to the passed in date
            dateTime = new DateTime(DateTime.Now.Year, iMonth, 1);

            // Return the last day of the month
            return GetFirstDayOfMonth(dateTime).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Gets the week for A specified date.
        /// </summary>
        /// <returns></returns>
        public static int GetWeekForASpecifiedDate(this DateTime dateTime, DateTime fromDate)
        {
            // Get jan 1st of the year
            DateTime startOfYear = fromDate.AddDays(-fromDate.Day + 1).AddMonths(-fromDate.Month + 1);
            // Get dec 31st of the year
            DateTime endOfYear = startOfYear.AddYears(1).AddDays(-1);
            // ISO 8601 weeks start with Monday 
            // The first week of a year includes the first Thursday 
            // DayOfWeek returns 0 for sunday up to 6 for saterday
            int[] iso8601Correction = { 6, 7, 8, 9, 10, 4, 5 };
            int nds = fromDate.Subtract(startOfYear).Days + iso8601Correction[(int)startOfYear.DayOfWeek];
            int wk = nds / 7;
            switch (wk)
            {
                case 0:
                    // Return weeknumber of dec 31st of the previous year
                    return GetWeekForASpecifiedDate(dateTime, startOfYear.AddDays(-1));
                case 53:
                    // If dec 31st falls before thursday it is week 01 of next year
                    return endOfYear.DayOfWeek < DayOfWeek.Thursday ? 1 : wk;
                default:
                    return wk;
            }
        }

        /// <summary>
        /// Converts to UTC date time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static DateTime ConvertToUTCDateTime(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime();
        }

        /// <summary>
        /// Converts to local date time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static DateTime ConvertToLocalDateTime(this DateTime dateTime)
        {
            return dateTime.ToLocalTime();
        }

        /// <summary>
        /// Ends the of day.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 59);
        }

        /// <summary>
        /// Starts the of day.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 00, 00, 00, 00);
        }
    }
}
