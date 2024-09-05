﻿using Microsoft.Data.Sqlite;

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
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = "INSERT INTO food_items (name, calories, created_at) VALUES (@itemName, @itemCalories, @itemCreationDate) RETURNING id;";
                    
                    var itemNameParameter = new SqliteParameter("@itemName",SqliteType.Text);
                    itemNameParameter.Value = name;
                    
                    var itemCaloriesParameter = new SqliteParameter("@itemCalories",SqliteType.Real);
                    itemCaloriesParameter.Value = calories;
                    
                    var itemCreationDateParameter = new SqliteParameter("@itemCreationDate",SqliteType.Text);
                    itemCreationDateParameter.Value = creationDate.ToString("yyyy-MM-dd HH:mm:ss");
                    
                    insertCommand.Parameters.AddRange(new List<SqliteParameter>()
                    {
                        itemNameParameter,
                        itemCaloriesParameter,
                        itemCreationDateParameter
                    });
                    insertCommand.Prepare();

                    var createdItemId = insertCommand.ExecuteScalar();

                    using (var selectNewItemCommand = connection.CreateCommand())
                    {
                        selectNewItemCommand.CommandText = $"SELECT name, calories, created_at FROM food_items WHERE id = {createdItemId} LIMIT 1;";

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
