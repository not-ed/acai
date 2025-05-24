using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace AcaiMobile;

public static class AcaiUpdateChecker
{
    private static readonly IReleaseRetriever ReleaseRetriever = new GithubReleaseRetriever();

    private static bool _updateInProgress = false;
    
    private const string LastUpdateTimePreferencesKey = "lastAutomaticUpdateCheckPerformed";
    
    private static readonly IToast InitiationToast = Toast.Make("Checking for updates...");
    private static readonly IToast NoUpdatesFoundToast = Toast.Make("No new updates found.");

    public static void PerformAutomaticUpdateCheck()
    {
        DateTime lastAutomaticCheck = Preferences.Get(LastUpdateTimePreferencesKey, DateTime.MinValue);
        if (DateTime.Now > lastAutomaticCheck.AddDays(1))
        {
            CheckForUpdates();
            Preferences.Set(LastUpdateTimePreferencesKey, DateTime.Now);
        }
    }
    
    public static async void CheckForUpdates()
    {
        if (!_updateInProgress)
        {
            _updateInProgress = true;
            
            InitiationToast.Show();
            var newRelease = await ReleaseRetriever.CheckForNewReleases();
            InitiationToast.Dismiss();
            
            if (newRelease != null)
            {
                var updateAlert = await Shell.Current.DisplayAlert($"{newRelease.Version ?? "Update"} Available", $"A new Release of Acai is available, which brings about a series of improvements and fixes.\n\nWould you like to download this Release now?", "Download", "Dismiss");
                if (updateAlert)
                {
                    try
                    {
                        Browser.Default.OpenAsync(newRelease.DirectDownloadUrl ?? newRelease.ReleasePageUrl, BrowserLaunchMode.SystemPreferred);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            else
            {
                var updateCheckThrewAnException = ReleaseRetriever.GetExceptionMessage() != null;
                var resultToast = updateCheckThrewAnException ? Toast.Make($"Unable to check for updates ({ReleaseRetriever.GetExceptionMessage()}).") : NoUpdatesFoundToast;
                resultToast.Show();
            }

            _updateInProgress = false;
        }
    }
}