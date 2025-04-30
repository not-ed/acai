namespace AcaiMobile;

public partial class ItemEditorPage : TabbedPage
{
    public ItemEditorPage()
    {
        InitializeComponent();
        BindingContext = new ItemEditorViewModel();
    }

    protected override bool OnBackButtonPressed()
    {
        ((ItemEditorViewModel)BindingContext).OnBackButtonPressed();
        return true;
    }
    
    public void PopulateFields(string name, float calories, DateTime creationDate, float? protein, float? carbohydrates, float? fat, float? fibre, float? water)
    {
        ((ItemEditorViewModel)BindingContext).PopulateFields(name,calories,creationDate,protein,carbohydrates, fat, fibre, water);
    }
    
    public bool HasBeenSubmitted()
    {
        return ((ItemEditorViewModel)BindingContext).Submitted;
    }

    public string GetSubmittedItemName()
    {
        return ((ItemEditorViewModel)BindingContext).NewItemName;
    }

    public float GetSubmittedItemCalories()
    {
        return ((ItemEditorViewModel)BindingContext).NewItemCalories;
    }

    public DateTime GetSubmittedItemCreationDate()
    {
        return ((ItemEditorViewModel)BindingContext).NewItemCreationDate;
    }
    
    public float? GetSubmittedItemProtein()
    {
        return ((ItemEditorViewModel)BindingContext).NewItemProtein;
    }
    
    public float? GetSubmittedItemCarbohydrates()
    {
        return ((ItemEditorViewModel)BindingContext).NewItemCarbohydrates;
    }
    
    public float? GetSubmittedItemFat()
    {
        return ((ItemEditorViewModel)BindingContext).NewItemFat;
    }
    
    public float? GetSubmittedItemFibre()
    {
        return ((ItemEditorViewModel)BindingContext).NewItemFibre;
    }
    
    public float? GetSubmittedItemWater()
    {
        return ((ItemEditorViewModel)BindingContext).NewItemWater;
    }

    public bool ItemShortcutCreationIsRequested()
    {
        return ((ItemEditorViewModel)BindingContext).CreateNewFoodItemShortcut;
    }
}