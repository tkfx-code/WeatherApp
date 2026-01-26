using Microsoft.Extensions.Logging;
using WeatherApp.Services;
using WeatherApp.Repositories;
using WeatherApp.ViewModels;

namespace WeatherApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    //fonts.AddFont("Font Awesome 7 Free-Solid-900.otf", "FontAwesomeSolid");
                    //fonts.AddFont("Font Awesome 7 Free - Regular - 400.otf", "FontAwesomeRegular");
                    //fonts.AddFont("Font Awesome 7 Brands-Regular-400.otf", "FontAwesomeBrands");
                });

            // Add services to the container.
            builder.Services.AddHttpClient<IWeatherService, WeatherService>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<MainPage>();
            //add builders for detailspage if i make one
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
