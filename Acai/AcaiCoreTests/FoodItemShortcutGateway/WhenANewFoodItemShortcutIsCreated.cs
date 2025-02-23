using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodItemShortcutGateway;

public class WhenANewFoodItemShortcutIsCreated
{
    private AcaiCore.FoodItemShortcutGateway _subject;

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
            }
        }
        
        _subject = new AcaiCore.FoodItemShortcutGateway(connectionFactory);
    }

    [Test]
    [TestCase(1, "Test Item Shortcut", 100, 10f, 20f, 30f, 40f, 50f)]
    [TestCase(2, "Test Item Shortcut (Null macros)", 100, null, null, null, null, null)]
    public void ThenTheNewReturnedShortcutIsMappedCorrectly(int expectedId, string expectedName, float expectedCalories, float? expectedProtein, float? expectedCarbohydrates, float? expectedFat, float? expectedFibre, float? expectedWater)
    {
        var result = _subject.CreateNewFoodItemShortcut(expectedName, expectedCalories, expectedProtein, expectedCarbohydrates, expectedFat, expectedFibre, expectedWater);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.GetID(), Is.EqualTo(expectedId));
            Assert.That(result.GetName(), Is.EqualTo(expectedName));
            Assert.That(result.GetCalories(), Is.EqualTo(expectedCalories));
            Assert.That(result.GetProtein(), Is.EqualTo(expectedProtein));
            Assert.That(result.GetCarbohydrates(), Is.EqualTo(expectedCarbohydrates));
            Assert.That(result.GetFat(), Is.EqualTo(expectedFat));
            Assert.That(result.GetFibre(), Is.EqualTo(expectedFibre));
            Assert.That(result.GetWater(), Is.EqualTo(expectedWater));
        });
    }
}