namespace AcaiCore
{
    public class SessionInitializationFacade : ISessionInitializationFacade
    {
        private SessionInitializationFailureReason _initializationFailureReason;
        private AcaiSession _session;
        private readonly List<IJournalTableSchema> _journalTableSchemas;
        private readonly ISqliteConnectionFactory _sqliteConnectionFactory;

        public SessionInitializationFacade(List<IJournalTableSchema> journalTableSchemas, ISqliteConnectionFactory sqliteConnectionFactory)
        {
            _journalTableSchemas = journalTableSchemas;
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        //TODO: "myjournal.sqlite" is hard-coded in SqliteConnectionFactory. this needs to be parameterized to parameterization here to truly work.
        public bool InitializeSessionFromNewJournalFileAtPath(string journalFilePath)
        {
            if (File.Exists(journalFilePath))
            {
                _initializationFailureReason = SessionInitializationFailureReason.JOURNAL_FILE_ALREADY_EXISTS;
                return false;
            }

            using (var newJournalFile = File.Create(journalFilePath))
            {
                using (var connection = _sqliteConnectionFactory.CreateOpenConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        foreach (var table in _journalTableSchemas)
                        {
                            command.CommandText = table.GetSQLTableCreationQuery();
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }

            return true;
        }

        public bool InitializeSessionFromExistingJournalFileAtPath(string journalFilePath)
        {
            throw new NotImplementedException();
        }

        public SessionInitializationFailureReason GetInitializationFailureReason()
        {
            return _initializationFailureReason;
        }

        public AcaiSession GetSession()
        {
            return _session;
        }
    }
}
