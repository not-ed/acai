using AcaiMobile.Pages;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.LifecycleEvents;

namespace AcaiMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureLifecycleEvents(events =>
                    events.AddAndroid(android => 
                        android.OnResume(_ => AcaiAndroidEvents.OnResume())
                    )
                );

            // Setup dependency injection for injecting ViewModel class as the binding context of a page.
            builder.Services.AddSingleton<FoodJournalPage>();
            builder.Services.AddSingleton<FoodJournalViewModel>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<SettingsPageViewModel>();

            return builder.Build();
        }
    }
}