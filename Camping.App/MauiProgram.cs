using Camping.App.ViewModels;
using Camping.App.Views;
using Camping.Core.Data.Helpers;
using Camping.Core.Data.Repositories;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Interfaces.Services;
using Camping.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Syncfusion.Licensing;
using Syncfusion.Maui.Core.Hosting;

namespace Camping.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            SyncfusionLicenseProvider.RegisterLicense("NDE1NzY5N0AzMjM3MmUzMDJlMzBiYU5vV3Q0SENLMjROcjR3dnJQVnNVa2dtd0FtQnh0R1hpdFovQllKZ2hzPQ==;NDE1NzY5OEAzMjM3MmUzMDJlMzBtNkR2c1pWanAxbkF2Q1hUMW5rSTNsZkJrVUFlVC83TUJhS3RISkRXUXNFPQ==;Mgo+DSMBPh8sVXJzS0d+WFlPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9mSHxSc0RiXHdfcXdcT2NXU0M=;ORg4AjUWIQA/Gnt2UlhhQlVMfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hTH9Rd0BiWHpWcXVRR2VcWkZx;NDE1NzcwMUAzMjM3MmUzMDJlMzBablg5M0Rrc3gzdFVpbUlLUDU0a09aOVA1N3FXUU1YSDdXeXdMd1p4UzRnPQ==;NDE1NzcwMkAzMjM3MmUzMDJlMzBTTVhWMm5WZy9pTHpySjRrdmpOVkovUzVTdHZEWlFBdDhDVnFIRWtLbVNnPQ==;Mgo+DSMBMAY9C3t2UlhhQlVMfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hTH9Rd0BiWHpWcXVQRGRUWkZx;NDE1NzcwNEAzMjM3MmUzMDJlMzBLZy9BT3ZHRXZ1QkhYNXorb2Fta1lNVHhaT1pZNWhqUWczZm1yc0lZQXVvPQ==;NDE1NzcwNUAzMjM3MmUzMDJlMzBNOEpxdUN0UGRPMEVseEcyRjdINURZVnJ1ZjJ6Zlc5Mnh1RzZCcFRQc2pVPQ==;NDE1NzcwNkAzMjM3MmUzMDJlMzBablg5M0Rrc3gzdFVpbUlLUDU0a09aOVA1N3FXUU1YSDdXeXdMd1p4UzRnPQ==");
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
            builder.Services.AddSingleton<IGastRepository, MySqlGastRepository>();

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
