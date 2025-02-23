namespace AcaiCore
{
    public interface IFoodItemShortcutGateway
    {
        public FoodItemShortcutDTO CreateNewFoodItemShortcut(string name, float calories, float? protein, float? carbohydrates, float? fat, float? fibre, float? water);

        public void DeleteFoodItemShortcut(long id);

        public List<FoodItemShortcutDTO> GetAllFoodItemShortcuts();
    }

    public class FoodItemShortcutDTO
    {
        private long id;
        private string name;
        private float calories;
        private float? protein;
        private float? carbohydrates;
        private float? fat;
        private float? fibre;
        private float? water;

        public FoodItemShortcutDTO(long id, string name, float calories, float? protein, float? carbohydrates, float? fat, float? fibre, float? water)
        {
            this.id = id;
            this.name = name;
            this.calories = calories;
            this.protein = protein;
            this.carbohydrates = carbohydrates;
            this.fat = fat;
            this.fibre = fibre;
            this.water = water;
        }
        
        public long GetID() { return id; }
        public string GetName() { return name; }
        public float GetCalories() { return calories; }
        public float? GetProtein() { return protein; }
        public float? GetCarbohydrates() { return carbohydrates; }
        public float? GetFat() { return fat; }
        public float? GetFibre() { return fibre; }
        public float? GetWater() { return water; }
    }
}