namespace AcaiCore
{
    class Program
    {
        static void Main(string[] args)
        {
            IFoodItemGateway foodItemGateway = new FoodItemGateway(new SqliteConnectionFactory());

            Console.WriteLine("Heres your journal for today:");
            foreach (var item in foodItemGateway.GetFoodItemsForDate(DateTime.Now))
            {
                Console.WriteLine($" - {item.GetName()} | {item.GetCalories()} cals | {item.GetCreationDate()}");
            }
            Console.WriteLine("\n");

            Console.WriteLine("Write name of new item to add:");
            string name = Console.ReadLine();
            Console.WriteLine("Write number of calories:");
            float calories = float.Parse(Console.ReadLine());
            var createNewFoodItemResult = foodItemGateway.CreateNewFoodItem(name, calories, DateTime.Now);

            Console.WriteLine("Inserted New Item:");
            Console.WriteLine($"  NAME: '{createNewFoodItemResult.GetName()}'");
            Console.WriteLine($"  CALS: '{createNewFoodItemResult.GetCalories()}'");
            Console.WriteLine($"  DATE: {createNewFoodItemResult.GetCreationDate().Day} {createNewFoodItemResult.GetCreationDate().Month} {createNewFoodItemResult.GetCreationDate().Year} ({createNewFoodItemResult.GetCreationDate().Hour}:{createNewFoodItemResult.GetCreationDate().Minute}:{createNewFoodItemResult.GetCreationDate().Second})");
        }
    }
}
