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
    private float _totalCalories = 22;

    private int _tempTotal = 0;

    public FoodJournalViewModel()
    {
        ReinitializeFoodItemList();
    }

    [RelayCommand]
    public async void AddFoodItem()
    {
        _tempTotal++;
        _foodItemsList.Add(new FoodJournalViewItem(new FoodItemDTO(_tempTotal, $"ID {_tempTotal - 1}: DTO Item {_tempTotal} ({DateTime.Now})", _tempTotal * 100f, DateTime.Now)));
        UpdateTotalCalories();
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

    private void ReinitializeFoodItemList()
    {
        for (int i = 0; i < 10; i++)
        {
            _foodItemsList.Add(new FoodJournalViewItem(new FoodItemDTO(i, $"ID {i}: DTO Item {i + 1} ({DateTime.Now})", i * 100f, DateTime.Now)));
        }

        _tempTotal = 10;
        UpdateTotalCalories();
    }
    
    private void UpdateTotalCalories()
    {
        TotalCalories = _foodItemsList.Sum(x => x.Calories);
    }
    
}