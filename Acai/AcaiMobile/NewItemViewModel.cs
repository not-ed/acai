using AcaiCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AcaiMobile;

public partial class FoodItemViewShortcut(FoodItemShortcutDTO shortcut) : ObservableObject
{
    [ObservableProperty] private long _id = shortcut.GetID();
    [ObservableProperty] private string _name = shortcut.GetName();
    [ObservableProperty] private float _calories = shortcut.GetCalories();
}

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
    
    [ObservableProperty]
    private List<FoodItemViewShortcut> _foodItemShortcuts;

    public NewItemViewModel()
    {
        var session = AcaiSessionSingleton.Get(null).Result;
        _foodItemShortcuts = new List<FoodItemViewShortcut>();
        foreach (var shortcut in session.GetFoodItemShortcutGateway().GetAllFoodItemShortcuts())
        {
            _foodItemShortcuts.Add(new FoodItemViewShortcut(shortcut));
        }

        _foodItemShortcuts = _foodItemShortcuts.OrderBy(x => x.Name).ToList();
    }
    
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

    [RelayCommand]
    private void PopulateFieldsWithFoodItemShortcut(FoodItemViewShortcut shortcut)
    {
        NewItemName = shortcut.Name;
        NewItemCalories = shortcut.Calories;
        
        CreateNewFoodItemShortcut = false;
    }
}