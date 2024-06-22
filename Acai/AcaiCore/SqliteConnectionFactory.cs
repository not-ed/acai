using Microsoft.Data.Sqlite;

namespace AcaiCore
{
    public class SqliteConnectionFactory : ISqliteConnectionFactory
    {
        public SqliteConnection CreateOpenConnection()
        {
            var connection = new SqliteConnection("Data Source=myjournal.sqlite");
            connection.Open();

            return connection;
        }
    }
}
