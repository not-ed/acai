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

public partial class FoodJournalDateViewPage : ObservableObject
{
    private int _lastCarouselIndex = 0;
    
    [ObservableProperty]
    private ObservableCollection<FoodJournalViewItem> _foodItemsList = new ObservableCollection<FoodJournalViewItem>();

    [ObservableProperty]
    private DateTime _selectedDate;

    [ObservableProperty]
    private float _totalCalories = 0;

    public FoodJournalDateViewPage(DateTime selectedDate)
    {
        _selectedDate = selectedDate;
        
        ReinitializeFoodItemList();
    }
    
    private async void ReinitializeFoodItemList()
    {
        FoodItemsList.Clear();
        var session = await AcaiSessionSingleton.Get(Shell.Current.CurrentPage);
        
        foreach (var foodItem in session.GetFoodItemGateway().GetFoodItemsForDate(SelectedDate))
        {
            FoodItemsList.Add(new FoodJournalViewItem(foodItem));
        }
        
        UpdateTotalCalories();
    }
    
    private void UpdateTotalCalories()
    {
        TotalCalories = FoodItemsList.Sum(x => x.Calories);
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
        var itemPendingDeletion = FoodItemsList.FirstOrDefault(x => x.Id == itemId.Id);
        if (itemPendingDeletion != null)
        {
            var session = await AcaiSessionSingleton.Get(Shell.Current.CurrentPage);
            session.GetFoodItemGateway().DeleteFoodItem(itemPendingDeletion.Id);
            FoodItemsList.Remove(itemPendingDeletion);
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
}

public partial class FoodJournalViewModel : ObservableObject
{
    [ObservableProperty]
    private FoodJournalDateViewPage[] _foodJournalDateViewPages = new FoodJournalDateViewPage[3]
    {
        new FoodJournalDateViewPage(DateTime.Now),
        new FoodJournalDateViewPage(DateTime.Now.AddDays(1)),
        new FoodJournalDateViewPage(DateTime.Now.AddDays(-1)),
    };

    private int _lastCarouselIndex = 0;

    // [RelayCommand]
    // public void OnCarouselSwipe(int newIndex)
    // {
    //     var newCarouselIndex = newIndex;
    //     var swipedRight = ((newCarouselIndex > _lastCarouselIndex) && (_lastCarouselIndex != 0 || newCarouselIndex != 2)) || (_lastCarouselIndex == 2 && newCarouselIndex == 0);
    //
    //     if (swipedRight)
    //     {
    //         SelectedDate = _selectedDate.AddDays(1);
    //     }
    //     else
    //     {
    //         SelectedDate = _selectedDate.AddDays(-1);
    //     }
    //
    //     ReinitializeFoodItemList();
    //     _lastCarouselIndex = newCarouselIndex;
    // }
    
    // [RelayCommand]
    // public void OnDateTap()
    // {
    //     SelectedDate = DateTime.Now;
    //     ReinitializeFoodItemList();
    // }
    
}