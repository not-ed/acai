namespace AcaiCore;

public class JournalTableSchemas
{
    public static readonly List<IJournalTableSchema> All = new List<IJournalTableSchema>()
    {
        new FoodItemTableSchema(),
        new FoodItemShortcutTableSchema(),
    };
}