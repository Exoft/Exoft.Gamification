using System;
using System.Globalization;

namespace Exoft.Gamification.Api.Common.Helpers
{
    public static class Iso8601DateTimeExtensions
    {
        public static string ConvertToIso8601Date(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static string ConvertToIso8601DateTimeUtc(this DateTime dateTime)
        {
            return $"{dateTime.ToUniversalTime().ToString("s", CultureInfo.InvariantCulture)}Z";
        }
    }


    public static class Iso8601StringExtensions
    {
        public static bool TryParseIso8601Date(this string iso8601Date, out DateTime result)
        {
            if (string.IsNullOrWhiteSpace(iso8601Date))
            {
                result = default(DateTime);
                return false;
            }

            var dateTimeParseResult = DateTime.TryParseExact(iso8601Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime iso8601DateTimeObject);
            if (!dateTimeParseResult)
            {
                result = default(DateTime);
                return false;
            }

            result = iso8601DateTimeObject;
            return true;
        }

        public static bool TryParseIso8601DateTimeToUtc(this string iso8601DateTime, out DateTime result)
        {
            if (string.IsNullOrWhiteSpace(iso8601DateTime))
            {
                result = default(DateTime);
                return false;
            }

            var dateTimeParseResult = DateTime.TryParse(iso8601DateTime, null, DateTimeStyles.RoundtripKind, out DateTime iso8601DateTimeObject);

            if (!dateTimeParseResult)
            {
                result = default(DateTime);
                return false;
            }

            result = iso8601DateTimeObject.ToUniversalTime();
            return true;
        }
    }
}
