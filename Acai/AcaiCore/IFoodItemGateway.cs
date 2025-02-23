namespace AcaiCore
{
    public interface IFoodItemGateway
    {
        public FoodItemDTO CreateNewFoodItem(string name, float calories, DateTime creationTime, float? protein, float? carbohydrates, float? fat, float? fibre, float? water);

        public void DeleteFoodItem(long itemId);

        public FoodItemDTO UpdateExistingFoodItem(long itemId, string name, float calories, DateTime creationDate,  float? protein, float? carbohydrates, float? fat, float? fibre, float? water);

        public List<FoodItemDTO> GetFoodItemsForDate(DateTime date);
    }

    public class FoodItemDTO
    {
        private long id;
        private string name;
        private float calories;
        private DateTime creationDate;
        private float? protein;
        private float? carbohydrates;
        private float? fat;
        private float? fibre;
        private float? water;

        public FoodItemDTO(long id, string name, float calories, DateTime creationDate, float? protein, float? carbohydrates, float? fat, float? fibre, float? water)
        {
            this.id = id;
            this.name = name;
            this.calories = calories;
            this.creationDate = creationDate;
            this.protein = protein;
            this.carbohydrates = carbohydrates;
            this.fat = fat;
            this.fibre = fibre;
            this.water = water;
        }

        public long GetID() { return id; }
        public string GetName() { return name; }
        public float GetCalories() { return calories; }
        public DateTime GetCreationDate() { return creationDate; }
        
        public float? GetProtein() { return protein; }
        public float? GetCarbohydrates() { return carbohydrates; }
        public float? GetFat() { return fat; }
        public float? GetFibre() { return fibre; }
        public float? GetWater() { return water; }
    }
}
