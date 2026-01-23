
namespace WeatherApp.Model
{
    //Model is my DTO
    public class WeatherModelBase
    {
        public string DayDisplay { get; set; }
        public string IconUrl { get; set; }
    }
    public class WeatherModel : WeatherModelBase
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public Main main { get; set; }
        public Wind wind { get; set; }
        //public Clouds clouds { get; set; }
        //public int dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        //public int cod { get; set; }
    }
    public class ForecastResponse
    {
        public List<ForecastItem> List { get; set; }
        public CityInfo city { get; set; }
    }
    public class ForecastItem : WeatherModelBase
    {
        public Main main { get; set; }
        public List<Weather> weather { get; set; }
        public string dt_txt { get; set; }
        
    }

    public class Coord
    {
        public float lon { get; set; }
        public float lat { get; set; }
    }

    public class Main
    {
        public float temp { get; set; } //Default = Kelvin, Metric = Celsius, Imperial = Fahrenheit
        public float feels_like { get; set; } //Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit 
        public float temp_min { get; set; } //Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit 
        public float temp_max { get; set; } //Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit 
        //public int pressure { get; set; }
        public int humidity { get; set; }
        //public int sea_level { get; set; }
        //public int grnd_level { get; set; }
    }

    public class Wind
    {
        public float speed { get; set; }
        //public int deg { get; set; }
    }

    //public class Clouds
    //{
    //    public int all { get; set; }
    //}

    public class Sys
    {
        public string country { get; set; } //Country code: GB, JP etc.
        public int sunrise { get; set; } //unix UTC, needs help method in VM to turn into clock time
        public int sunset { get; set; } //unix UTC, needs help method in VM to turn into clock time
    }

    public class Weather
    {
        public int id { get; set; }
        //public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class CityInfo
    {
        public string name { get; set; }
        public string country { get; set; }
    }
}
