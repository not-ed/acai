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
    private string _note = string.Empty;
    [ObservableProperty]
    private bool _displayNote = false;
    
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
    [ObservableProperty]
    private bool _displayItemFibre = false;
    [ObservableProperty]
    private bool _displayItemWater = false;
    
    public FoodJournalViewModel()
    {
        RefreshNote();
        ReinitializeFoodItemList();
    }

    [RelayCommand]
    public void OnPageAppear()
    {
        RefreshNote();
        ReinitializeFoodItemList();
        CaloricLimit = Preferences.Get(PreferenceIndex.DailyCaloricLimit.Key, PreferenceIndex.DailyCaloricLimit.DefaultValue);
        DisplayItemProtein = Preferences.Get(PreferenceIndex.DisplayProtein.Key, PreferenceIndex.DisplayProtein.DefaultValue);
        DisplayItemCarbohydrates = Preferences.Get(PreferenceIndex.DisplayCarbohydrates.Key, PreferenceIndex.DisplayCarbohydrates.DefaultValue);
        DisplayItemFat = Preferences.Get(PreferenceIndex.DisplayFat.Key, PreferenceIndex.DisplayFat.DefaultValue);
        DisplayItemFibre = Preferences.Get(PreferenceIndex.DisplayFibre.Key, PreferenceIndex.DisplayFibre.DefaultValue);
        DisplayItemWater = Preferences.Get(PreferenceIndex.DisplayWater.Key, PreferenceIndex.DisplayWater.DefaultValue);
    }

    [RelayCommand]
    public void AddFoodItem()
    {
        var newItemPage = new ItemEditorPage();
        DisplayAndProcessNewItemContentPage(newItemPage);
    }
    
    [RelayCommand]
    public void CopyFoodItem(FoodJournalViewItem selectedItem)
    {
        var copyItemPage = new ItemEditorPage();
        copyItemPage.PopulateFields(
            selectedItem.Name,
            selectedItem.Calories,
            selectedItem.CreationDate,
            selectedItem.Protein,
            selectedItem.Carbohydrates,
            selectedItem.Fat,
            selectedItem.Fibre,
            selectedItem.Water);
        DisplayAndProcessNewItemContentPage(copyItemPage);
    }

    [RelayCommand]
    public void EditFoodItem(FoodJournalViewItem selectedItem)
    {
        var editItemPage = new ItemEditorPage();
        editItemPage.PopulateFields(
            selectedItem.Name,
            selectedItem.Calories,
            selectedItem.CreationDate,
            selectedItem.Protein,
            selectedItem.Carbohydrates,
            selectedItem.Fat,
            selectedItem.Fibre,
            selectedItem.Water);
        
        DisplayAndProcessEditItemContentPage(editItemPage, selectedItem);
    }

    private async void DisplayAndProcessEditItemContentPage(ItemEditorPage editItemPage, FoodJournalViewItem selectedItem)
    {
        await Shell.Current.Navigation.PushModalAsync(editItemPage, true);
        editItemPage.Disappearing += async (object sender, EventArgs eventArgs) =>
        {
            var session = await AcaiSessionSingleton.Get();
            session.GetFoodItemGateway().UpdateExistingFoodItem(
                selectedItem.Id,
                editItemPage.GetSubmittedItemName(),
                editItemPage.GetSubmittedItemCalories(),
                editItemPage.GetSubmittedItemCreationDate(),
                editItemPage.GetSubmittedItemProtein(),
                editItemPage.GetSubmittedItemCarbohydrates(),
                editItemPage.GetSubmittedItemFat(),
                editItemPage.GetSubmittedItemFibre(),
                editItemPage.GetSubmittedItemWater());
            ReinitializeFoodItemList();
        
            ProcessItemShortcutCreation(editItemPage, session);
        };
    }
    
    private async void DisplayAndProcessNewItemContentPage(ItemEditorPage newItemPage)
    {
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
        
                ProcessItemShortcutCreation(newItemPage, session);
            }
        };
    }
    
    private void ProcessItemShortcutCreation(ItemEditorPage submittedItemPage, AcaiSession session)
    {
        if (submittedItemPage.ItemShortcutCreationIsRequested())
        {
            session.GetFoodItemShortcutGateway().CreateNewFoodItemShortcut(
                submittedItemPage.GetSubmittedItemName(), 
                submittedItemPage.GetSubmittedItemCalories(),
                submittedItemPage.GetSubmittedItemProtein(),
                submittedItemPage.GetSubmittedItemCarbohydrates(),
                submittedItemPage.GetSubmittedItemFat(),
                submittedItemPage.GetSubmittedItemFibre(),
                submittedItemPage.GetSubmittedItemWater());
        }
    }
    
    public async void DeleteFoodItem(FoodJournalViewItem selectedItem)
    {
        var itemPendingDeletion = _foodItemsList.FirstOrDefault(x => x.Id == selectedItem.Id);
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
        RefreshNote();
        ReinitializeFoodItemList();
    }

    [RelayCommand]
    public void ReturnSelectedDateToNow()
    {
        SelectedDate = DateTime.Now;
        RefreshNote();
        ReinitializeFoodItemList();
    }

    [RelayCommand]
    public async void EditNote()
    {
        var newNote = await Shell.Current.DisplayPromptAsync("Edit Note","","Save","Cancel",initialValue:Note);
        if (newNote != null)
        {
            var session = await AcaiSessionSingleton.Get();
            session.GetFoodJournalNoteGateway().CreateOrUpdateNoteForDate(SelectedDate, newNote);
            RefreshNote();
        }
    }

    private async void RefreshNote()
    {
        var session = await AcaiSessionSingleton.Get();
        var noteRecord = session.GetFoodJournalNoteGateway().GetNoteForDate(SelectedDate);
        Note = noteRecord?.GetContent();
        DisplayNote = !string.IsNullOrEmpty(Note);
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