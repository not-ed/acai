using System.Collections.ObjectModel;
using AcaiCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AcaiMobile;

public partial class WeightJournalViewItem(WeightJournalEntryDTO weighIn) : ObservableObject
{
    [ObservableProperty] private long _id = weighIn.GetID();
    [ObservableProperty] private DateTime _creationDate = weighIn.GetCreationDate();
    [ObservableProperty] private string _displayWeight = $"{weighIn.GetCanonicalPounds()}lbs";
    [ObservableProperty] private float? _bodyFatPercentage = weighIn.GetBodyFatPercentage();
    [ObservableProperty] private string _note = weighIn.GetNote();
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
        
        var session = await AcaiSessionSingleton.Get();
        foreach (var weighIn in session.GetWeightJournalGateway().GetAllWeighIns())
        {
            EntryList.Add(new WeightJournalViewItem(weighIn));
        }
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
        var session = await AcaiSessionSingleton.Get();
        
        //STUB
        var r = new Random();
        var lbs = 120 + r.NextSingle() % 45;
        var bodyFat = r.NextSingle() % 35;
        var ignoreFat = r.Next() % 2 == 0;
        var ignoreNote = r.Next() % 2 == 0;
        session.GetWeightJournalGateway().CreateNewWeighIn(DateTime.Now, lbs, ignoreFat ? null : bodyFat, ignoreNote ? null : $"Test note. IGNORE FAT: {ignoreFat}. IGNORE NOTE: {ignoreNote}");
        
        ReinitializeEntriesList();
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