namespace AcaiMobile;

public partial class FoodJournalPage : ContentPage
{
	public FoodJournalPage(FoodJournalViewModel viewModel)
	{
		InitializeComponent();
		// Set the Binding Context to a View Model class we provide through Dependency Injection (see MauiProgram.cs)
        BindingContext = viewModel;
    }
}