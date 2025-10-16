using cebujeepney.ViewModels;
using cebujeepney.Views;

namespace cebujeepney
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

            // ViewModels
            builder.Services.AddSingleton<LoginVM>();
            builder.Services.AddTransient<RegisterVM>();

            // Pages
            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddTransient<RegisterView>();

            return builder.Build();
        }
    }
}

