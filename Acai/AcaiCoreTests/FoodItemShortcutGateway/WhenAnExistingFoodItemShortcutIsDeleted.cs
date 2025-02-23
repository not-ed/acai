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

                command.CommandText = new FoodItemShortcutMacronutrientsSchema().GetSQLTableCreationQuery();
                command.ExecuteNonQuery();
                
                command.CommandText = "INSERT INTO food_item_shortcuts (id, name, calories, protein, carbohydrates, fat, fibre, water) VALUES" +
                                      "(1, 'Test Shortcut 1', 100, 10, 20, 30, 40, 50)," +
                                      "(2, 'Test Shortcut 2', 200, 60, 70, 80, 90, 100)," +
                                      "(3, 'Test Shortcut 3', 300, 110, 120, 130, 140, 150)," +
                                      "(4, 'Test Shortcut 4', 400, 160, 170, 180, 190, 200);";
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
                command.CommandText = "SELECT id, name, calories, protein, carbohydrates, fat, fibre, water FROM food_item_shortcuts;";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _result.Add(new FoodItemShortcutDTO(
                            reader.GetInt64(0),
                            reader.GetString(1),
                            reader.GetFloat(2),
                            reader.GetFloat(3),
                            reader.GetFloat(4),
                            reader.GetFloat(5),
                            reader.GetFloat(6),
                            reader.GetFloat(7)));
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
        Assert.Multiple(() =>
        {
            Assert.That(firstRemainingFoodItem.GetName(), Is.EqualTo("Test Shortcut 1"));
            Assert.That(firstRemainingFoodItem.GetCalories(), Is.EqualTo(100));
            Assert.That(firstRemainingFoodItem.GetProtein(), Is.EqualTo(10));
            Assert.That(firstRemainingFoodItem.GetCarbohydrates(), Is.EqualTo(20));
            Assert.That(firstRemainingFoodItem.GetFat(), Is.EqualTo(30));
            Assert.That(firstRemainingFoodItem.GetFibre(), Is.EqualTo(40));
            Assert.That(firstRemainingFoodItem.GetWater(), Is.EqualTo(50));
        });
        
        var secondRemainingFoodItem = _result.First(x => x.GetID() == 3);
        Assert.Multiple(() =>
        {
            Assert.That(secondRemainingFoodItem.GetName(), Is.EqualTo("Test Shortcut 3"));
            Assert.That(secondRemainingFoodItem.GetCalories(), Is.EqualTo(300));
            Assert.That(secondRemainingFoodItem.GetProtein(), Is.EqualTo(110));
            Assert.That(secondRemainingFoodItem.GetCarbohydrates(), Is.EqualTo(120));
            Assert.That(secondRemainingFoodItem.GetFat(), Is.EqualTo(130));
            Assert.That(secondRemainingFoodItem.GetFibre(), Is.EqualTo(140));
            Assert.That(secondRemainingFoodItem.GetWater(), Is.EqualTo(150));
        });
        
        var thirdRemainingFoodItem = _result.First(x => x.GetID() == 4);
        Assert.Multiple(() =>
        {
            Assert.That(thirdRemainingFoodItem.GetName(), Is.EqualTo("Test Shortcut 4"));
            Assert.That(thirdRemainingFoodItem.GetCalories(), Is.EqualTo(400));
            Assert.That(thirdRemainingFoodItem.GetProtein(), Is.EqualTo(160));
            Assert.That(thirdRemainingFoodItem.GetCarbohydrates(), Is.EqualTo(170));
            Assert.That(thirdRemainingFoodItem.GetFat(), Is.EqualTo(180));
            Assert.That(thirdRemainingFoodItem.GetFibre(), Is.EqualTo(190));
            Assert.That(thirdRemainingFoodItem.GetWater(), Is.EqualTo(200));
        });
    }
    
}