using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodItemShortcutGateway;

public class WhenANewFoodItemShortcutIsCreated
{
    FoodItemShortcutDTO _result;

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
            }
        }

        var subject = new AcaiCore.FoodItemShortcutGateway(connectionFactory);
        _result = subject.CreateNewFoodItemShortcut("Test Item Shortcut", 100);
    }

    [Test]
    public void ThenAnIDForTheNewItemShortcutIsReturned()
    {
        Assert.That(_result.GetID(), Is.EqualTo(1));
    }

    [Test]
    public void ThenTheCorrectNameIsWritten()
    {
        Assert.That(_result.GetName(), Is.EqualTo("Test Item Shortcut"));
    }
    
    [Test]
    public void ThenTheNumberOfCaloriesIsWritten()
    {
        Assert.That(_result.GetCalories(), Is.EqualTo(100));
    }
}