using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AcaiCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AcaiMobile;

public partial class FoodJournalViewItem(FoodItemDTO item) : ObservableObject
{
    [ObservableProperty] private long _id = item.GetID();
    [ObservableProperty] private string _name = item.GetName();
    [ObservableProperty] private float _calories = item.GetCalories();
    [ObservableProperty] private DateTime _creationDate = item.GetCreationDate();
}

public partial class FoodJournalViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<FoodJournalViewItem> _foodItemsList = new ObservableCollection<FoodJournalViewItem>();

    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Now;

    [ObservableProperty]
    private float _totalCalories = 0;

    public FoodJournalViewModel()
    {
        ReinitializeFoodItemList();
    }

    [RelayCommand]
    public async void AddFoodItem()
    {
        var newItemPage = new NewItemContentPage();
        await Shell.Current.Navigation.PushModalAsync(newItemPage, true);
        newItemPage.Disappearing += async (object sender, EventArgs eventArgs) =>
        {
            if (newItemPage.HasBeenSubmitted())
            {
                var session = await AcaiSessionSingleton.Get(Shell.Current.CurrentPage);
                var newItemDto = session.GetFoodItemGateway().CreateNewFoodItem(newItemPage.GetEnteredNewItemName(), newItemPage.GetEnteredNewItemCalories(), newItemPage.GetNewItemCreationDate());
                _foodItemsList.Add(new FoodJournalViewItem(newItemDto));
                UpdateTotalCalories();
            }
        };
    }

    public void DeleteFoodItem(FoodJournalViewItem itemId)
    {
        var itemPendingDeletion = _foodItemsList.FirstOrDefault(x => x.Id == itemId.Id);
        if (itemPendingDeletion != null)
        {
            _foodItemsList.Remove(itemPendingDeletion);
        }
        UpdateTotalCalories();
    }

    [RelayCommand]
    public async void DisplayItemActions(FoodJournalViewItem selectedItem)
    {
        var result = await Shell.Current.DisplayAlert("Delete Entry?", $"Delete {selectedItem.Name}?", "Yes", "No");
        if (result)
        {
            DeleteFoodItem(selectedItem);
        }
    }

    private async void ReinitializeFoodItemList()
    {
        _foodItemsList.Clear();
        var session = await AcaiSessionSingleton.Get(Shell.Current.CurrentPage);
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