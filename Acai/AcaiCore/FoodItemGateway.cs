namespace AcaiCore
{
    public class FoodItemGateway : IFoodItemGateway
    {
        private ISqliteConnectionFactory sqliteConnectionFactory;

        public FoodItemGateway(ISqliteConnectionFactory connectionFactory)
        {
            this.sqliteConnectionFactory = connectionFactory;
        }

        public FoodItemDTO CreateNewFoodItem(string name, float calories, DateTime creationDate)
        {
            string createdItemName = "";
            float createdItemCalories = 0;
            DateTime createdItemCreationDate = DateTime.Now;

            using (var connection = sqliteConnectionFactory.CreateOpenConnection())
            {
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = "INSERT INTO food_items (name, calories, created_at) VALUES (" +
                    $"'{name}'," +
                    $"{calories}," +
                    $"'{creationDate.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                    ") RETURNING id;";
                var createdItemId = insertCommand.ExecuteScalar();
                insertCommand.Dispose();

                var selectNewItemCommand = connection.CreateCommand();
                selectNewItemCommand.CommandText = $"SELECT name, calories, created_at FROM food_items WHERE id = {createdItemId} LIMIT 1;";
                selectNewItemCommand.Dispose();

                using (var reader = selectNewItemCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        createdItemName = reader.GetString(0);
                        createdItemCalories = reader.GetFloat(1);
                        createdItemCreationDate = reader.GetDateTime(2);
                    }
                }
            }

            return new FoodItemDTO(createdItemName, createdItemCalories, createdItemCreationDate);
        }

        public List<FoodItemDTO> GetFoodItemsForDate(DateTime date)
        {
            var items = new List<FoodItemDTO>();

            using (var connection = sqliteConnectionFactory.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT name, calories, created_at FROM food_items WHERE date(created_at) = date('{date.ToString("yyyy-MM-dd")}');";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new FoodItemDTO(reader.GetString(0),reader.GetFloat(1),reader.GetDateTime(2)));
                        }
                    }
                }
            }

            return items;
        }
    }
}
