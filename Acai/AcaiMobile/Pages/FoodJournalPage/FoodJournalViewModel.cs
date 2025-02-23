using System.Collections.ObjectModel;
using AcaiCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AcaiMobile;

public partial class FoodJournalViewItem(FoodItemDTO item) : ObservableObject
{
    [ObservableProperty] private long _id = item.GetID();
    [ObservableProperty] private string _name = item.GetName();
    [ObservableProperty] private float _calories = item.GetCalories();
    [ObservableProperty] private float? _protein = item.GetProtein();
    [ObservableProperty] private float? _carbohydrates = item.GetCarbohydrates();
    [ObservableProperty] private float? _fat = item.GetFat();
    [ObservableProperty] private float? _fibre = item.GetFibre();
    [ObservableProperty] private float? _water = item.GetWater();
    [ObservableProperty] private DateTime _creationDate = item.GetCreationDate();
    [ObservableProperty] private bool _isExpanded = false;
}

public partial class FoodJournalViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<FoodJournalViewItem> _foodItemsList = new ObservableCollection<FoodJournalViewItem>();

    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Now;

    [ObservableProperty]
    private float _totalCalories = 0;
    [ObservableProperty] 
    private float _caloricLimit = 0;
    
    [ObservableProperty]
    private bool _displayItemProtein = false;
    [ObservableProperty]
    private bool _displayItemCarbohydrates = false;
    [ObservableProperty]
    private bool _displayItemFat = false;
    
    public FoodJournalViewModel()
    {
        ReinitializeFoodItemList();
    }

    [RelayCommand]
    public void OnPageAppear()
    {
        ReinitializeFoodItemList();
        CaloricLimit = Preferences.Get(PreferenceIndex.DailyCaloricLimit.Key, PreferenceIndex.DailyCaloricLimit.DefaultValue);
        DisplayItemProtein = Preferences.Get(PreferenceIndex.DisplayProtein.Key, PreferenceIndex.DisplayProtein.DefaultValue);
        DisplayItemCarbohydrates = Preferences.Get(PreferenceIndex.DisplayCarbohydrates.Key, PreferenceIndex.DisplayCarbohydrates.DefaultValue);
        DisplayItemFat = Preferences.Get(PreferenceIndex.DisplayFat.Key, PreferenceIndex.DisplayFat.DefaultValue);
    }

    [RelayCommand]
    public async void AddFoodItem()
    {
        var newItemPage = new NewItemContentPage(new NewItemViewModel());
        await Shell.Current.Navigation.PushModalAsync(newItemPage, true);
        newItemPage.Disappearing += async (object sender, EventArgs eventArgs) =>
        {
            if (newItemPage.HasBeenSubmitted())
            {
                var session = await AcaiSessionSingleton.Get();
                session.GetFoodItemGateway().CreateNewFoodItem(
                    newItemPage.GetSubmittedItemName(),
                    newItemPage.GetSubmittedItemCalories(),
                    newItemPage.GetSubmittedItemCreationDate(),
                    newItemPage.GetSubmittedItemProtein(),
                    newItemPage.GetSubmittedItemCarbohydrates(),
                    newItemPage.GetSubmittedItemFat(),
                    newItemPage.GetSubmittedItemFibre(),
                    newItemPage.GetSubmittedItemWater());
                ReinitializeFoodItemList();
        
                if (newItemPage.ItemShortcutCreationIsRequested())
                {
                    session.GetFoodItemShortcutGateway().CreateNewFoodItemShortcut(
                        newItemPage.GetSubmittedItemName(), 
                        newItemPage.GetSubmittedItemCalories(),
                        newItemPage.GetSubmittedItemProtein(),
                        newItemPage.GetSubmittedItemCarbohydrates(),
                        newItemPage.GetSubmittedItemFat(),
                        newItemPage.GetSubmittedItemFibre(),
                        newItemPage.GetSubmittedItemWater());
                }
            }
        };
    }

    public async void DeleteFoodItem(FoodJournalViewItem itemId)
    {
        var itemPendingDeletion = _foodItemsList.FirstOrDefault(x => x.Id == itemId.Id);
        if (itemPendingDeletion != null)
        {
            var session = await AcaiSessionSingleton.Get();
            session.GetFoodItemGateway().DeleteFoodItem(itemPendingDeletion.Id);
            _foodItemsList.Remove(itemPendingDeletion);
        }
        UpdateTotalCalories();
    }

    [RelayCommand]
    public void ToggleItemExpansion(FoodJournalViewItem selectedItem)
    {
        var selectedItemIsAlreadyExpanded = selectedItem.IsExpanded;
        foreach (var item in _foodItemsList)
        {
            item.IsExpanded = false;
        }
        selectedItem.IsExpanded = !selectedItemIsAlreadyExpanded;
    }
    
    [RelayCommand]
    public async void PromptItemDeletion(FoodJournalViewItem selectedItem)
    {
        var result = await Shell.Current.DisplayAlert("Delete Entry?", $"Delete {selectedItem.Name}?", "Yes", "No");
        if (result)
        {
            DeleteFoodItem(selectedItem);
        }
    }

    [RelayCommand]
    public void ProgressSelectedDateByNumberOfDays(string days)
    {
        SelectedDate = SelectedDate.AddDays(int.Parse(days));
        ReinitializeFoodItemList();
    }

    [RelayCommand]
    public void ReturnSelectedDateToNow()
    {
        SelectedDate = DateTime.Now;
        ReinitializeFoodItemList();
    }

    private async void ReinitializeFoodItemList()
    {
        _foodItemsList.Clear();
        var session = await AcaiSessionSingleton.Get();
        foreach (var foodItem in session.GetFoodItemGateway().GetFoodItemsForDate(_selectedDate))
        {
            _foodItemsList.Add(new FoodJournalViewItem(foodItem));
        }
        UpdateTotalCalories();
    }
    
    private void UpdateTotalCalories()
    {
        TotalCalories = _foodItemsList.Sum(x => x.Calories);
    }
    
}