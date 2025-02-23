namespace AcaiMobile;

public readonly struct PreferencesKeyDefaultValuePair<T>(string key, T defaultValue)
{
    public string Key { get; } = key;
    public T DefaultValue { get; } = defaultValue;
}

public static class PreferenceIndex
{
    public static readonly PreferencesKeyDefaultValuePair<float> DailyCaloricLimit = new("dailyCaloricLimit", 2000.0f);
    public static readonly PreferencesKeyDefaultValuePair<bool> DisplayProtein = new("displayProtein", true);
}