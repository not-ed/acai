namespace AcaiMobile;

public partial class NewItemContentPage : ContentPage
{
    private bool _submitted = false;

    private string _enteredNewItemName = string.Empty;
    private float _enteredNewItemCalories = 0;
    private DateTime _enteredNewItemCreationDate = DateTime.Now;

	public NewItemContentPage()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        _submitted = false;
        ItemNameField.Text = string.Empty;
        CaloriesField.Text = string.Empty;
        ItemDateField.Date = DateTime.Now;
    }

    private void ValidateFields(object sender, EventArgs e)
    {
        var itemNameAndCaloriesFieldIsPopulated = ItemNameField.Text?.Length > 0 && CaloriesField.Text?.Length > 0;
        var caloriesFieldIsAValidValue = float.TryParse(CaloriesField.Text, out _);
        var allFieldsAreValid = itemNameAndCaloriesFieldIsPopulated && caloriesFieldIsAValidValue;

        if (allFieldsAreValid)
        {
            UpdatePopulatedData();
        }

        AddItemButton.IsEnabled = allFieldsAreValid;
    }

    private void UpdatePopulatedData()
    {
       _enteredNewItemName = ItemNameField.Text;
       _enteredNewItemCalories = float.Parse(CaloriesField.Text);
       _enteredNewItemCreationDate = ItemDateField.Date;
    }

    private void OnAddItemButtonClicked(object sender, EventArgs e)
    {
        _submitted = true;
        Navigation.PopModalAsync();
    }

    public bool HasBeenSubmitted()
    {
        return _submitted;
    }

    public string GetEnteredNewItemName()
    {
        return _enteredNewItemName;
    }

    public float GetEnteredNewItemCalories()
    {
        return _enteredNewItemCalories;
    }

    public DateTime GetNewItemCreationDate()
    {
        return _enteredNewItemCreationDate;
    }
}