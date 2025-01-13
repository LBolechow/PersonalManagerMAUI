using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PersonalManager.Config;
using Syncfusion.Licensing;
using Syncfusion.Maui.Core.Hosting;

namespace PersonalManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Services.AddSingleton<AppDbContext>(serviceProvider =>
            {
                var dbName = "personalManager.db"; // Zmienna z nazwą bazy danych
                return new AppDbContext(dbName);
            });
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmtCfExyWmFZfVtgfV9HY1ZTRWY/P1ZhSXxWdkRiWH9ZcnNWTmVdUUA=");


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
