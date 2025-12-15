using Camping.App.ViewModels;
using Camping.App.Views;
using Camping.Core.Data.Repositories;
using Camping.Core.Data.Helpers;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Interfaces.Services;
using Camping.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Syncfusion.Maui.Core.Hosting;

namespace Camping.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<DbConnection>();
            //Repositories
            builder.Services.AddSingleton<IVeldRepository, VeldRepository>();
            builder.Services.AddSingleton<IStaanplaatsRepository, StaanplaatsRepository>();
            builder.Services.AddSingleton<IAccommodatieRepository, AccommodatieRepository>();
            builder.Services.AddSingleton<IReserveringRepository, ReserveringRepository>();

            //Services
            builder.Services.AddSingleton<IVeldService, VeldService>();
            builder.Services.AddSingleton<IReservatieDataService, ReservatieDataService>();
            builder.Services.AddSingleton<IAccommodatieService, AccommodatieService>();
            builder.Services.AddSingleton<IReserveringService, ReserveringService>();
            builder.Services.AddSingleton<ReserveringshouderValidatieService>();

            //ViewModels
            builder.Services.AddTransient<PlattegrondViewModel>();
            builder.Services.AddTransient<ReserveringsoverzichtViewModel>();
            builder.Services.AddTransient<KalenderViewModel>();
            builder.Services.AddTransient<VeldDetailViewModel>();

            //Views
            builder.Services.AddTransient<PlattegrondView>();
            builder.Services.AddTransient<ReserveringsoverzichtView>();
            builder.Services.AddTransient<KalenderView>();
            builder.Services.AddTransient <VeldDetailView>();


#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                // Make sure to add "using Microsoft.Maui.LifecycleEvents;" in the top of the file 
                events.AddWindows(windowsLifecycleBuilder =>
                {
                    windowsLifecycleBuilder.OnWindowCreated(window =>
                    {
                        window.ExtendsContentIntoTitleBar = false;
                        var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                        switch (appWindow.Presenter)
                        {
                            case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                                // Gezien elementen prima mee scalen, is het forceren van full screen niet meer nodig.
                                // Plaatsing nog wel een beetje whack als het scherm heel klein is, maar dat is een edge case.
                                // overlappedPresenter.SetBorderAndTitleBar(false, false);
                                overlappedPresenter.Maximize();
                                break;
                        }
                    });
                });
            });
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
