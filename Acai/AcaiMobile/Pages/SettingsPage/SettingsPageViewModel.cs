﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Exception = Java.Lang.Exception;

namespace AcaiMobile.Pages;

public partial class SettingsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private float _dailyCaloricLimit = Preferences.Get(PreferenceIndex.DailyCaloricLimit.Key, PreferenceIndex.DailyCaloricLimit.DefaultValue);
    
    [ObservableProperty]
    private bool _warnBeforeDiscardingFoodItemChanges = Preferences.Get(PreferenceIndex.WarnBeforeDiscardingFoodItemChanges.Key, PreferenceIndex.WarnBeforeDiscardingFoodItemChanges.DefaultValue);
    
    [ObservableProperty]
    private bool _displayProtein = Preferences.Get(PreferenceIndex.DisplayProtein.Key, PreferenceIndex.DisplayProtein.DefaultValue);
    
    [ObservableProperty]
    private bool _displayCarbohydrates = Preferences.Get(PreferenceIndex.DisplayCarbohydrates.Key, PreferenceIndex.DisplayCarbohydrates.DefaultValue);

    [ObservableProperty]
    private bool _displayFat = Preferences.Get(PreferenceIndex.DisplayFat.Key, PreferenceIndex.DisplayFat.DefaultValue);
    
    [ObservableProperty]
    private bool _displayFibre = Preferences.Get(PreferenceIndex.DisplayFibre.Key, PreferenceIndex.DisplayFibre.DefaultValue);
    
    [ObservableProperty]
    private bool _displayWater = Preferences.Get(PreferenceIndex.DisplayWater.Key, PreferenceIndex.DisplayWater.DefaultValue);

    [ObservableProperty] 
    private string _versionString = AppInfo.VersionString;
    
    [ObservableProperty]
    private bool _performAutomaticAppUpdates = Preferences.Get(PreferenceIndex.PerformAutomaticAppUpdates.Key, PreferenceIndex.PerformAutomaticAppUpdates.DefaultValue);
    
    [RelayCommand]
    private async void UpdateDailyCaloricLimitSetting()
    {
        var newDailyCaloricLimitPrompt = await Shell.Current.DisplayPromptAsync("Daily Caloric Limit", string.Empty, "Submit", "Cancel", "2000 kcal",-1, Keyboard.Numeric, DailyCaloricLimit.ToString());
        if (float.TryParse(newDailyCaloricLimitPrompt, out var enteredLimit) && enteredLimit != DailyCaloricLimit)
        {
            Preferences.Set(PreferenceIndex.DailyCaloricLimit.Key, enteredLimit);
            DailyCaloricLimit = Preferences.Get(PreferenceIndex.DailyCaloricLimit.Key, PreferenceIndex.DailyCaloricLimit.DefaultValue);
        }
    }

    [RelayCommand]
    private void UpdateWarningBeforeDiscardingFoodItemChanges()
    {
        Preferences.Set(PreferenceIndex.WarnBeforeDiscardingFoodItemChanges.Key, WarnBeforeDiscardingFoodItemChanges);
    }
    
    [RelayCommand]
    private void UpdateProteinVisibility()
    {
        Preferences.Set(PreferenceIndex.DisplayProtein.Key, DisplayProtein);
    }
    
    [RelayCommand]
    private void UpdateCarbohydratesVisibility()
    {
        Preferences.Set(PreferenceIndex.DisplayCarbohydrates.Key, DisplayCarbohydrates);
    }
    
    [RelayCommand]
    private void UpdateFatVisibility()
    {
        Preferences.Set(PreferenceIndex.DisplayFat.Key, DisplayFat);
    }
    
    [RelayCommand]
    private void UpdateFibreVisibility()
    {
        Preferences.Set(PreferenceIndex.DisplayFibre.Key, DisplayFibre);
    }
    
    [RelayCommand]
    private void UpdateWaterVisibility()
    {
        Preferences.Set(PreferenceIndex.DisplayWater.Key, DisplayWater);
    }

    [RelayCommand]
    private async void CheckForAppUpdates()
    {
        AcaiUpdateChecker.CheckForUpdates();
    }
    
    [RelayCommand]
    private void UpdatePerformAutomaticAppUpdatesFlag()
    {
        Preferences.Set(PreferenceIndex.PerformAutomaticAppUpdates.Key, PerformAutomaticAppUpdates);
    }
}