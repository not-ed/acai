using Microsoft.Data.Sqlite;

namespace AcaiCore
{
    public class SqliteConnectionFactory : ISqliteConnectionFactory
    {
        private string _dataSourceLocation;

        public SqliteConnectionFactory(string dataSourceLocation)
        {
            _dataSourceLocation = dataSourceLocation;
        }

        public void SetDataSourceLocation(string dataSourceLocation)
        {
            _dataSourceLocation = dataSourceLocation;
        }

        public SqliteConnection CreateOpenConnection()
        {
            var connection = new SqliteConnection($"Data Source={_dataSourceLocation}");
            connection.Open();

            return connection;
        }
    }
}
