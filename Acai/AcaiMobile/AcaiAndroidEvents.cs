using CommunityToolkit.Maui.Alerts;

namespace AcaiMobile;

public static class AcaiAndroidEvents
{
    public static void OnResume()
    {
        var placeholderToast = Toast.Make("TODO: Check for Updates.");
        placeholderToast.Show();
    }
}