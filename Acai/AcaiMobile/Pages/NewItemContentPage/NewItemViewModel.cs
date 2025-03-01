using AcaiCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AcaiMobile;

public partial class FoodItemViewShortcut(FoodItemShortcutDTO shortcut) : ObservableObject
{
    [ObservableProperty] private long _id = shortcut.GetID();
    [ObservableProperty] private string _name = shortcut.GetName();
    [ObservableProperty] private float _calories = shortcut.GetCalories();
    [ObservableProperty] private float? _protein = shortcut.GetProtein();
    [ObservableProperty] private float? _carbohydrates = shortcut.GetCarbohydrates();
    [ObservableProperty] private float? _fat = shortcut.GetFat();
    [ObservableProperty] private float? _fibre = shortcut.GetFibre();
    [ObservableProperty] private float? _water = shortcut.GetWater();
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
    private bool _displayProteinField = Preferences.Get(PreferenceIndex.DisplayProtein.Key, PreferenceIndex.DisplayProtein.DefaultValue);
    [ObservableProperty]
    private bool _displayCarbohydratesField = Preferences.Get(PreferenceIndex.DisplayCarbohydrates.Key, PreferenceIndex.DisplayCarbohydrates.DefaultValue);
    [ObservableProperty]
    private bool _displayFatField = Preferences.Get(PreferenceIndex.DisplayFat.Key, PreferenceIndex.DisplayFat.DefaultValue);
    [ObservableProperty]
    private bool _displayFibreField = Preferences.Get(PreferenceIndex.DisplayFibre.Key, PreferenceIndex.DisplayFibre.DefaultValue);
    [ObservableProperty]
    private bool _displayWaterField = Preferences.Get(PreferenceIndex.DisplayWater.Key, PreferenceIndex.DisplayWater.DefaultValue);
    
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

    public void PopulateFields(string name, float calories, DateTime creationDate, float? protein, float? carbohydrates, float? fat, float? fibre, float? water)
    {
        NewItemName = name;
        NewItemCalories = calories;
        NewItemCreationDate = creationDate;
        NewItemProtein = protein;
        NewItemCarbohydrates = carbohydrates;
        NewItemFat = fat;
        NewItemFibre = fibre;
        NewItemWater = water;
    }
    
    [RelayCommand]
    private void PopulateFieldsWithFoodItemShortcut(FoodItemViewShortcut shortcut)
    {
        PopulateFields(shortcut.Name, shortcut.Calories, NewItemCreationDate, shortcut.Protein, shortcut.Carbohydrates, shortcut.Fat, shortcut.Fibre, shortcut.Water);
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