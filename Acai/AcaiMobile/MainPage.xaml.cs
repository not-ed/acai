using System.Collections.ObjectModel;
using AcaiCore;

namespace AcaiMobile
{
    public class FoodItemViewData
    {
        public string name {get; set;}
        public float calories {get; set;}
        public DateTime creationDate { get; set;}

        public FoodItemViewData(FoodItemDTO foodItem)
        {
            this.name = foodItem.GetName();
            this.calories = foodItem.GetCalories();
            this.creationDate = foodItem.GetCreationDate();
        }

        public FoodItemViewData(string name, float calories, DateTime creationDate)
        {
            this.name = name;
            this.calories = calories;
            this.creationDate = creationDate;
        }
    }
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<FoodItemViewData> _foodItems = new ObservableCollection<FoodItemViewData>();

        public MainPage()
        {
            InitializeComponent();
            ItemListView.ItemsSource = _foodItems;
        }

        private void OnPageLoad(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                _foodItems.Add(new FoodItemViewData(new FoodItemDTO($"Test item {i}", i * 100, DateTime.Now)));
            }
        }

        private async void OnAddItemButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NewItemContentPage());

            _foodItems.Add(new FoodItemViewData(new FoodItemDTO($"Test item {_foodItems.Count}", _foodItems.Count * 100, DateTime.Now)));
        }
    }
}