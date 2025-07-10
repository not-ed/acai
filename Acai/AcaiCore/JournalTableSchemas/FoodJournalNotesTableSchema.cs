using Microsoft.Data.Sqlite;

namespace AcaiCore
{
    public class FoodJournalNotesTableSchema : IJournalTableSchema
    {
        private readonly string creationQuery = "CREATE TABLE \"food_journal_notes\" (\"id\" INTEGER NOT NULL UNIQUE,\"date\" TEXT NOT NULL UNIQUE,\"content\" TEXT NOT NULL, PRIMARY KEY(\"id\" AUTOINCREMENT))";
        public string GetSQLTableCreationQuery()
        {
            return creationQuery;
        }

        public bool PresentInConnection(SqliteConnection sqliteConnection)
        {
            var idColumnIsPresent = false;
            var dateColumnIsPresent = false;
            var contentColumnIsPresent = false;
            
            using (var selectTableCommand = sqliteConnection.CreateCommand())
            {
                selectTableCommand.CommandText = "PRAGMA TABLE_INFO(food_journal_notes);";
                using (var reader = selectTableCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!idColumnIsPresent && IDColumnIsPresentInSchema(reader))
                        {
                            idColumnIsPresent = true;
                            continue;
                        }
                        
                        if (!dateColumnIsPresent && DateColumnIsPresentInSchema(reader))
                        {
                            dateColumnIsPresent = true;
                            continue;
                        }
                        
                        if (!contentColumnIsPresent && ContentColumnIsPresentInSchema(reader))
                        {
                            contentColumnIsPresent = true;
                        }
                    }
                }
            }

            return idColumnIsPresent && dateColumnIsPresent && contentColumnIsPresent;
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
        
        private bool DateColumnIsPresentInSchema(SqliteDataReader reader)
        {
            if (reader.GetInt64(0) != 1) return false; // cid
            if (reader.GetString(1) != "date") return false; // name
            if (reader.GetString(2) != "TEXT") return false; // type
            if (reader.GetInt64(3) != 1) return false; // notnull
            if (reader.GetInt64(5) != 0) return false; // pk

            return true;
        }
        
        private bool ContentColumnIsPresentInSchema(SqliteDataReader reader)
        {
            if (reader.GetInt64(0) != 2) return false; // cid
            if (reader.GetString(1) != "content") return false; // name
            if (reader.GetString(2) != "TEXT") return false; // type
            if (reader.GetInt64(3) != 1) return false; // notnull
            if (reader.GetInt64(5) != 0) return false; // pk

            return true;
        }
    }
}