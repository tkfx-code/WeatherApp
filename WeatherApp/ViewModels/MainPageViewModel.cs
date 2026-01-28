using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WeatherApp.Services;
using WeatherApp.Helpers;
using WeatherApp.Model;

namespace WeatherApp.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IWeatherService _weatherService;

        //Main Search object view
        public ObservableCollection<WeatherModel> Weather {  get; set; }
        public ObservableCollection<ForecastItem> Forecast { get; set; }
        //Detailed view
        [ObservableProperty]
        WeatherModel _selectedWeather;
        [ObservableProperty] 
        private bool _isBusy;
        [ObservableProperty]
        private string _selectedCity;
        [ObservableProperty]
        private string _selectedTimeline;
        [ObservableProperty]
        private string _errorMessage;
        [ObservableProperty]
        bool _isDetailsVisible;

        public MainPageViewModel(IWeatherService weatherService)
        {
            //Repo
            _weatherService = weatherService;
            Weather = new ObservableCollection<WeatherModel>();
            Forecast = new ObservableCollection<ForecastItem>();
        }

        //Clear city, make sure all info is cleared
        [RelayCommand]
        public void Clear()
        {
            SelectedCity = string.Empty;
            ErrorMessage = string.Empty;

            Weather.Clear();
            Forecast.Clear();

            IsBusy = false;
            SelectedWeather = null;
        }

        //Properties
        [RelayCommand]
        public async Task FetchCityWeather()
        {
            if (string.IsNullOrWhiteSpace(SelectedCity))
            {
                ErrorMessage = "Write a city name before search";
                return;
            }

            //Save to local var
            string cityToSearch = SelectedCity;

            Weather.Clear();
            Forecast.Clear();
            ErrorMessage = string.Empty;
            IsBusy = true;

            try
            {
                var result = await _weatherService.GetWeatherAsync(cityToSearch);
                
                if (result != null)
                {
                    //Fetch Day name version and icon
                    result.DayDisplay = WeatherHelper.GetDayName(null, true);
                    result.IconUrl = WeatherHelper.GetIconUrl(result.weather);

                    //Force show icon
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        result.IconUrl = WeatherHelper.GetIconUrl(result.weather);
                    });

                    //Show converted Sunrise and Sunset time 
                    result.SunriseView = WeatherHelper.ConvertUnixToTime(result.sys.sunrise, result.timezone);
                    result.SunsetView = WeatherHelper.ConvertUnixToTime(result.sys.sunset, result.timezone);

                    Weather.Add(result);
                    IsDetailsVisible = false;
                }
                var forecastResult = await _weatherService.GetForecastAsync(cityToSearch);
                if (forecastResult != null)
                {
                    //filter to one measuring point per day
                    var fiveDayForecast = forecastResult.Where(f => f.dt_txt.Contains("12:00:00")).ToList();
                    foreach (var item in fiveDayForecast)
                    {
                        //Fetch Day name version and icon
                        item.DayDisplay = WeatherHelper.GetDayName(item.dt_txt, false);
                        item.IconUrl = WeatherHelper.GetIconUrl(item.weather);

                        //No need to specifically add 5 days since our API only stores 5 days forecast
                        Forecast.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
               Debug.WriteLine($"Error: {e.Message}");
               ErrorMessage = WeatherHelper.GeneralError;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public void ToggleDetails()
        {
            IsDetailsVisible = !IsDetailsVisible;
        }

    }
}
