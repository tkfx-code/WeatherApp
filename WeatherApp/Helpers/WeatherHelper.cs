using WeatherApp.Model;
using static WeatherApp.Model.WeatherModelBase;

namespace WeatherApp.Helpers
{
    public static class WeatherHelper
    {
        //Error strings for different errors
        public const string CityNotFoundError = "We could not find the town you were looking for";
        public const string NetworkError = "We could not connect to our weather service";
        public const string GeneralError = "Something went wrong! Try again later";

        //Fetch display name for day, long if today, short if forecast
        public static string GetDayName(string dtTxt, bool isLong = false)
        {
            string format = isLong ? "dddd" : "ddd";

            if(DateTime.TryParse(dtTxt, out DateTime date))
            {
                return date.ToString(format, System.Globalization.CultureInfo.InvariantCulture);
            }
            //Todays date does not have a dt_txt and will therefore show full length name
            return DateTime.Now.ToString(format, System.Globalization.CultureInfo.InvariantCulture);
        }
        //Fetch display icon for weather
        public static string GetIconUrl(List<Weather> weatherList)
        {
            var first = weatherList.FirstOrDefault();
            if (first != null && !string.IsNullOrEmpty(first.icon))
            {
                return $"https://openweathermap.org/img/wn/{first.icon}@2x.png";
            }
            return string.Empty;
        }

        //Convert unix time to something we can read
        public static string ConvertUnixToTime(int unixTime, int timezoneOffset)
        {
            //Returns "12:00" format
            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime + timezoneOffset).DateTime;
            return dateTime.ToString("HH:mm"); 
        }
    }
}
