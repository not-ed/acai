using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private NewItemContentPage _newItemModal = new NewItemContentPage();
        private ObservableCollection<FoodItemViewData> _foodItems = new ObservableCollection<FoodItemViewData>();
        private float _totalCalories = 0;

        public MainPage()
        {
            InitializeComponent();
            _foodItems.CollectionChanged += UpdateCaloricTotal;
            _newItemModal.Disappearing += OnNewItemModalDismissed;
            ItemListView.ItemsSource = _foodItems;
        }

        private void UpdateCaloricTotal(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            _totalCalories = _foodItems.Sum(x => x.calories);
            TotalCaloriesLabel.Text = $"Total: {_totalCalories.ToString()} kcal";
        }

        private async void OnAddItemButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(_newItemModal);
        }

        private void OnNewItemModalDismissed(object sender, EventArgs eventArgs)
        {
            if (_newItemModal.HasBeenSubmitted())
            {
                _foodItems.Add(new FoodItemViewData(
                    new FoodItemDTO(
                        _newItemModal.GetEnteredNewItemName(),
                        _newItemModal.GetEnteredNewItemCalories(),
                        _newItemModal.GetNewItemCreationDate())
                ));
            }
        }
    }
}