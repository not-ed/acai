namespace AcaiCore;

public class FoodItemShortcutGateway : IFoodItemShortcutGateway
{
    private ISqliteConnectionFactory sqliteConnectionFactory;

    public FoodItemShortcutGateway(ISqliteConnectionFactory connectionFactory)
    {
        this.sqliteConnectionFactory = connectionFactory;
    }

    public FoodItemShortcutDTO CreateNewFoodItemShortcut(string name, float calories)
    {
        throw new NotImplementedException();
    }

    public void DeleteFoodItemShortcut(long id)
    {
        throw new NotImplementedException();
    }

    public List<FoodItemShortcutDTO> GetAllFoodItemShortcuts()
    {
        throw new NotImplementedException();
    }
}