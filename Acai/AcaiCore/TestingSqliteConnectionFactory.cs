using Microsoft.Data.Sqlite;
using System.Data;

namespace AcaiCore
{
    public class TestingSqliteConnectionFactory : ISqliteConnectionFactory
    {
        private SqliteConnection _connection;
        private string _connectionString = "";

        public TestingSqliteConnectionFactory()
        {
            _connectionString = $"Data Source={Guid.NewGuid()};Mode=Memory;Cache=Shared";
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            var createFoodItemsTableCommand = _connection.CreateCommand();
            createFoodItemsTableCommand.CommandText = "CREATE TABLE IF NOT EXISTS \"food_items\" (\r\n\t\"id\"\tINTEGER NOT NULL UNIQUE,\r\n\t\"name\"\tTEXT NOT NULL,\r\n\t\"calories\"\tREAL NOT NULL,\r\n\t\"created_at\"\tTEXT NOT NULL,\r\n\tPRIMARY KEY(\"id\" AUTOINCREMENT)\r\n);";
            createFoodItemsTableCommand.ExecuteNonQuery();
            createFoodItemsTableCommand.Dispose();
        }

        ~TestingSqliteConnectionFactory()
        {
            if ( _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        public SqliteConnection CreateOpenConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
