using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AcaiMobile;

public partial class WeighInEditorPageViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _submitted = false;

    [ObservableProperty]
    private bool _canBeSubmitted = false;

    [ObservableProperty]
    private DateTime _weighInDate = DateTime.Now;

    [ObservableProperty]
    private string _pounds = string.Empty;

    [ObservableProperty]
    private string _bodyFat = string.Empty;

    private readonly Popup _boundPopup;

    public WeighInEditorPageViewModel(Popup boundPopup)
    {
        _boundPopup = boundPopup;
    }
    
    [RelayCommand]
    private void ValidateNewWeighInDetails()
    {
        var hasNotAlreadyBeenSubmitted = !_submitted;
        var poundsIsValid = !string.IsNullOrWhiteSpace(_pounds) && float.TryParse(_pounds, out var parsedPounds) && parsedPounds >= 0;

        var bodyFatIsValid = true;
        if (!string.IsNullOrWhiteSpace(_bodyFat))
        {
            if (float.TryParse(_bodyFat, out var parsedBodyFat))
            {
                bodyFatIsValid = parsedBodyFat >= 0 && parsedBodyFat <= 100;
            }
            else
            {
                bodyFatIsValid = false;
            }
        }

        CanBeSubmitted = hasNotAlreadyBeenSubmitted && poundsIsValid && bodyFatIsValid;
    }

    [RelayCommand]
    private void Submit()
    {
        _submitted = true;
        _boundPopup?.Close();
    }
}
