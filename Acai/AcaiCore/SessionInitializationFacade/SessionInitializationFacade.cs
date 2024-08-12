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

        public bool InitializeSessionFromNewJournalFileAtPath(string journalFilePath)
        {
            if (File.Exists(journalFilePath))
            {
                _initializationFailureReason = SessionInitializationFailureReason.JOURNAL_FILE_ALREADY_EXISTS;
                return false;
            }

            File.Create(journalFilePath).Close();
             
            _sqliteConnectionFactory.SetDataSourceLocation(journalFilePath);
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

            _session = new AcaiSession(new FoodItemGateway(_sqliteConnectionFactory));

            return true;
        }

        public bool InitializeSessionFromExistingJournalFileAtPath(string journalFilePath)
        {
            if (!File.Exists(journalFilePath))
            {
                _initializationFailureReason = SessionInitializationFailureReason.JOURNAL_FILE_DOES_NOT_EXIST;
                return false;
            }

            _sqliteConnectionFactory.SetDataSourceLocation(journalFilePath);

            using (var connection = _sqliteConnectionFactory.CreateOpenConnection()){
                foreach (var schema in _journalTableSchemas)
                {
                    if (!schema.PresentInConnection(connection))
                    {
                        _initializationFailureReason = SessionInitializationFailureReason.JOURNAL_FILE_IS_MISSING_TABLES;
                        return false;
                    }
                }
            }

            _session = new AcaiSession(new FoodItemGateway(_sqliteConnectionFactory));

            return true;
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
