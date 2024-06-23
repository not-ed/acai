using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodItemGateway
{
    [TestFixture]
    public class WhenAllFoodItemsForAParticularDateAreRetrieved
    {
        List<FoodItemDTO> _result;

        [SetUp]
        public void Setup()
        {
            var connectionFactory = new TestingSqliteConnectionFactory();
            var subject = new AcaiCore.FoodItemGateway(connectionFactory);

            using (var connection = connectionFactory.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO food_items (name, calories, created_at) VALUES" +
                        "('Test Item 1', 100, '2024-06-23 12:16:20')," +
                        "('Test Item 2', 200, '2024-06-23 13:17:21')," +
                        "('Test Item 3', 300, '2024-06-23 14:18:22')," +
                        "('Test Item 4', 400, '2024-06-22 15:19:23');";
                    command.ExecuteNonQuery();
                }
            }

            _result = subject.GetFoodItemsForDate(new DateTime(2024,6,23));
        }

        [Test]
        public void ThenOnlyTheCorrectItemsAreReturned()
        {
            Assert.That(_result.Count, Is.EqualTo(3));

            var firstItem = _result.Where(x => x.GetName() == "Test Item 1").FirstOrDefault();
            Assert.That(firstItem, Is.Not.Null);
            Assert.That(firstItem.GetCalories(), Is.EqualTo(100));
            Assert.That(firstItem.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(firstItem.GetCreationDate().Month, Is.EqualTo(06));
            Assert.That(firstItem.GetCreationDate().Day, Is.EqualTo(23));
            Assert.That(firstItem.GetCreationDate().Hour, Is.EqualTo(12));
            Assert.That(firstItem.GetCreationDate().Minute, Is.EqualTo(16));
            Assert.That(firstItem.GetCreationDate().Second, Is.EqualTo(20));


            var secondItem = _result.Where(x => x.GetName() == "Test Item 2").FirstOrDefault();
            Assert.That(secondItem, Is.Not.Null);
            Assert.That(secondItem.GetCalories(), Is.EqualTo(200));
            Assert.That(secondItem.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(secondItem.GetCreationDate().Month, Is.EqualTo(06));
            Assert.That(secondItem.GetCreationDate().Day, Is.EqualTo(23));
            Assert.That(secondItem.GetCreationDate().Hour, Is.EqualTo(13));
            Assert.That(secondItem.GetCreationDate().Minute, Is.EqualTo(17));
            Assert.That(secondItem.GetCreationDate().Second, Is.EqualTo(21));

            var thirdItem = _result.Where(x => x.GetName() == "Test Item 3").FirstOrDefault();
            Assert.That(thirdItem, Is.Not.Null);
            Assert.That(thirdItem.GetCalories(), Is.EqualTo(300));
            Assert.That(thirdItem.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(thirdItem.GetCreationDate().Month, Is.EqualTo(06));
            Assert.That(thirdItem.GetCreationDate().Day, Is.EqualTo(23));
            Assert.That(thirdItem.GetCreationDate().Hour, Is.EqualTo(14));
            Assert.That(thirdItem.GetCreationDate().Minute, Is.EqualTo(18));
            Assert.That(thirdItem.GetCreationDate().Second, Is.EqualTo(22));
        }
    }
}
