using Microsoft.Extensions.Logging;
using Camping.App.ViewModels;
using Camping.App.Views;
using Microsoft.Maui.LifecycleEvents;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Data.Repositories;

namespace Camping.App
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

            builder.Services.AddSingleton<PlattegrondViewModel>();
            builder.Services.AddSingleton<PlattegrondView>();
            builder.Services.AddSingleton<IStaanplaatsRepository, InMemoryStaanplaatsRepository>();
            builder.Services.AddSingleton<PlattegrondViewModel>();
            builder.Services.AddSingleton<PlattegrondView>();

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
                                overlappedPresenter.SetBorderAndTitleBar(false, false);
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
