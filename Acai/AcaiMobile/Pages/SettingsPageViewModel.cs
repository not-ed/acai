using CommunityToolkit.Mvvm.ComponentModel;

namespace AcaiMobile.Pages;

public partial class SettingsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private float _dailyCaloricLimit = 1234;

    public SettingsPageViewModel()
    {
        
    }
}