namespace AcaiCore
{
    public class SessionInitializationFacade : ISessionInitializationFacade
    {
        private SessionInitializationFailureReason _initializationFailureReason;
        private AcaiSession _session;

        public bool InitializeSessionFromNewJournalFileAtPath(string journalFilePath)
        {
            if (File.Exists(journalFilePath))
            {
                _initializationFailureReason = SessionInitializationFailureReason.JOURNAL_FILE_ALREADY_EXISTS;
                return false;
            }

            using (var newJournalFile = File.Create(journalFilePath))
            {

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
