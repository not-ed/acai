namespace AcaiMobile;

public readonly struct PreferencesKeyDefaultValuePair<T>(string key, T defaultValue)
{
    public string Key { get; } = key;
    public T DefaultValue { get; } = defaultValue;
}

public static class PreferenceIndex
{
    public static readonly PreferencesKeyDefaultValuePair<float> DailyCaloricLimit = new("dailyCaloricLimit", 2000.0f);
    public static readonly PreferencesKeyDefaultValuePair<bool> WarnBeforeDiscardingFoodItemChanges = new("warnBeforeDiscardingFoodItemChanges", true);
    public static readonly PreferencesKeyDefaultValuePair<bool> DisplayProtein = new("displayProtein", true);
    public static readonly PreferencesKeyDefaultValuePair<bool> DisplayCarbohydrates = new("displayCarbohydrates", true);
    public static readonly PreferencesKeyDefaultValuePair<bool> DisplayFat = new("displayFat", true);
    public static readonly PreferencesKeyDefaultValuePair<bool> DisplayFibre = new("displayFibre", true);
    public static readonly PreferencesKeyDefaultValuePair<bool> DisplayWater = new("displayWater", true);
    public static readonly PreferencesKeyDefaultValuePair<bool> PerformAutomaticAppUpdates = new("performAutomaticAppUpdates", true);
}