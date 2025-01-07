using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AcaiMobile;

public partial class NewItemViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _submitted = false;
    [ObservableProperty] 
    private bool _canBeSubmitted = false;
    [ObservableProperty]
    private string _newItemName = string.Empty;

    [ObservableProperty] private float _newItemCalories = 0;
    [ObservableProperty]
    private DateTime _newItemCreationDate = DateTime.Now;
    [ObservableProperty]
    private bool _createNewFoodItemShortcut = false;

    [RelayCommand]
    private void ValidateNewItemDetails()
    {
        var hasNotAlreadyBeenSubmitted = !_submitted;
        var itemNameIsNotEmpty = !string.IsNullOrWhiteSpace(_newItemName) && _newItemName.Length > 0;

        CanBeSubmitted = hasNotAlreadyBeenSubmitted && itemNameIsNotEmpty;
    }

    [RelayCommand]
    private void SubmitModal()
    {
        _submitted = true;
        Shell.Current.Navigation.PopModalAsync(true);
    }
}