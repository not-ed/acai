using Microsoft.Data.Sqlite;

namespace AcaiCore;

public class FoodItemShortcutGateway : IFoodItemShortcutGateway
{
    private ISqliteConnectionFactory sqliteConnectionFactory;

    public FoodItemShortcutGateway(ISqliteConnectionFactory connectionFactory)
    {
        this.sqliteConnectionFactory = connectionFactory;
    }

    public FoodItemShortcutDTO CreateNewFoodItemShortcut(string name, float calories)
    {
        long createdShortcutId = -1;
        string createdShortcutName = "";
        float createdShortcutCalories = 0;

        using (var connection = sqliteConnectionFactory.CreateOpenConnection())
        {
            using (var insertCommand = connection.CreateCommand())
            {
                insertCommand.CommandText = "INSERT INTO food_item_shortcuts (name, calories) VALUES (@shortcutName, @shortcutCalories) RETURNING id;";
                
                var shortcutNameParameter = new SqliteParameter("shortcutName", SqliteType.Text);
                shortcutNameParameter.Value = name;
                
                var shortcutCaloriesParameter = new SqliteParameter("shortcutCalories", SqliteType.Real);
                shortcutCaloriesParameter.Value = calories;
                
                insertCommand.Parameters.AddRange(new List<SqliteParameter>()
                {
                    shortcutNameParameter,
                    shortcutCaloriesParameter
                });
                insertCommand.Prepare();
                
                createdShortcutId = (long)insertCommand.ExecuteScalar();

                using (var selectNewShortcutId = connection.CreateCommand())
                {
                    selectNewShortcutId.CommandText = $"SELECT name, calories FROM food_item_shortcuts WHERE id = {createdShortcutId} LIMIT 1;";
                    using (var reader = selectNewShortcutId.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            createdShortcutName = reader.GetString(0);
                            createdShortcutCalories = reader.GetFloat(1);
                        }
                    }
                }
            }
        }
        
        return new FoodItemShortcutDTO(createdShortcutId, createdShortcutName, createdShortcutCalories, null, null, null, null, null);
    }

    public void DeleteFoodItemShortcut(long id)
    {
        using (var connection = sqliteConnectionFactory.CreateOpenConnection())
        {
            using (var deleteCommand = connection.CreateCommand())
            {
                deleteCommand.CommandText = "DELETE FROM food_item_shortcuts WHERE id = @deletedShortcutID;";

                var shortcutIdParameter = new SqliteParameter("@deletedShortcutID", SqliteType.Real);
                shortcutIdParameter.Value = id;

                deleteCommand.Parameters.Add(shortcutIdParameter);
                deleteCommand.Prepare();
                deleteCommand.ExecuteNonQuery();
            }
        }
    }

    public List<FoodItemShortcutDTO> GetAllFoodItemShortcuts()
    {
        List<FoodItemShortcutDTO> shortcuts = new List<FoodItemShortcutDTO>();

        using (var connection = sqliteConnectionFactory.CreateOpenConnection())
        {
            using (var selectCommand = connection.CreateCommand())
            {
                selectCommand.CommandText = "SELECT id, name, calories, protein, carbohydrates, fat, fibre, water FROM food_item_shortcuts;";

                using (var reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        shortcuts.Add(
                            new FoodItemShortcutDTO(reader.GetInt64(0), 
                                reader.GetString(1), 
                                reader.GetFloat(2),
                                reader.IsDBNull(3) ? null : reader.GetFloat(3),
                                reader.IsDBNull(4) ? null : reader.GetFloat(4),
                                reader.IsDBNull(5) ? null : reader.GetFloat(5),
                                reader.IsDBNull(6) ? null : reader.GetFloat(6),
                                reader.IsDBNull(7) ? null : reader.GetFloat(7)));
                    }
                }
            }
        }
        
        return shortcuts;
    }
}