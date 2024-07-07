namespace AcaiCore
{
    public class SessionInitializationFacade : ISessionInitializationFacade
    {
        private AcaiSession _session;

        public bool InitializeSessionFromNewJournalFileAtPath(string journalFilePath)
        {
            throw new NotImplementedException();
        }

        public bool InitializeSessionFromExistingJournalFileAtPath(string journalFilePath)
        {
            throw new NotImplementedException();
        }

        public SessionInitializationFailureReason GetInitializationFailureReason()
        {
            throw new NotImplementedException();
        }

        public AcaiSession GetSession()
        {
            return _session;
        }
    }
}
