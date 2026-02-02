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
        [ObservableProperty]
        private Brush _backgroundBrush = WeatherHelper.GetBackgroundGradient("Clear");
        [ObservableProperty]
        private ObservableCollection<CityInfo> _citySuggestions = new();
        [ObservableProperty]
        private string _weatherAnimation = string.Empty;


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
            CitySuggestions.Clear();
            WeatherAnimation = string.Empty;

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
                CitySuggestions.Clear();
                
                if (result != null)
                {
                    //Fetch Day name version and icon
                    result.DayDisplay = WeatherHelper.GetDayName(null, true);
                    result.IconUrl = WeatherHelper.GetIconUrl(result.weather);

                    //Show converted Sunrise and Sunset time 
                    result.SunriseView = WeatherHelper.ConvertUnixToTime(result.sys.sunrise, result.timezone);
                    result.SunsetView = WeatherHelper.ConvertUnixToTime(result.sys.sunset, result.timezone);

                    //Find Weather type for Background
                    var weatherCondition = result.weather.FirstOrDefault()?.main ?? "Clear";
                    if (!string.IsNullOrEmpty(weatherCondition))
                    {
                        BackgroundBrush = WeatherHelper.GetBackgroundGradient(weatherCondition);
                    }
                    else
                    {
                        await Shell.Current.DisplayAlertAsync("Error", WeatherHelper.GeneralError, "OK");
                    }
                    WeatherAnimation = WeatherHelper.GetWeatherAnimation(weatherCondition);

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

        //Show city suggestions when typing from 3 letters
        partial void OnSelectedCityChanged(string value)
        {
            if (value?.Length >= 3) GetCitySuggestions(value);
            else CitySuggestions.Clear();
        }

        //Fetch city suggestions from API
        private async void GetCitySuggestions(string query)
        {
            var cities = await _weatherService.GetCitySuggestionsAsync(query);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CitySuggestions = new ObservableCollection<CityInfo>(cities);
            });
        }
    }
}
