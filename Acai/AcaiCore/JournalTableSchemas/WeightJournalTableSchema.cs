using Microsoft.Data.Sqlite;

namespace AcaiCore
{
    public class WeightJournalTableSchema : IJournalTableSchema
    {
        private readonly string creationQuery = "CREATE TABLE \"weigh_in_entries\" (\"id\" INTEGER NOT NULL UNIQUE,\"date\" TEXT NOT NULL,\"canonical_lbs\" REAL NOT NULL,\"body_fat_percentage\" REAL,\"note\" TEXT, PRIMARY KEY(\"id\" AUTOINCREMENT))";
        public string GetSQLTableCreationQuery()
        {
            return creationQuery;
        }

        public bool PresentInConnection(SqliteConnection sqliteConnection)
        {
            var idColumnIsPresent = false;
            var dateColumnIsPresent = false;
            var canonicalLbsColumnIsPresent = false;
            var bodyFatPercentageColumnIsPresent = false;
            var noteColumnIsPresent = false;

            using (var selectTableCommand = sqliteConnection.CreateCommand())
            {
                selectTableCommand.CommandText = "PRAGMA TABLE_INFO(weigh_in_entries);";
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

                        if (!canonicalLbsColumnIsPresent && CanonicalLbsColumnIsPresentInSchema(reader))
                        {
                            canonicalLbsColumnIsPresent = true;
                            continue;
                        }

                        if (!bodyFatPercentageColumnIsPresent && BodyFatPercentageColumnIsPresentInSchema(reader))
                        {
                            bodyFatPercentageColumnIsPresent = true;
                            continue;
                        }

                        if (!noteColumnIsPresent && NoteColumnIsPresentInSchema(reader))
                        {
                            noteColumnIsPresent = true;
                        }
                    }
                }
            }

            return idColumnIsPresent && dateColumnIsPresent && canonicalLbsColumnIsPresent && bodyFatPercentageColumnIsPresent && noteColumnIsPresent;
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

        private bool CanonicalLbsColumnIsPresentInSchema(SqliteDataReader reader)
        {
            if (reader.GetInt64(0) != 2) return false; // cid
            if (reader.GetString(1) != "canonical_lbs") return false; // name
            if (reader.GetString(2) != "REAL") return false; // type
            if (reader.GetInt64(3) != 1) return false; // notnull
            if (reader.GetInt64(5) != 0) return false; // pk

            return true;
        }

        private bool BodyFatPercentageColumnIsPresentInSchema(SqliteDataReader reader)
        {
            if (reader.GetInt64(0) != 3) return false; // cid
            if (reader.GetString(1) != "body_fat_percentage") return false; // name
            if (reader.GetString(2) != "REAL") return false; // type
            if (reader.GetInt64(3) != 0) return false; // notnull (optional)
            if (reader.GetInt64(5) != 0) return false; // pk

            return true;
        }

        private bool NoteColumnIsPresentInSchema(SqliteDataReader reader)
        {
            if (reader.GetInt64(0) != 4) return false; // cid
            if (reader.GetString(1) != "note") return false; // name
            if (reader.GetString(2) != "TEXT") return false; // type
            if (reader.GetInt64(3) != 0) return false; // notnull (optional)
            if (reader.GetInt64(5) != 0) return false; // pk

            return true;
        }
    }
}
