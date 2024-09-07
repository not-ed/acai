namespace AcaiCore
{
    public interface IFoodItemGateway
    {
        public FoodItemDTO CreateNewFoodItem(string name, float calories, DateTime creationTime);

        public void DeleteFoodItem(long itemId);

        public FoodItemDTO UpdateExistingFoodItem(long itemId, string name, float calories, DateTime creationDate);

        public List<FoodItemDTO> GetFoodItemsForDate(DateTime date);
    }

    public class FoodItemDTO
    {
        private long id;
        private string name;
        private float calories;
        private DateTime creationDate;

        public FoodItemDTO(long id, string name, float calories, DateTime creationDate)
        {
            this.id = id;
            this.name = name;
            this.calories = calories;
            this.creationDate = creationDate;
        }

        public long GetID() { return id; }
        public string GetName() { return name; }
        public float GetCalories() { return calories; }
        public DateTime GetCreationDate() { return creationDate; }
    }
}
