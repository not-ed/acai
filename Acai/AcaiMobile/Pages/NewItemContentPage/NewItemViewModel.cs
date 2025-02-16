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
    [ObservableProperty] 
    private float _newItemCalories = 0;
    [ObservableProperty]
    private DateTime _newItemCreationDate = DateTime.Now;
    [ObservableProperty]
    private float? _newItemProtein = null;
    [ObservableProperty]
    private float? _newItemCarbohydrates = null;
    [ObservableProperty]
    private float? _newItemFat = null;
    [ObservableProperty]
    private float? _newItemFibre = null;
    [ObservableProperty]
    private float? _newItemWater = null;
    
    [ObservableProperty]
    private bool _createNewFoodItemShortcut = false;
    
    [ObservableProperty]
    private List<FoodItemViewShortcut> _foodItemShortcutResults;
    private List<FoodItemViewShortcut> _allFoodItemShortcuts;

    public NewItemViewModel()
    {
        var session = AcaiSessionSingleton.Get().Result;
        _allFoodItemShortcuts = new List<FoodItemViewShortcut>();
        foreach (var shortcut in session.GetFoodItemShortcutGateway().GetAllFoodItemShortcuts())
        {
            _allFoodItemShortcuts.Add(new FoodItemViewShortcut(shortcut));
        }
        DisplayAllFoodItemShortcutsInResults();
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
        ReturnToFirstPage();
    }

    private void ReturnToFirstPage()
    {
        var tabbedPage = (TabbedPage)Shell.Current.CurrentPage;
        tabbedPage.CurrentPage = tabbedPage.Children[0];
    }

    [RelayCommand]
    private void SearchFoodItemShortcuts(string searchQuery)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            DisplayAllFoodItemShortcutsInResults();
        }
        else
        {
            DisplayFoodItemShortcutsMatchingQuery(searchQuery);
        }
    }

    private void DisplayAllFoodItemShortcutsInResults()
    {
        FoodItemShortcutResults = _allFoodItemShortcuts.OrderBy(x => x.Name).ToList();
    }

    private void DisplayFoodItemShortcutsMatchingQuery(string searchQuery)
    {
        FoodItemShortcutResults = _allFoodItemShortcuts
            .Where(x => x.Name.ToLower().Contains(searchQuery.ToLower()))
            .OrderBy(x => x.Name.ToLower().Replace(searchQuery.ToLower(), string.Empty).Length)
            .ThenBy(x => x.Name)
            .ToList();
    }
}