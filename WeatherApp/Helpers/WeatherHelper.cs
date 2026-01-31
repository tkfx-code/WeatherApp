using WeatherApp.Model;
using Microsoft.Maui.Controls;

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

            if (DateTime.TryParse(dtTxt, out DateTime date))
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
                return $"http://openweathermap.org/img/wn/{first.icon}@2x.png";
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

        //Responsive background
        public static Brush GetBackgroundGradient(string weatherCondition)
        {
            var gradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };

            //Standard colors
            Color startColor = Color.FromArgb("#2193b0");
            Color endColor = Color.FromArgb("#6dd5ed");

            if(string.IsNullOrEmpty(weatherCondition))
            {
                return new LinearGradientBrush { };
            }

            //Logic to choose colors based on weather condition
            switch (weatherCondition.ToLower())
            {
                case "clouds":
                    startColor = Color.FromArgb("#606c88");
                    endColor = Color.FromArgb("#3f4c6b");
                    break;
                case "rain":
                case "drizzle":
                    startColor = Color.FromArgb("#203a43");
                    endColor = Color.FromArgb("#2c5364");
                    break;
                case "thunderstorm":
                    startColor = Color.FromArgb("#141e30");
                    endColor = Color.FromArgb("#243b55");
                    break;
                case "clear":
                    startColor = Color.FromArgb("#2193b0");
                    endColor = Color.FromArgb("#6dd5ed");
                    break;
                case "snow":
                    startColor = Color.FromArgb("#83a4d4");
                    endColor = Color.FromArgb("#b6fbff");
                    break;
                case "mist":
                case "fog":
                    startColor = Color.FromArgb("#FF0000"); //#606c88
                    endColor = Color.FromArgb("#3f4c6b");
                    break;
                default:
                    break;
            }
            gradient.GradientStops.Add(new GradientStop(startColor, 0.1f));
            gradient.GradientStops.Add(new GradientStop(endColor, 1.0f));

            return gradient;
        }

    }
}
