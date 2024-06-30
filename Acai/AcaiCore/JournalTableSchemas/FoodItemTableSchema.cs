using Microsoft.Data.Sqlite;

namespace AcaiCore
{
    public class FoodItemTableSchema : IJournalTableSchema
    {
        private readonly string creationQuery = "CREATE TABLE \"food_items\" (\"id\" INTEGER NOT NULL UNIQUE,\"name\" TEXT NOT NULL,\"calories\" REAL NOT NULL,\"created_at\" TEXT NOT NULL, PRIMARY KEY(\"id\" AUTOINCREMENT))";
        public string GetSQLTableCreationQuery()
        {
            return creationQuery;
        }

        public bool PresentInConnection(SqliteConnection sqliteConnection)
        {
            bool schemaIsPresentInConnection = false;
            using (var selectTableCommand = sqliteConnection.CreateCommand())
            {
                selectTableCommand.CommandText = $"SELECT * FROM sqlite_master where name = 'food_items' AND sql = '{creationQuery}' LIMIT 1;";
                using (var reader = selectTableCommand.ExecuteReader())
                {
                    schemaIsPresentInConnection = reader.Read();
                }
            }
            return schemaIsPresentInConnection;
        }
    }
}
