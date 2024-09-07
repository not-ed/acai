using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodItemGateway
{
    [TestFixture]
    class WhenAnExistingFoodItemIsUpdated
    {
        private FoodItemDTO _result;
        private AcaiCore.FoodItemGateway _subject;

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

                    command.CommandText = "INSERT INTO food_items (id, name, calories, created_at) VALUES " +
                                          "(1, 'Test Item 1', 100, '2024-09-07 12:16:20')," +
                                          "(2, 'Test Item 2', 200, '2024-09-07 13:17:21')," +
                                          "(3, 'Test Item 3', 300, '2024-09-07 14:18:22');";
                    command.ExecuteNonQuery();
                }
            }

            _subject = new AcaiCore.FoodItemGateway(connectionFactory);
            _result = _subject.UpdateExistingFoodItem(2, "Updated Item Name", 201, new DateTime(2024, 10, 8, 15, 19, 23));
        }

        [Test]
        public void ThenTheRequestedRecordIsReturnedWithTheNewDetails()
        {
            Assert.That(_result.GetID(), Is.EqualTo(2));
            Assert.That(_result.GetName(), Is.EqualTo("Updated Item Name"));
            Assert.That(_result.GetCalories(), Is.EqualTo(201));
            Assert.That(_result.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(_result.GetCreationDate().Month, Is.EqualTo(10));
            Assert.That(_result.GetCreationDate().Day, Is.EqualTo(8));
            Assert.That(_result.GetCreationDate().Hour, Is.EqualTo(15));
            Assert.That(_result.GetCreationDate().Minute, Is.EqualTo(19));
            Assert.That(_result.GetCreationDate().Second, Is.EqualTo(23));
        }

        [Test]
        public void ThenOnlyTheRequestedRecordIsUpdatedInTheDatabase()
        {
            var updatedRecord = _subject.GetFoodItemsForDate(new DateTime(2024,10,8)).First();
            Assert.That(updatedRecord.GetID(), Is.EqualTo(2));
            Assert.That(updatedRecord.GetName(), Is.EqualTo("Updated Item Name"));
            Assert.That(updatedRecord.GetCalories(), Is.EqualTo(201));
            Assert.That(updatedRecord.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(updatedRecord.GetCreationDate().Month, Is.EqualTo(10));
            Assert.That(updatedRecord.GetCreationDate().Day, Is.EqualTo(8));
            Assert.That(updatedRecord.GetCreationDate().Hour, Is.EqualTo(15));
            Assert.That(updatedRecord.GetCreationDate().Minute, Is.EqualTo(19));
            Assert.That(updatedRecord.GetCreationDate().Second, Is.EqualTo(23));

            var remainingRecords = _subject.GetFoodItemsForDate(new DateTime(2024, 9, 7));

            var firstRemainingRecord = remainingRecords.First(x => x.GetID() == 1);
            Assert.That(firstRemainingRecord.GetName(), Is.EqualTo("Test Item 1"));
            Assert.That(firstRemainingRecord.GetCalories(), Is.EqualTo(100));
            Assert.That(firstRemainingRecord.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(firstRemainingRecord.GetCreationDate().Month, Is.EqualTo(9));
            Assert.That(firstRemainingRecord.GetCreationDate().Day, Is.EqualTo(7));
            Assert.That(firstRemainingRecord.GetCreationDate().Hour, Is.EqualTo(12));
            Assert.That(firstRemainingRecord.GetCreationDate().Minute, Is.EqualTo(16));
            Assert.That(firstRemainingRecord.GetCreationDate().Second, Is.EqualTo(20));

            var secondRemainingRecord = remainingRecords.First(x => x.GetID() == 3);
            Assert.That(secondRemainingRecord.GetName(), Is.EqualTo("Test Item 3"));
            Assert.That(secondRemainingRecord.GetCalories(), Is.EqualTo(300));
            Assert.That(secondRemainingRecord.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(secondRemainingRecord.GetCreationDate().Month, Is.EqualTo(9));
            Assert.That(secondRemainingRecord.GetCreationDate().Day, Is.EqualTo(7));
            Assert.That(secondRemainingRecord.GetCreationDate().Hour, Is.EqualTo(14));
            Assert.That(secondRemainingRecord.GetCreationDate().Minute, Is.EqualTo(18));
            Assert.That(secondRemainingRecord.GetCreationDate().Second, Is.EqualTo(22));
        }

    }
}
