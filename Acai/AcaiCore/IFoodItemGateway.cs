namespace AcaiCore
{
    public interface IFoodItemGateway
    {
        public FoodItemDTO CreateNewFoodItem(string name, float calories, DateTime creationTime);
        public List<FoodItemDTO> GetFoodItemsForDate(DateTime date);
    }

    public class FoodItemDTO
    {
        private string name;
        private float calories;
        private DateTime creationDate;

        public FoodItemDTO(string name, float calories, DateTime creationDate)
        {
            this.name = name;
            this.calories = calories;
            this.creationDate = creationDate;
        }

        public string GetName() { return name; }
        public float GetCalories() { return calories; }
        public DateTime GetCreationDate() { return creationDate; }
    }
}
