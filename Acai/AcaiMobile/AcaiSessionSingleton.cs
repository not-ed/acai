using AcaiCore;

namespace AcaiMobile
{
    public static class AcaiSessionSingleton
    {
        private static AcaiSession _session;

        public static async Task<AcaiSession> Get(Page requestingPage)
        {
            if (_session == null)
            {
                await InitializeAcaiSession(requestingPage);
            }

            return _session;
        }

        //https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-system-helpers?view=net-maui-8.0&tabs=android
        private static async Task InitializeAcaiSession(Page requestingPage)
        {
            var journalPath = FileSystem.Current.AppDataDirectory + "acaijournal.sql";
            var sessionInitializationFacade = new SessionInitializationFacade(
                new List<IJournalTableSchema>() { new FoodItemTableSchema() }, new SqliteConnectionFactory(journalPath));

            var failedToInitializeSession = !sessionInitializationFacade.InitializeSessionFromExistingJournalFileAtPath(journalPath);
            if (failedToInitializeSession)
            {
                switch (sessionInitializationFacade.GetInitializationFailureReason())
                {
                    case SessionInitializationFailureReason.JOURNAL_FILE_DOES_NOT_EXIST:
                        sessionInitializationFacade.InitializeSessionFromNewJournalFileAtPath(journalPath);
                        break;
                    case SessionInitializationFailureReason.JOURNAL_FILE_IS_MISSING_TABLES:
                        // TODO: Repair Journal File tables.
                        break;
                    case SessionInitializationFailureReason.NONE:
                        // TODO: How should this case be handled?
                        break;
                }
            }

            _session = sessionInitializationFacade.GetSession();
        }
    }
}
