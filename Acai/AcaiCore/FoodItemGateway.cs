using Microsoft.Data.Sqlite;

namespace AcaiCore
{
    public class FoodItemGateway : IFoodItemGateway
    {
        private ISqliteConnectionFactory sqliteConnectionFactory;

        public FoodItemGateway(ISqliteConnectionFactory connectionFactory)
        {
            this.sqliteConnectionFactory = connectionFactory;
        }

        public FoodItemDTO CreateNewFoodItem(string name, float calories, DateTime creationDate, float? protein, float? carbohydrates, float? fat, float? fibre, float? water)
        {
            long createdItemId = -1;
            string createdItemName = "";
            float createdItemCalories = 0;
            DateTime createdItemCreationDate = DateTime.Now;
            float? createdItemProtein = null;
            float? createdItemCarbohydrates = null;
            float? createdItemFat = null;
            float? createdItemFibre = null;
            float? createdItemWater = null;

            using (var connection = sqliteConnectionFactory.CreateOpenConnection())
            {
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = "INSERT INTO food_items (name, calories, created_at, protein, carbohydrates, fat, fibre, water) VALUES " +
                        "(@itemName, @itemCalories, @itemCreationDate, @itemProtein, @itemCarbohydrates, @itemFat, @itemFibre, @itemWater) " +
                        "RETURNING id;";
                    
                    var itemNameParameter = new SqliteParameter("@itemName",SqliteType.Text);
                    itemNameParameter.Value = name;
                    
                    var itemCaloriesParameter = new SqliteParameter("@itemCalories",SqliteType.Real);
                    itemCaloriesParameter.Value = calories;
                    
                    var itemCreationDateParameter = new SqliteParameter("@itemCreationDate",SqliteType.Text);
                    itemCreationDateParameter.Value = creationDate.ToString("yyyy-MM-dd HH:mm:ss");
                    
                    var itemProteinParameter = new SqliteParameter("@itemProtein", SqliteType.Real);
                    itemProteinParameter.Value = protein != null ? protein : DBNull.Value;
                    
                    var itemCarbohydratesParameter = new SqliteParameter("@itemCarbohydrates", SqliteType.Real);
                    itemCarbohydratesParameter.Value = carbohydrates != null ? carbohydrates : DBNull.Value;
                    
                    var itemFatParameter = new SqliteParameter("@itemFat", SqliteType.Real);
                    itemFatParameter.Value = fat != null ? fat : DBNull.Value;
                    
                    var itemFibreParameter = new SqliteParameter("@itemFibre", SqliteType.Real);
                    itemFibreParameter.Value = fibre != null ? fibre : DBNull.Value;
                    
                    var itemWaterParameter = new SqliteParameter("@itemWater", SqliteType.Real);
                    itemWaterParameter.Value = water != null ? water : DBNull.Value;
                    
                    insertCommand.Parameters.AddRange(new List<SqliteParameter>()
                    {
                        itemNameParameter,
                        itemCaloriesParameter,
                        itemCreationDateParameter,
                        itemProteinParameter,
                        itemCarbohydratesParameter,
                        itemFatParameter,
                        itemFibreParameter,
                        itemWaterParameter,
                    });
                    insertCommand.Prepare();

                    createdItemId = (long)insertCommand.ExecuteScalar();

                    using (var selectNewItemCommand = connection.CreateCommand())
                    {
                        selectNewItemCommand.CommandText = "SELECT name, calories, created_at, protein, carbohydrates, fat, fibre, water " +
                            "FROM food_items " +
                            $"WHERE id = {createdItemId} " +
                            "LIMIT 1;";

                        using (var reader = selectNewItemCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                createdItemName = reader.GetString(0);
                                createdItemCalories = reader.GetFloat(1);
                                createdItemCreationDate = reader.GetDateTime(2);
                                createdItemProtein = reader.IsDBNull(3) ? null : reader.GetFloat(3);
                                createdItemCarbohydrates = reader.IsDBNull(4) ? null : reader.GetFloat(4);
                                createdItemFat = reader.IsDBNull(5) ? null : reader.GetFloat(5);
                                createdItemFibre = reader.IsDBNull(6) ? null : reader.GetFloat(6);
                                createdItemWater = reader.IsDBNull(7) ? null : reader.GetFloat(7);
                            }
                        }
                    }
                }
            }

