namespace AcaiCore
{
    public interface IFoodItemShortcutGateway
    {
        public FoodItemShortcutDTO CreateNewFoodItemShortcut(string name, float calories);

        public void DeleteFoodItemShortcut(long id);

        public List<FoodItemShortcutDTO> GetAllFoodItemShortcuts();
    }

    public class FoodItemShortcutDTO
    {
        private long id;
        private string name;
        private float calories;

        public FoodItemShortcutDTO(long id, string name, float calories)
        {
            this.id = id;
            this.name = name;
            this.calories = calories;
        }
        
        public long GetID() { return id; }
        public string GetName() { return name; }
        public float GetCalories() { return calories; }
    }
}