namespace AcaiCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Is this thing on?");
            IFoodItemGateway foodItemGateway = new FoodItemGateway(new SqliteConnectionFactory());

            string name = Console.ReadLine();
            float calories = float.Parse(Console.ReadLine());

            var createNewFoodItemResult = foodItemGateway.CreateNewFoodItem(name, calories, DateTime.Now);

            Console.WriteLine("Inserted New Item:");
            Console.WriteLine($"  NAME: '{createNewFoodItemResult.GetName()}'");
            Console.WriteLine($"  CALS: '{createNewFoodItemResult.GetCalories()}'");
            Console.WriteLine($"  DATE: {createNewFoodItemResult.GetCreationDate().Day} {createNewFoodItemResult.GetCreationDate().Month} {createNewFoodItemResult.GetCreationDate().Year} ({createNewFoodItemResult.GetCreationDate().Hour}:{createNewFoodItemResult.GetCreationDate().Minute}:{createNewFoodItemResult.GetCreationDate().Second})");
        }
    }
}
