namespace AcaiMobile;

public partial class NewItemContentPage : ContentPage
{
    public NewItemContentPage(NewItemViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    public bool HasBeenSubmitted()
    {
        return ((NewItemViewModel)BindingContext).Submitted;
    }

    public string GetSubmittedItemName()
    {
        return ((NewItemViewModel)BindingContext).NewItemName;
    }

    public float GetSubmittedItemCalories()
    {
        return ((NewItemViewModel)BindingContext).NewItemCalories;
    }

    public DateTime GetSubmittedItemCreationDate()
    {
        return ((NewItemViewModel)BindingContext).NewItemCreationDate;
    }

    public bool ItemShortcutCreationIsRequested()
    {
        return ((NewItemViewModel)BindingContext).CreateNewFoodItemShortcut;
    }
}