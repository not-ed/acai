namespace AcaiMobile.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsPageViewModel viewModel)
    {
        InitializeComponent();
        // Set the Binding Context to a View Model class we provide through Dependency Injection (see MauiProgram.cs)
        BindingContext = viewModel;
    }
}