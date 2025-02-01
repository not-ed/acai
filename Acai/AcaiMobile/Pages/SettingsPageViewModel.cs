using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AcaiMobile.Pages;

public partial class SettingsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private float _dailyCaloricLimit = Preferences.Get(PreferenceIndex.DailyCaloricLimit.Key, PreferenceIndex.DailyCaloricLimit.DefaultValue);

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
}