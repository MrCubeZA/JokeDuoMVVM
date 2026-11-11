using ChuckNorrisJokesApp.Services;
using ChuckNorrisJokesApp.Services.Interfaces;
using ChuckNorrisJokesApp.ViewModels;
using ChuckNorrisJokesApp.Views;
using Microsoft.Extensions.Logging;

namespace ChuckNorrisJokesApp
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
            builder.Services.AddSingleton<IChuckNorrisApiService, ChuckNorrisApiHttpService>();
            builder.Services.AddSingleton<IDadJokeApiService, DadJokeApiHttpService>();
            builder.Services.AddSingleton<IPopUps, MauiPopupService>();

            builder.Services.AddTransient<JokesView>();
            builder.Services.AddTransient<JokesViewModel>();

            return builder.Build();
        }
    }
}
