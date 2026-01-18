using CommunityToolkit.Maui.Views;

namespace AcaiMobile;

public partial class WeighInEditorPage : Popup
{
    public WeighInEditorPage()
    {
        InitializeComponent();
        BindingContext = new WeighInEditorPageViewModel(this);
    }

    public DateTime GetSubmittedDate()
    {
        var viewModel = (WeighInEditorPageViewModel)BindingContext;
        return viewModel.WeighInDate;
    }

    public float GetSubmittedPounds()
    {
        var viewModel = (WeighInEditorPageViewModel)BindingContext;
        return float.Parse(viewModel.Pounds);
    }

    public float? GetSubmittedBodyFat()
    {
        var viewModel = (WeighInEditorPageViewModel)BindingContext;
        return float.TryParse(viewModel.BodyFat, out var parsedBodyFat) ? parsedBodyFat : null;
    }

    public bool HasBeenSubmitted()
    {
        var viewModel = (WeighInEditorPageViewModel)BindingContext;
        return viewModel.Submitted;
    }
}