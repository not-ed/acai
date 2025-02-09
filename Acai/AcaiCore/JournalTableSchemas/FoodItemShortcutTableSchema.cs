using Microsoft.Data.Sqlite;

namespace AcaiCore
{
    public class FoodItemShortcutTableSchema : IJournalTableSchema
    {
        private readonly string creationQuery = "CREATE TABLE \"food_item_shortcuts\" (\"id\" INTEGER NOT NULL UNIQUE, \"name\" TEXT NOT NULL, \"calories\" REAL NOT NULL, PRIMARY KEY (\"id\" AUTOINCREMENT))";
        public string GetSQLTableCreationQuery()
        {
            return creationQuery;
        }

        public bool PresentInConnection(SqliteConnection sqliteConnection)
        {
            var idColumnIsPresent = false;
            var nameColumnIsPresent = false;
            var caloriesColumnIsPresent = false;
            
            using (var selectTableCommand = sqliteConnection.CreateCommand())
            {
                selectTableCommand.CommandText = "PRAGMA TABLE_INFO(food_item_shortcuts);";
                using (var reader = selectTableCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!idColumnIsPresent && IDColumnIsPresentInSchema(reader))
                        {
                            idColumnIsPresent = true;
                            continue;
                        }
                        
                        if (!nameColumnIsPresent && NameColumnIsPresentInSchema(reader))
                        {
                            nameColumnIsPresent = true;
                            continue;
                        }
                        
                        if (!caloriesColumnIsPresent && CaloriesColumnIsPresentInSchema(reader))
                        {
                            caloriesColumnIsPresent = true;
                        }
                    }
                }
            }

            return idColumnIsPresent && nameColumnIsPresent && caloriesColumnIsPresent;
        }
        
        private bool IDColumnIsPresentInSchema(SqliteDataReader reader)
        {
            if (reader.GetInt64(0) != 0) return false; // cid
            if (reader.GetString(1) != "id") return false; // name
            if (reader.GetString(2) != "INTEGER") return false; // type
            if (reader.GetInt64(3) != 1) return false; // notnull
            if (reader.GetInt64(5) != 1) return false; // pk

            return true;
        }
        
        private bool NameColumnIsPresentInSchema(SqliteDataReader reader)
        {
            if (reader.GetInt64(0) != 1) return false; // cid
            if (reader.GetString(1) != "name") return false; // name
            if (reader.GetString(2) != "TEXT") return false; // type
            if (reader.GetInt64(3) != 1) return false; // notnull
            if (reader.GetInt64(5) != 0) return false; // pk

            return true;
        }
        
        private bool CaloriesColumnIsPresentInSchema(SqliteDataReader reader)
        {
            if (reader.GetInt64(0) != 2) return false; // cid
            if (reader.GetString(1) != "calories") return false; // name
            if (reader.GetString(2) != "REAL") return false; // type
            if (reader.GetInt64(3) != 1) return false; // notnull
            if (reader.GetInt64(5) != 0) return false; // pk

            return true;
        }
    }
}