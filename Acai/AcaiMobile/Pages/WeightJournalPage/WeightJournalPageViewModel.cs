using System.Collections.ObjectModel;
using AcaiCore;
using CommunityToolkit.Maui.Views;
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
        foreach (var weighIn in session.GetWeightJournalGateway().GetAllWeighIns().OrderByDescending(x => x.GetCreationDate()))
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
        var weighInEditorPage = new WeighInEditorPage();
        await Shell.Current.CurrentPage.ShowPopupAsync(weighInEditorPage);

        if (weighInEditorPage.HasBeenSubmitted())
        {
            var session = await AcaiSessionSingleton.Get();
            session.GetWeightJournalGateway().CreateNewWeighIn(
                weighInEditorPage.GetSubmittedDate(),
                weighInEditorPage.GetSubmittedPounds(),
                weighInEditorPage.GetSubmittedBodyFat(),
                null);

            ReinitializeEntriesList();
        }
    }
    
    [RelayCommand]
    public async void PromptItemDeletion(WeightJournalViewItem selectedItem)
    {
        var result = await Shell.Current.DisplayAlert("Delete Weigh-in?", $"Delete Weigh-in for {selectedItem.CreationDate.ToString("ddd d MMM yyyy")}?", "Yes", "No");
        if (result)
        {
            var session = await AcaiSessionSingleton.Get();
            session.GetWeightJournalGateway().DeleteWeighIn(selectedItem.Id);

            var deletedItem = EntryList.FirstOrDefault(x => x.Id == selectedItem.Id);
            if (deletedItem != null)
            {
                EntryList.Remove(deletedItem);
            }
        }
    }
    
    [RelayCommand]
    public async void EditItem(WeightJournalViewItem selectedItem)
    {
        var result = await Shell.Current.DisplayPromptAsync("STUB", $"Entry Edit: {selectedItem.CreationDate}");
    }
}