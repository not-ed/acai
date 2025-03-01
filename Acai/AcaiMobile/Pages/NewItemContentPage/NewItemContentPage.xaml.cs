namespace AcaiMobile;

public partial class NewItemContentPage : TabbedPage
{
    public NewItemContentPage()
    {
        InitializeComponent();
        BindingContext = new NewItemViewModel();
    }
    
    public void PopulateFields(string name, float calories, DateTime creationDate, float? protein, float? carbohydrates, float? fat, float? fibre, float? water)
    {
        ((NewItemViewModel)BindingContext).PopulateFields(name,calories,creationDate,protein,carbohydrates, fat, fibre, water);
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
    
    public float? GetSubmittedItemProtein()
    {
        return ((NewItemViewModel)BindingContext).NewItemProtein;
    }
    
    public float? GetSubmittedItemCarbohydrates()
    {
        return ((NewItemViewModel)BindingContext).NewItemCarbohydrates;
    }
    
    public float? GetSubmittedItemFat()
    {
        return ((NewItemViewModel)BindingContext).NewItemFat;
    }
    
    public float? GetSubmittedItemFibre()
    {
        return ((NewItemViewModel)BindingContext).NewItemFibre;
    }
    
    public float? GetSubmittedItemWater()
    {
        return ((NewItemViewModel)BindingContext).NewItemWater;
    }

    public bool ItemShortcutCreationIsRequested()
    {
        return ((NewItemViewModel)BindingContext).CreateNewFoodItemShortcut;
    }
}