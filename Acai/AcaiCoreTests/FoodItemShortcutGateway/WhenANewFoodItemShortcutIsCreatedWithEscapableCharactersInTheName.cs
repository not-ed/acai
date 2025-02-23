using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodItemShortcutGateway;

public class WhenANewFoodItemShortcutIsCreatedWithEscapableCharactersInTheName
{
    private TestingSqliteConnectionFactory _connectionFactory;

    [OneTimeSetUp]
    public void Setup()
    {
        _connectionFactory = new TestingSqliteConnectionFactory();
        using (var connection = _connectionFactory.CreateOpenConnection())
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = new FoodItemShortcutTableSchema().GetSQLTableCreationQuery();
                command.ExecuteNonQuery();
                
                command.CommandText = new FoodItemShortcutMacronutrientsSchema().GetSQLTableCreationQuery();
                command.ExecuteNonQuery();
            }
        }
    }

    [Test]
    [TestCase("Something with 'single quotes' in it")]
    [TestCase("Something with \"quotes\" in it")]
    [TestCase("Something with a percentage % in it")]
    public void ThenTheCharactersAreCorrectlyEscapedInTheFinalRecord(string expectedItemName)
    {
        var subject = new AcaiCore.FoodItemShortcutGateway(_connectionFactory);
        var result = subject.CreateNewFoodItemShortcut(expectedItemName, 100, null, null, null, null, null);

        Assert.That(result.GetName(), Is.EqualTo(expectedItemName));
    }
}