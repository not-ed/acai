namespace AcaiMobile;

public partial class NewItemContentPage : ContentPage
{
	public NewItemContentPage()
	{
		InitializeComponent();
	}

    private void ValidateFields(object sender, EventArgs e)
    {
        var itemNameAndCaloriesFieldIsPopulated = ItemNameField.Text?.Length > 0 && CaloriesField.Text?.Length > 0;
        AddItemButton.IsEnabled = itemNameAndCaloriesFieldIsPopulated;
    }

    private void OnAddItemButtonClicked(object sender, EventArgs e)
    {
        Console.WriteLine($"NAME: {ItemNameField.Text}");
        Console.WriteLine($"CALS: {CaloriesField.Text}");
        Console.WriteLine($"DATE: {ItemDateField.Date}");
        Navigation.PopModalAsync();
    }
}