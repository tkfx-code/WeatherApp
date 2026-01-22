using Microsoft.Extensions.Logging;
using WeatherApp.Services;
using WeatherApp.Repositories;
using WeatherApp.ViewModels;
using System.Reflection;
using Microsoft.Extensions.Configuration;

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
                });

            // Add services to the container.
            builder.Services.AddHttpClient<IWeatherService, WeatherService>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<MainPage>();

            //    builder.Services.AddControllers();
            //        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //        builder.Services.AddOpenApi();
            //        builder.Services.AddSwaggerGen();

            //        // Configure the HTTP request pipeline.
            //        if (app.Environment.IsDevelopment())
            //        {
            //            app.UseSwagger();
            //            app.UseSwaggerUI();
            //            //app.MapOpenApi();
            //        }

            //app.UseHttpsRedirection();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
