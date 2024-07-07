namespace AcaiCore
{
    public interface ISessionInitializationFacade
    {
        public bool InitializeSessionFromNewJournalFileAtPath(string journalFilePath);
        public bool InitializeSessionFromExistingJournalFileAtPath(string journalFilePath);
        public SessionInitializationFailureReason GetInitializationFailureReason();
        public AcaiSession GetSession();
    }

    public enum SessionInitializationFailureReason
    {
        NONE,
    }
}
