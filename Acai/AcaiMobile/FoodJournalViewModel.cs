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
        var newItemPage = new NewItemContentPage(new NewItemViewModel());
        await Shell.Current.Navigation.PushModalAsync(newItemPage, true);
        newItemPage.Disappearing += async (object sender, EventArgs eventArgs) =>
        {
            if (newItemPage.HasBeenSubmitted())
            {
                var session = await AcaiSessionSingleton.Get(Shell.Current.CurrentPage);
                var newItemDto = session.GetFoodItemGateway().CreateNewFoodItem(newItemPage.GetSubmittedItemName(), newItemPage.GetSubmittedItemCalories(), newItemPage.GetSubmittedItemCreationDate());
                ReinitializeFoodItemList();
        
                if (newItemPage.ItemShortcutCreationIsRequested())
                {
                    session.GetFoodItemShortcutGateway().CreateNewFoodItemShortcut(newItemPage.GetSubmittedItemName(), newItemPage.GetSubmittedItemCalories());
                }
            }
        };
    }

    public async void DeleteFoodItem(FoodJournalViewItem itemId)
    {
        var itemPendingDeletion = _foodItemsList.FirstOrDefault(x => x.Id == itemId.Id);
        if (itemPendingDeletion != null)
        {
            var session = await AcaiSessionSingleton.Get(Shell.Current.CurrentPage);
            session.GetFoodItemGateway().DeleteFoodItem(itemPendingDeletion.Id);
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

    [RelayCommand]
    public void OnDateTap()
    {
        SelectedDate = DateTime.Now;
        ReinitializeFoodItemList();
    }

    [RelayCommand]
    public void OnSwipeLeft()
    {
        SelectedDate = _selectedDate.AddDays(-1);
        ReinitializeFoodItemList();
    }

    [RelayCommand]
    public void OnSwipeRight()
    {
        SelectedDate = _selectedDate.AddDays(1);
        ReinitializeFoodItemList();
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