            return new FoodItemDTO(
                createdItemId,
                createdItemName,
                createdItemCalories,
                createdItemCreationDate,
                createdItemProtein,
                createdItemCarbohydrates,
                createdItemFat,
                createdItemFibre,
                createdItemWater);
        }

        public void DeleteFoodItem(long itemId)
        {
            using (var connection = sqliteConnectionFactory.CreateOpenConnection())
            {
                using (var deleteCommand = connection.CreateCommand())
                {
                    deleteCommand.CommandText = "DELETE FROM food_items WHERE id = @deletedItemID;";

                    var itemIdParameter = new SqliteParameter("@deletedItemID", SqliteType.Real);
                    itemIdParameter.Value = itemId;

                    deleteCommand.Parameters.Add(itemIdParameter);
                    deleteCommand.Prepare();
                    deleteCommand.ExecuteNonQuery();
                }   
            }
        }

        public FoodItemDTO UpdateExistingFoodItem(long itemId, string name, float calories, DateTime creationDate, float? protein, float? carbohydrates, float? fat, float? fibre, float? water)
        {
            string updatedItemName = "";
            float updatedItemCalories = 0;
            DateTime updatedItemCreationDate = DateTime.Now;
            float? updatedItemProtein = null;
            float? updatedItemCarbohydrates = null;
            float? updatedItemFat = null;
            float? updatedItemFibre = null;
            float? updatedItemWater = null;

            using (var connection = sqliteConnectionFactory.CreateOpenConnection())
            {
                using (var updateCommand = connection.CreateCommand())
                {
                    updateCommand.CommandText = "UPDATE food_items SET (name, calories, created_at, protein, carbohydrates, fat, fibre, water) = " +
                        "(@itemName, @itemCalories, @itemCreationDate, @itemProtein, @itemCarbohydrates, @itemFat, @itemFibre, @itemWater) " +
                        "WHERE id = @itemID;";

                    var itemIdParameter = new SqliteParameter("@itemID", SqliteType.Text);
                    itemIdParameter.Value = itemId;

                    var itemNameParameter = new SqliteParameter("@itemName", SqliteType.Text);
                    itemNameParameter.Value = name;

                    var itemCaloriesParameter = new SqliteParameter("@itemCalories", SqliteType.Real);
                    itemCaloriesParameter.Value = calories;

                    var itemCreationDateParameter = new SqliteParameter("@itemCreationDate", SqliteType.Text);
                    itemCreationDateParameter.Value = creationDate.ToString("yyyy-MM-dd HH:mm:ss");
                    
                    var itemProteinParameter = new SqliteParameter("@itemProtein", SqliteType.Real);
                    itemProteinParameter.Value = protein != null ? protein : DBNull.Value;
                    
                    var itemCarbohydratesParameter = new SqliteParameter("@itemCarbohydrates", SqliteType.Real);
                    itemCarbohydratesParameter.Value = carbohydrates != null ? carbohydrates : DBNull.Value;
                    
                    var itemFatParameter = new SqliteParameter("@itemFat", SqliteType.Real);
                    itemFatParameter.Value = fat != null ? fat : DBNull.Value;
                    
                    var itemFibreParameter = new SqliteParameter("@itemFibre", SqliteType.Real);
                    itemFibreParameter.Value = fibre != null ? fibre : DBNull.Value;
                    
                    var itemWaterParameter = new SqliteParameter("@itemWater", SqliteType.Real);
                    itemWaterParameter.Value = water != null ? water : DBNull.Value;

                    updateCommand.Parameters.AddRange(new List<SqliteParameter>()
                    {
                        itemIdParameter,
                        itemNameParameter,
                        itemCaloriesParameter,
                        itemCreationDateParameter,
                        itemProteinParameter,
                        itemCarbohydratesParameter,
                        itemFatParameter,
                        itemFibreParameter,
                        itemWaterParameter
                    });
                    updateCommand.Prepare();
                    updateCommand.ExecuteNonQuery();

                    using (var selectUpdatedItemCommand = connection.CreateCommand())
                    {
                        selectUpdatedItemCommand.CommandText = $"SELECT name, calories, created_at, protein, carbohydrates, fat, fibre, water FROM food_items WHERE id = {itemId} LIMIT 1;";

                        using (var reader = selectUpdatedItemCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                updatedItemName = reader.GetString(0);
                                updatedItemCalories = reader.GetFloat(1);
                                updatedItemCreationDate = reader.GetDateTime(2);
                                updatedItemProtein = reader.IsDBNull(3) ? null : reader.GetFloat(3);
                                updatedItemCarbohydrates = reader.IsDBNull(4) ? null : reader.GetFloat(4);
                                updatedItemFat = reader.IsDBNull(5) ? null : reader.GetFloat(5);
                                updatedItemFibre = reader.IsDBNull(6) ? null : reader.GetFloat(6);
                                updatedItemWater = reader.IsDBNull(7) ? null : reader.GetFloat(7);
                            }
                        }
                    }
                }
            }

            return new FoodItemDTO(
                itemId,
                updatedItemName,
                updatedItemCalories,
                updatedItemCreationDate,
                updatedItemProtein,
                updatedItemCarbohydrates,
                updatedItemFat,
                updatedItemFibre,
                updatedItemWater);
        }

        public List<FoodItemDTO> GetFoodItemsForDate(DateTime date)
        {
            var items = new List<FoodItemDTO>();

            using (var connection = sqliteConnectionFactory.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT id, name, calories, created_at, protein, carbohydrates, fat, fibre, water FROM food_items WHERE date(created_at) = date('{date.ToString("yyyy-MM-dd")}');";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = reader.GetInt64(0);
                            string name = reader.GetString(1);
                            float calories = reader.GetFloat(2);
                            DateTime creationDate = reader.GetDateTime(3);
                            float? protein = reader.IsDBNull(4) ? null : reader.GetFloat(4);
                            float? carbohydrates = reader.IsDBNull(5) ? null : reader.GetFloat(5);
                            float? fat = reader.IsDBNull(6) ? null : reader.GetFloat(6);
                            float? fibre = reader.IsDBNull(7) ? null : reader.GetFloat(7);
                            float? water = reader.IsDBNull(8) ? null : reader.GetFloat(8);
                            
                            items.Add(new FoodItemDTO(
                                id,
                                name,
                                calories,
                                creationDate,
                                protein,
                                carbohydrates,
                                fat,
                                fibre,
                                water));
                        }
                    }
                }
            }

            return items;
        }
    }
}
