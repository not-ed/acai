using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AcaiMobile;

public partial class WeightJournalViewItem(DateTime creationDate, string displayWeight, float bodyFatPercentage) : ObservableObject
{
    [ObservableProperty] private DateTime _creationDate = creationDate;
    [ObservableProperty] private string _displayWeight = displayWeight;
    [ObservableProperty] private float _bodyFatPercentage = bodyFatPercentage;
    [ObservableProperty] private string _note = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc nec magna id lorem sollicitudin gravida. Integer molestie euismod arcu, vel.";
    [ObservableProperty] private bool _isExpanded = false;
}

public partial class WeightJournalPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<WeightJournalViewItem> _entryList = new ObservableCollection<WeightJournalViewItem>();

    public WeightJournalPageViewModel()
    {
        ReinitializeEntriesList();
    }
    
    private async void ReinitializeEntriesList()
    {
        EntryList.Clear();
        
        EntryList.Add(new WeightJournalViewItem(DateTime.Now,"12st 9lbs", 30));
        EntryList.Add(new WeightJournalViewItem(DateTime.Now.AddDays(-7),"80.2kg", 29.4f));
        EntryList.Add(new WeightJournalViewItem(DateTime.Now.AddDays(-14),"177lbs", 29.6f));
        EntryList.Add(new WeightJournalViewItem(DateTime.Now.AddDays(-21),"177lbs", 29.6f));
        EntryList.Add(new WeightJournalViewItem(DateTime.Now.AddDays(-28),"177lbs", 29.6f));
        EntryList.Add(new WeightJournalViewItem(DateTime.Now.AddDays(-35),"177lbs", 29.6f));
        EntryList.Add(new WeightJournalViewItem(DateTime.Now.AddDays(-42),"177lbs", 29.6f));
        EntryList.Add(new WeightJournalViewItem(DateTime.Now.AddDays(-49),"177lbs", 29.6f));
    }
    
    [RelayCommand]
    public void ToggleItemExpansion(WeightJournalViewItem selectedItem)
    {
        var selectedItemIsAlreadyExpanded = selectedItem.IsExpanded;
        foreach (var entry in EntryList)
        {
            entry.IsExpanded = false;
        }
        selectedItem.IsExpanded = !selectedItemIsAlreadyExpanded;
    }
    
    [RelayCommand]
    public async void DisplayAndProcessWeighInCreation()
    {
        var result = await Shell.Current.DisplayPromptAsync("STUB", "New Entry");
    }
    
    [RelayCommand]
    public async void PromptItemDeletion(WeightJournalViewItem selectedItem)
    {
        var result = await Shell.Current.DisplayPromptAsync("STUB", $"Entry Deletion: {selectedItem.CreationDate}");
    }
    
    [RelayCommand]
    public async void EditItem(WeightJournalViewItem selectedItem)
    {
        var result = await Shell.Current.DisplayPromptAsync("STUB", $"Entry Edit: {selectedItem.CreationDate}");
    }
}