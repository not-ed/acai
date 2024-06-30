using Microsoft.Data.Sqlite;

namespace AcaiCore
{
    public interface IJournalTableSchema
    {
        public string GetSQLTableCreationQuery();
        public bool PresentInConnection(SqliteConnection sqliteConnection);
    }
}
