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
    [ObservableProperty] private float _canonicalPounds = weighIn.GetCanonicalPounds();
    [ObservableProperty] private string _poundsDisplayWeight = string.Empty;
    [ObservableProperty] private string _stoneDisplayWeight = string.Empty;
    [ObservableProperty] private string _kilogramsDisplayWeight = string.Empty;
    [ObservableProperty] private float? _bodyFatPercentage = weighIn.GetBodyFatPercentage();
    [ObservableProperty] private string _note = weighIn.GetNote();
    [ObservableProperty] private bool _isExpanded = false;
    
    public void RefreshDisplayWeights()
    {
        float leftoverPounds = CanonicalPounds % 14;
        float stone = (CanonicalPounds - leftoverPounds) / 14;
        StoneDisplayWeight = $"{stone}st {Math.Round(leftoverPounds, 2)}lbs";

        KilogramsDisplayWeight = $"{Math.Round(CanonicalPounds * 0.453592, 2)}Kg";

        PoundsDisplayWeight = $"{Math.Round(CanonicalPounds, 2)}lbs";
    }
}

public partial class WeightJournalPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<WeightJournalViewItem> _entryList = new ObservableCollection<WeightJournalViewItem>();

    [ObservableProperty]
    private bool _displayInPounds = false;
    [ObservableProperty]
    private bool _displayInStone = false;
    [ObservableProperty]
    private bool _displayInKilograms = false;
    
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
            var entry = new WeightJournalViewItem(weighIn);
            entry.RefreshDisplayWeights();
            EntryList.Add(entry);
        }
    }

    [RelayCommand]
    public void RefreshPreferredFormatting()
    {
        string preferredUnitOfMeasurement = Preferences.Get(PreferenceIndex.WeighInUnitOfMeasurement.Key, PreferenceIndex.WeighInUnitOfMeasurement.DefaultValue);
        DisplayInStone = preferredUnitOfMeasurement == "1";
        DisplayInKilograms = preferredUnitOfMeasurement == "2";
        DisplayInPounds = (!DisplayInStone && !DisplayInKilograms);
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
        var weighInEditorPage = new WeighInEditorPage();
        weighInEditorPage.PopulateFields(
            selectedItem.CreationDate,
            selectedItem.CanonicalPounds,
            selectedItem.BodyFatPercentage);

        await Shell.Current.CurrentPage.ShowPopupAsync(weighInEditorPage);

        if (weighInEditorPage.HasBeenSubmitted())
        {
            var session = await AcaiSessionSingleton.Get();
            session.GetWeightJournalGateway().UpdateExistingWeighIn(
                selectedItem.Id,
                weighInEditorPage.GetSubmittedDate(),
                weighInEditorPage.GetSubmittedPounds(),
                weighInEditorPage.GetSubmittedBodyFat(),
                null);

            ReinitializeEntriesList();
        }
    }
}