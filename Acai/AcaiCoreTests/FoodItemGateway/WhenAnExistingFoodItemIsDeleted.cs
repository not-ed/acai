using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodItemGateway
{
    [TestFixture]
    class WhenAnExistingFoodItemIsDeleted
    {
        private List<FoodItemDTO> _result;

        [OneTimeSetUp]
        public void Setup()
        {
            var connectionFactory = new TestingSqliteConnectionFactory();

            using (var connection = connectionFactory.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = new FoodItemTableSchema().GetSQLTableCreationQuery();
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO food_items (id, name, calories, created_at) VALUES" +
                                          "(1, 'Test Item 1', 100, '2024-09-06 12:16:20')," +
                                          "(2, 'Test Item 2', 200, '2024-09-06 13:17:21')," +
                                          "(3, 'Test Item 3', 300, '2024-09-06 14:18:22')," +
                                          "(4, 'Test Item 4', 400, '2024-09-06 15:19:23');";
                    command.ExecuteNonQuery();
                }
            }

            var subject = new AcaiCore.FoodItemGateway(connectionFactory);
            subject.DeleteFoodItem(2);
            _result = subject.GetFoodItemsForDate(new DateTime(2024, 9, 6));
        }

        [Test]
        public void ThenTheFoodItemNoLongerExists()
        {
            var deletedItem = _result.FirstOrDefault(x => x.GetID() == 2);
            Assert.That(deletedItem, Is.Null);
        }

        [Test]
        public void ThenOnlyTheRequestedFoodItemIsDeleted()
        {
            Assert.That(_result.Count, Is.EqualTo(3));

            var firstRemainingFoodItem = _result.First(x => x.GetID() == 1);
            Assert.That(firstRemainingFoodItem.GetName(), Is.EqualTo("Test Item 1"));
            Assert.That(firstRemainingFoodItem.GetCalories(), Is.EqualTo(100));
            Assert.That(firstRemainingFoodItem.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(firstRemainingFoodItem.GetCreationDate().Month, Is.EqualTo(9));
            Assert.That(firstRemainingFoodItem.GetCreationDate().Day, Is.EqualTo(6));
            Assert.That(firstRemainingFoodItem.GetCreationDate().Hour, Is.EqualTo(12));
            Assert.That(firstRemainingFoodItem.GetCreationDate().Minute, Is.EqualTo(16));
            Assert.That(firstRemainingFoodItem.GetCreationDate().Second, Is.EqualTo(20));
            
            var secondRemainingFoodItem = _result.First(x => x.GetID() == 3);
            Assert.That(secondRemainingFoodItem.GetName(), Is.EqualTo("Test Item 3"));
            Assert.That(secondRemainingFoodItem.GetCalories(), Is.EqualTo(300));
            Assert.That(secondRemainingFoodItem.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(secondRemainingFoodItem.GetCreationDate().Month, Is.EqualTo(9));
            Assert.That(secondRemainingFoodItem.GetCreationDate().Day, Is.EqualTo(6));
            Assert.That(secondRemainingFoodItem.GetCreationDate().Hour, Is.EqualTo(14));
            Assert.That(secondRemainingFoodItem.GetCreationDate().Minute, Is.EqualTo(18));
            Assert.That(secondRemainingFoodItem.GetCreationDate().Second, Is.EqualTo(22));

            var thirdRemainingFoodItem = _result.First(x => x.GetID() == 4);
            Assert.That(thirdRemainingFoodItem.GetName(), Is.EqualTo("Test Item 4"));
            Assert.That(thirdRemainingFoodItem.GetCalories(), Is.EqualTo(400));
            Assert.That(thirdRemainingFoodItem.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(thirdRemainingFoodItem.GetCreationDate().Month, Is.EqualTo(9));
            Assert.That(thirdRemainingFoodItem.GetCreationDate().Day, Is.EqualTo(6));
            Assert.That(thirdRemainingFoodItem.GetCreationDate().Hour, Is.EqualTo(15));
            Assert.That(thirdRemainingFoodItem.GetCreationDate().Minute, Is.EqualTo(19));
            Assert.That(thirdRemainingFoodItem.GetCreationDate().Second, Is.EqualTo(23));
        }
    }
}
