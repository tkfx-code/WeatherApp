using WeatherApp.Model;

namespace WeatherApp.Helpers
{
    public static class WeatherHelper
    {
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
    }
}
