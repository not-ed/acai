namespace AcaiMobile.Pages;

public partial class WeightJournalPage : ContentPage
{
    public WeightJournalPage(WeightJournalPageViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }
}