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
