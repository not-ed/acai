using CommunityToolkit.Maui.Alerts;
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
    
    private IUpdateChecker _updateChecker = new GithubUpdateChecker();
    
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
        var initiationToast = Toast.Make("Checking for updates...");
        initiationToast.Show();

        var newRelease = await _updateChecker.CheckForNewReleases();
        initiationToast.Dismiss();
        if (newRelease != null)
        {
            var updateAlert = await Shell.Current.DisplayAlert($"{newRelease.Version ?? "Update"} Available", $"A new Release of Acai is available, which brings about a series of improvements and fixes.\n\nWould you like to download this Release now?", "Download", "Dismiss");
            if (updateAlert)
            {
                try
                {
                    Browser.Default.OpenAsync(newRelease.DirectDownloadUrl ?? newRelease.ReleasePageUrl, BrowserLaunchMode.SystemPreferred);
                }
                catch
                {
                    // ignored
                }
            }
        }
        else
        {
            var updateCheckThrewAnException = _updateChecker.GetExceptionMessage() != null;
            var resultToast = updateCheckThrewAnException ? Toast.Make($"Unable to check for updates ({_updateChecker.GetExceptionMessage()}).") : Toast.Make("No new updates found.");
            resultToast.Show();
        }
    }
}