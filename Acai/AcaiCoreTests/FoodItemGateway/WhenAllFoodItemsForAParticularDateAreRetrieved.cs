using AcaiCore;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AcaiCoreTests.FoodItemGateway
{
    [TestFixture]
    public class WhenAllFoodItemsForAParticularDateAreRetrieved
    {
        List<FoodItemDTO> _result;

        [OneTimeSetUp]
        public void Setup()
        {
            var connectionFactory = new TestingSqliteConnectionFactory();
            var subject = new AcaiCore.FoodItemGateway(connectionFactory);

            using (var connection = connectionFactory.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = new FoodItemTableSchema().GetSQLTableCreationQuery();
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO food_items (id, name, calories, created_at) VALUES" +
                        "(1, 'Test Item 1', 100, '2024-06-23 12:16:20')," +
                        "(2, 'Test Item 2', 200, '2024-06-23 13:17:21')," +
                        "(3, 'Test Item 3', 300, '2024-06-23 14:18:22')," +
                        "(4, 'Test Item 4', 400, '2024-06-22 15:19:23');";
                    command.ExecuteNonQuery();
                }
            }

            _result = subject.GetFoodItemsForDate(new DateTime(2024,6,23));
        }

        [Test]
        public void ThenOnlyTheCorrectItemsAreReturned()
        {
            Assert.That(_result.Count, Is.EqualTo(3));

            var firstItem = _result.FirstOrDefault(x => x.GetID() == 1);
            Assert.That(firstItem, Is.Not.Null);
            Assert.That(firstItem.GetName(),Is.EqualTo("Test Item 1"));
            Assert.That(firstItem.GetCalories(), Is.EqualTo(100));
            Assert.That(firstItem.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(firstItem.GetCreationDate().Month, Is.EqualTo(06));
            Assert.That(firstItem.GetCreationDate().Day, Is.EqualTo(23));
            Assert.That(firstItem.GetCreationDate().Hour, Is.EqualTo(12));
            Assert.That(firstItem.GetCreationDate().Minute, Is.EqualTo(16));
            Assert.That(firstItem.GetCreationDate().Second, Is.EqualTo(20));

            var secondItem = _result.FirstOrDefault(x => x.GetID() == 2);
            Assert.That(secondItem, Is.Not.Null);
            Assert.That(secondItem.GetName(),Is.EqualTo("Test Item 2"));
            Assert.That(secondItem.GetCalories(), Is.EqualTo(200));
            Assert.That(secondItem.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(secondItem.GetCreationDate().Month, Is.EqualTo(06));
            Assert.That(secondItem.GetCreationDate().Day, Is.EqualTo(23));
            Assert.That(secondItem.GetCreationDate().Hour, Is.EqualTo(13));
            Assert.That(secondItem.GetCreationDate().Minute, Is.EqualTo(17));
            Assert.That(secondItem.GetCreationDate().Second, Is.EqualTo(21));

            var thirdItem = _result.FirstOrDefault(x => x.GetID() == 3);
            Assert.That(thirdItem, Is.Not.Null);
            Assert.That(thirdItem.GetName(),Is.EqualTo("Test Item 3"));
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
