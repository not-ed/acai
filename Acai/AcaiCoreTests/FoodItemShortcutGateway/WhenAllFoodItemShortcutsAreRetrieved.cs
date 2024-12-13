using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodItemShortcutGateway;

[TestFixture]
public class WhenAllFoodItemShortcutsAreRetrieved
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
                                      "(3, 'Test Shortcut 3', 300);";
                command.ExecuteNonQuery();
            }
        }

        var subject = new AcaiCore.FoodItemShortcutGateway(connectionFactory);
        _result = subject.GetAllFoodItemShortcuts();
    }

    [Test]
    public void ThenTheCorrectNumberOfShortcutsAreReturned()
    {
        Assert.That(_result.Count, Is.EqualTo(3));
    }

    [TestCase(1, "Test Shortcut 1", 100)]
    [TestCase(2, "Test Shortcut 2", 200)]
    [TestCase(3, "Test Shortcut 3", 300)]
    public void ThenEachShortcutReturnedIsCorrectlyPopulated(long expectedId, string expectedName, float expectedCalories)
    {
        var shortcut = _result.First(x => x.GetID() == expectedId);
        Assert.That(shortcut.GetName(), Is.EqualTo(expectedName));
        Assert.That(shortcut.GetCalories(), Is.EqualTo(expectedCalories));
    }
}