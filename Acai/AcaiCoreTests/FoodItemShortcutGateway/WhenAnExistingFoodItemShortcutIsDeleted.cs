using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodItemShortcutGateway;

[TestFixture]
public class WhenAnExistingFoodItemShortcutIsDeleted
{
    private List<FoodItemShortcutDTO> _result;

    [OneTimeSetUp]
    public void Setup()
    {
        var connectionFactory = new TestingSqliteConnectionFactory();

        using (var connection = connectionFactory.CreateOpenConnection())
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = new FoodItemShortcutTableSchema().GetSQLTableCreationQuery();
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO food_item_shortcuts (id, name, calories) VALUES" +
                                      "(1, 'Test Shortcut 1', 100)," +
                                      "(2, 'Test Shortcut 2', 200)," +
                                      "(3, 'Test Shortcut 3', 300)," +
                                      "(4, 'Test Shortcut 4', 400);";
                command.ExecuteNonQuery();
            }
        }

        var subject = new AcaiCore.FoodItemShortcutGateway(connectionFactory);
        subject.DeleteFoodItemShortcut(2);
        _result = new List<FoodItemShortcutDTO>();

        using (var connection = connectionFactory.CreateOpenConnection())
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id, name, calories FROM food_item_shortcuts;";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _result.Add(new FoodItemShortcutDTO(reader.GetInt64(0),reader.GetString(1),reader.GetFloat(2), null, null, null, null, null));
                    }
                }
            }
        }
    }

    [Test]
    public void ThenTheRequestedShortcutNoLongerExists()
    {
        var deletedItem = _result.FirstOrDefault(x => x.GetID() == 2);
        Assert.That(deletedItem, Is.Null);
    }

    [Test]
    public void ThenOnlyTheRequestedShortcutIsDeleted()
    {
        Assert.That(_result.Count, Is.EqualTo(3));

        var firstRemainingFoodItem = _result.First(x => x.GetID() == 1);
        Assert.That(firstRemainingFoodItem.GetName(), Is.EqualTo("Test Shortcut 1"));
        Assert.That(firstRemainingFoodItem.GetCalories(), Is.EqualTo(100));
        
        var secondRemainingFoodItem = _result.First(x => x.GetID() == 3);
        Assert.That(secondRemainingFoodItem.GetName(), Is.EqualTo("Test Shortcut 3"));
        Assert.That(secondRemainingFoodItem.GetCalories(), Is.EqualTo(300));

        var thirdRemainingFoodItem = _result.First(x => x.GetID() == 4);
        Assert.That(thirdRemainingFoodItem.GetName(), Is.EqualTo("Test Shortcut 4"));
        Assert.That(thirdRemainingFoodItem.GetCalories(), Is.EqualTo(400));
    }
    
}