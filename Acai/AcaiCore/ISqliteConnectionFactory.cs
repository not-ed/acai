using Microsoft.Data.Sqlite;

namespace AcaiCore
{
    public interface ISqliteConnectionFactory
    {
        public void SetDataSourceLocation(string dataSourceLocation);
        public SqliteConnection CreateOpenConnection();
    }
}
