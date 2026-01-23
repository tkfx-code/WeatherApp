using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WeatherApp.Model;
using WeatherApp.Services;
using System.Linq;
using WeatherApp.Helpers;

namespace WeatherApp.ViewModels
{
    partial class MainPageViewModel : ObservableObject
    {
        private readonly IWeatherService _weatherService;
        //Main Search object view
        public ObservableCollection<WeatherModel> Weather {  get; set; }
        public ObservableCollection<ForecastItem> Forecast { get; set; }
        //Detailed view
        [ObservableProperty]
        WeatherModel selectedWeather;
        [ObservableProperty] 
        private bool _isBusy;
        [ObservableProperty]
        private string _selectedCity;
        [ObservableProperty]
        private string _selectedTimeline;

        public RelayCommand ClearCommand { get; }

        public MainPageViewModel(IWeatherService weatherService)
        {
            //Repo
            _weatherService = weatherService;
            Weather = new ObservableCollection<WeatherModel>();
        }

        // Load Async method?
        // Update Dashboard method?

        //Properties, which variablesdo we send to UI? 
        [RelayCommand]
        public async void FetchCityWeather(string city)
        {
            _isBusy = true;
            _selectedCity = city;
            try
            {
                var result = await _weatherService.GetWeatherAsync(city);
                
                if (result != null)
                {
                    //Fetch Day name version and icon
                    result.DayDisplay = WeatherHelper.GetDayName(null, true);
                    result.IconUrl = WeatherHelper.GetIconUrl(result.weather[0].icon);

                    Weather.Clear();
                    Weather.Add(result);
                }
                var forecastResult = await _weatherService.GetForecastAsync(city);
                if (forecastResult != null)
                {
                    Forecast.Clear();

                    //filter to one measuring point per day
                    var fiveDayForecast = forecastResult.Where(f => f.dt_txt.Contains("12:00:00")).ToList();
                    foreach (var item in fiveDayForecast)
                    {
                        //Fetch Day name version and icon
                        item.DayDisplay = WeatherHelper.GetDayName(item.dt_txt);
                        item.IconUrl = WeatherHelper.GetIconUrl(item.weather[0].icon);

                        //No need to specifically add 5 days since our API only stores 5 days forecast
                        Forecast.Add(item);
                    }
                }
                _isBusy = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: {e.Message}");
                return;
            }
        }
    }
}
