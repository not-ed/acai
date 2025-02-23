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
                
                command.CommandText = new FoodItemShortcutMacronutrientsSchema().GetSQLTableCreationQuery();
                command.ExecuteNonQuery();
                
                command.CommandText = "INSERT INTO food_item_shortcuts (id, name, calories, protein, carbohydrates, fat, fibre, water) VALUES" +
                                      "(1, 'Test Shortcut 1', 100, 10, null, 20, null, 30)," +
                                      "(2, 'Test Shortcut 2', 200, null, 40, null, 50, null)," +
                                      "(3, 'Test Shortcut 3', 300, null, null, null, null, null);";
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

    [TestCase(1, "Test Shortcut 1", 100, 10f, null, 20f, null, 30f)]
    [TestCase(2, "Test Shortcut 2", 200, null, 40f, null, 50f, null)]
    [TestCase(3, "Test Shortcut 3", 300, null, null, null, null, null)]
    public void ThenEachShortcutReturnedIsCorrectlyPopulated(long expectedId, string expectedName, float expectedCalories, float? expectedProtein, float? expectedCarbohydrates, float? expectedFat, float? expectedFibre, float? expectedWater)
    {
        var shortcut = _result.First(x => x.GetID() == expectedId);
        Assert.Multiple(() =>
        {
            Assert.That(shortcut.GetName(), Is.EqualTo(expectedName));
            Assert.That(shortcut.GetCalories(), Is.EqualTo(expectedCalories));
            Assert.That(shortcut.GetProtein(), Is.EqualTo(expectedProtein));
            Assert.That(shortcut.GetCarbohydrates(), Is.EqualTo(expectedCarbohydrates));
            Assert.That(shortcut.GetFat(), Is.EqualTo(expectedFat));
            Assert.That(shortcut.GetFibre(), Is.EqualTo(expectedFibre));
            Assert.That(shortcut.GetWater(), Is.EqualTo(expectedWater));
        });
    }
}