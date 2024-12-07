using AcaiCore;
using CommunityToolkit.Maui.Storage;

namespace AcaiMobile
{
    public static class AcaiSessionSingleton
    {
        private static AcaiSession _session;

        private const string JournalFilePathPreferencesKey = "AcaiJournalFilePath";

        public static async Task<AcaiSession> Get(Page requestingPage)
        {
            if (_session == null)
            {
                await InitializeAcaiSession();
            }

            return _session;
        }

        // TODO: This whole thing generally is not great. The challenges of fighting with External Storage APIs in Android isn't worth it just to allow a journal to be saved to arbitrary paths.
        // This should be re-written at some point to use a hard-coded path in the App Data Directory, this would create an opportunity to either use Backup APIs frameworks for the platform or add a one-time import/export utility later.
        // https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-system-helpers
        private static async Task InitializeAcaiSession()
        {
            var userHasPreviouslySpecifiedAJournalFilePath = Preferences.Default.ContainsKey(JournalFilePathPreferencesKey);
            if (!userHasPreviouslySpecifiedAJournalFilePath)
            {
                var continueWithSetup = await Shell.Current.DisplayAlert(
                    "First-time Setup",
                    "It looks like this is your first time using Acai on this device, would you like to set up a Journal file now?" +
                    "\n\n" +
                    "All entries you make into Acai will be saved to a Journal file which is kept locally on your device. If you have not created a Journal file yet, one will need to be created. " +
                    "Alternatively, if you already have a Journal file in a cloud drive or from a different device, you can point Acai to continue using that." +
                    "\n\n" +
                    "Would you like to do this now? Note that this app will close if you select 'No'.",
                    "Yes",
                    "No");
                
                if (!continueWithSetup)
                {
                    Application.Current.Quit();
                }

                var journalSetupApproach = await Shell.Current.DisplayActionSheet(
                    "First-time Setup",
                    "Cancel",
                    null,
                    "Create new Journal file",
                    "I already have a Journal file");
                
                if (journalSetupApproach == null || journalSetupApproach == "Cancel")
                {
                    Application.Current.Quit();
                }

                var useExistingJournalFile = journalSetupApproach == "I already have a Journal file";
                if (useExistingJournalFile)
                {
                    var destinationFile = await FilePicker.PickAsync(PickOptions.Default);
                    if (destinationFile == null)
                    {
                        Application.Current.Quit();
                    }

                    var sessionInitializationFacade = new SessionInitializationFacade(
                        new List<IJournalTableSchema>() { new FoodItemTableSchema() },
                        new SqliteConnectionFactory(""));

                    var sessionInitializationOutcome =
                        sessionInitializationFacade.InitializeSessionFromExistingJournalFileAtPath(destinationFile.FullPath);
                    if (sessionInitializationOutcome == false)
                    {
                        switch (sessionInitializationFacade.GetInitializationFailureReason())
                        {
                            case SessionInitializationFailureReason.JOURNAL_FILE_DOES_NOT_EXIST:
                                await Shell.Current.DisplayAlert("Error", "Journal File does not exist.", "Ok");
                                break;
                            case SessionInitializationFailureReason.JOURNAL_FILE_IS_MISSING_TABLES:
                                await Shell.Current.DisplayAlert("Error", "File is missing tables.", "Ok");
                                break;
                            case SessionInitializationFailureReason.JOURNAL_FILE_ALREADY_EXISTS:
                                await Shell.Current.DisplayAlert("Error", "There is already a Journal file with this name at the same destination.", "Ok");
                                break;
                            default:
                                await Shell.Current.DisplayAlert("Error", "Failed to create and initialize a Journal file at this destination (unknown reason).", "Ok");
                                break;
                        }
                        Application.Current.Quit();
                    }

                    await Shell.Current.DisplayAlert("Done!", "Journal File set Successfully!", "Ok");
                    Preferences.Default.Set(JournalFilePathPreferencesKey, destinationFile.FullPath);
                    _session = sessionInitializationFacade.GetSession();
                }
                else
                {
                    var destinationFolder = await FolderPicker.Default.PickAsync();
                    if (!destinationFolder.IsSuccessful)
                    {
                        Application.Current.Quit();
                    }
                    
                    var journalFileName = await Shell.Current.DisplayPromptAsync("Journal file name", "Please enter a name for your Journal file.", "Submit", "Cancel", null, -1, Keyboard.Plain, "my-acai-journal.sql");
                    if (journalFileName == null)
                    {
                        Application.Current.Quit();
                    }

                    var fullJournalFilePath = destinationFolder.Folder.Path + "/" + journalFileName;

                    var sessionInitializationFacade = new SessionInitializationFacade(
                        new List<IJournalTableSchema>() { new FoodItemTableSchema() },
                        new SqliteConnectionFactory(""));

                    var sessionInitializationOutcome =
                        sessionInitializationFacade.InitializeSessionFromNewJournalFileAtPath(fullJournalFilePath);
                    if (sessionInitializationOutcome == false)
                    {
                        switch (sessionInitializationFacade.GetInitializationFailureReason())
                        {
                            case SessionInitializationFailureReason.JOURNAL_FILE_ALREADY_EXISTS:
                                await Shell.Current.DisplayAlert("Error", "There is already a Journal file with this name at the same destination.", "Ok");
                                break;
                            default:
                                await Shell.Current.DisplayAlert("Error", "Failed to create and initialize a Journal file at this destination (unknown reason).", "Ok");
                                break;
                        }
                        Application.Current.Quit();
                    }

                    Shell.Current.DisplayAlert("Done!", "Journal created successfully!", "Ok");
                    Preferences.Default.Set(JournalFilePathPreferencesKey,fullJournalFilePath);
                    _session = sessionInitializationFacade.GetSession();
                }
            }
            else
            {
                var sessionInitializationFacade = new SessionInitializationFacade(
                    new List<IJournalTableSchema>() { new FoodItemTableSchema() },
                    new SqliteConnectionFactory(""));

                var sessionInitializationOutcome = sessionInitializationFacade.InitializeSessionFromExistingJournalFileAtPath(Preferences.Default.Get(JournalFilePathPreferencesKey, ""));
                if (sessionInitializationOutcome == false)
                {
                    switch (sessionInitializationFacade.GetInitializationFailureReason())
                    {
                        case SessionInitializationFailureReason.JOURNAL_FILE_DOES_NOT_EXIST:
                            await Shell.Current.DisplayAlert("Unable to initialize Session from existing file", $"File does not exist. {Preferences.Default.Get(JournalFilePathPreferencesKey,"")}", "Ok");
                            break;
                        case SessionInitializationFailureReason.JOURNAL_FILE_IS_MISSING_TABLES:
                            await Shell.Current.DisplayAlert("Unable to initialize Session from existing file", "Journal file is missing tables, corruption likely.", "Ok");
                            break;
                        default:
                            await Shell.Current.DisplayAlert("Unable to initialize Session from existing file", "Unknown reason.", "Ok");
                            break;
                    }
                    Application.Current.Quit();
                }
                _session = sessionInitializationFacade.GetSession();
            }
        }
    }
}
