﻿using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

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
                });

            // Setup dependency injection for injecting ViewModel class as the binding context of a page.
            builder.Services.AddSingleton<FoodJournalPage>();
            builder.Services.AddSingleton<FoodJournalViewModel>();

            return builder.Build();
        }
    }
}