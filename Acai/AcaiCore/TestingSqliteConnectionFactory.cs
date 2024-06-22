using Microsoft.Data.Sqlite;

namespace AcaiCore
{
    public class TestingSqliteConnectionFactory : ISqliteConnectionFactory
    {
        public SqliteConnection CreateOpenConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var createFoodItemsTableCommand = connection.CreateCommand();
            createFoodItemsTableCommand.CommandText = "CREATE TABLE IF NOT EXISTS \"food_items\" (\r\n\t\"id\"\tINTEGER NOT NULL UNIQUE,\r\n\t\"name\"\tTEXT NOT NULL,\r\n\t\"calories\"\tREAL NOT NULL,\r\n\t\"created_at\"\tTEXT NOT NULL,\r\n\tPRIMARY KEY(\"id\" AUTOINCREMENT)\r\n);";
            createFoodItemsTableCommand.ExecuteNonQuery();
            createFoodItemsTableCommand.Dispose();

            return connection;
        }
    }
}
