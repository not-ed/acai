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
        JOURNAL_FILE_ALREADY_EXISTS,
        JOURNAL_FILE_DOES_NOT_EXIST,
        JOURNAL_FILE_IS_MISSING_TABLES,
    }
}
