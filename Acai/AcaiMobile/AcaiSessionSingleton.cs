using AcaiCore;

namespace AcaiMobile
{
    public static class AcaiSessionSingleton
    {
        private static AcaiSession _session;

        private const string AcaiJournalFileName = "AcaiJournal.sql";
        
        public static async Task<AcaiSession> Get()
        {
            if (_session == null)
            {
                await InitializeAcaiSession();
            }
            
            return _session;
        }

        private static async Task InitializeAcaiSession()
        {
            var journalFilePath = FileSystem.AppDataDirectory + "/" + AcaiJournalFileName;
            var sessionInitializationFacade = new SessionInitializationFacade(
                JournalTableSchemas.All,
                new SqliteConnectionFactory(""));
            
            var existingJournalInitializationIsSuccessful = sessionInitializationFacade.InitializeSessionFromExistingJournalFileAtPath(journalFilePath);
            if (!existingJournalInitializationIsSuccessful)
            {
                var initializationFailureReason = sessionInitializationFacade.GetInitializationFailureReason();
                if (initializationFailureReason == SessionInitializationFailureReason.JOURNAL_FILE_DOES_NOT_EXIST)
                {
                    sessionInitializationFacade.InitializeSessionFromNewJournalFileAtPath(journalFilePath);
                    _session = sessionInitializationFacade.GetSession();
                    return;
                }

                if (initializationFailureReason == SessionInitializationFailureReason.JOURNAL_FILE_HAS_BAD_TABLES)
                {
                    // TODO: What should happen in the event of journal file corruption?
                    Application.Current.Quit();
                }
            }

            _session = sessionInitializationFacade.GetSession();
        }
    }
}
