using Microsoft.Data.Sqlite;

namespace AcaiCore
{
    public interface ISqliteConnectionFactory
    {
        public SqliteConnection CreateOpenConnection();
    }
}
