namespace AcaiMobile;

public static class AcaiAndroidEvents
{
    public static void OnResume()
    {
        AcaiUpdateChecker.PerformAutomaticUpdateCheck();
    }
}