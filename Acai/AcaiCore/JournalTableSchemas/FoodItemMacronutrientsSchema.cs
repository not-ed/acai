using Microsoft.Data.Sqlite;

namespace AcaiCore
{
    public class FoodItemMacronutrientsSchema : IJournalTableSchema
    {
        private readonly string creationQuery = "ALTER TABLE food_items ADD COLUMN protein REAL;" + 
                                                "ALTER TABLE food_items ADD COLUMN carbohydrates REAL;" +
                                                "ALTER TABLE food_items ADD COLUMN fat REAL;" +
                                                "ALTER TABLE food_items ADD COLUMN fibre REAL;" +
                                                "ALTER TABLE food_items ADD COLUMN water REAL;";
        public string GetSQLTableCreationQuery()
        {
            return creationQuery;
        }

        public bool PresentInConnection(SqliteConnection sqliteConnection)
        {
            var proteinColumnIsPresent = false;
            var carbohydratesColumnIsPresent = false;
            var fatColumnIsPresent = false;
            var fibreColumnIsPresent = false;
            var waterColumnIsPresent = false;
            
            using (var selectTableCommand = sqliteConnection.CreateCommand())
            {
                selectTableCommand.CommandText = "PRAGMA TABLE_INFO(food_items);";
                using (var reader = selectTableCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!proteinColumnIsPresent && ProteinColumnIsPresentInSchema(reader))
                        {
                            proteinColumnIsPresent = true;
                            continue;
                        }
                        
                        if (!carbohydratesColumnIsPresent && CarbohydratesColumnIsPresentInSchema(reader))
                        {
                            carbohydratesColumnIsPresent = true;
                            continue;
                        }
                        
                        if (!fatColumnIsPresent && FatColumnIsPresentInSchema(reader))
                        {
                            fatColumnIsPresent = true;
                            continue;
                        }
                        
                        if (!fibreColumnIsPresent && FibreColumnIsPresentInSchema(reader))
                        {
                            fibreColumnIsPresent = true;
                            continue;
                        }
                        
                        if (!waterColumnIsPresent && WaterColumnIsPresentInSchema(reader))
                        {
                            waterColumnIsPresent = true;
                        }
                    }
                }
            }

            return proteinColumnIsPresent && carbohydratesColumnIsPresent && fatColumnIsPresent && fibreColumnIsPresent && waterColumnIsPresent;
        }

        private bool ProteinColumnIsPresentInSchema(SqliteDataReader reader)
        {
            if (reader.GetInt64(0) != 4) return false; // cid
            if (reader.GetString(1) != "protein") return false; // name
            if (reader.GetString(2) != "REAL") return false; // type
            if (reader.GetInt64(3) != 0) return false; // notnull
            if (reader.GetInt64(5) != 0) return false; // pk

            return true;
        }
        
        private bool CarbohydratesColumnIsPresentInSchema(SqliteDataReader reader)
        {
            if (reader.GetInt64(0) != 5) return false; // cid
            if (reader.GetString(1) != "carbohydrates") return false; // name
            if (reader.GetString(2) != "REAL") return false; // type
            if (reader.GetInt64(3) != 0) return false; // notnull
            if (reader.GetInt64(5) != 0) return false; // pk
            
            return true;
        }
        
        private bool FatColumnIsPresentInSchema(SqliteDataReader reader)
        {
            if (reader.GetInt64(0) != 6) return false; // cid
            if (reader.GetString(1) != "fat") return false; // name
            if (reader.GetString(2) != "REAL") return false; // type
            if (reader.GetInt64(3) != 0) return false; // notnull
            if (reader.GetInt64(5) != 0) return false; // pk
            
            return true;
        }
        
        private bool FibreColumnIsPresentInSchema(SqliteDataReader reader)
        {
            if (reader.GetInt64(0) != 7) return false; // cid
            if (reader.GetString(1) != "fibre") return false; // name
            if (reader.GetString(2) != "REAL") return false; // type
            if (reader.GetInt64(3) != 0) return false; // notnull
            if (reader.GetInt64(5) != 0) return false; // pk
            
            return true;
        }
        
        private bool WaterColumnIsPresentInSchema(SqliteDataReader reader)
        {
            if (reader.GetInt64(0) != 8) return false; // cid
            if (reader.GetString(1) != "water") return false; // name
            if (reader.GetString(2) != "REAL") return false; // type
            if (reader.GetInt64(3) != 0) return false; // notnull
            if (reader.GetInt64(5) != 0) return false; // pk
            
            return true;
        }
    }
}
