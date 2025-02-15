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

                    command.CommandText = new FoodItemMacronutrientsSchema().GetSQLTableCreationQuery();
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "INSERT INTO food_items (id, name, calories, created_at, protein, carbohydrates, fat, fibre, water) VALUES " +
                        "(1, 'Test Item 1', 100, '2024-09-07 12:16:20', 10, 11, 12, 13, 500)," +
                        "(2, 'Test Item 2', 200, '2024-09-07 13:17:21', 14, 15, 16, 17, 600)," +
                        "(3, 'Test Item 3', 300, '2024-09-07 14:18:22', 18, 19, 20, 21, 700);";
                    command.ExecuteNonQuery();
                }
            }

            _subject = new AcaiCore.FoodItemGateway(connectionFactory);
            _result = _subject.UpdateExistingFoodItem(2, "Updated Item Name", 201, new DateTime(2024, 10, 8, 15, 19, 23), 22, 23, 24, 25, 800);
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
            Assert.That(_result.GetProtein(), Is.EqualTo(22));
            Assert.That(_result.GetCarbohydrates(), Is.EqualTo(23));
            Assert.That(_result.GetFat(), Is.EqualTo(24));
            Assert.That(_result.GetFibre(), Is.EqualTo(25));
            Assert.That(_result.GetWater(), Is.EqualTo(800));
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
            Assert.That(updatedRecord.GetProtein(), Is.EqualTo(22));
            Assert.That(updatedRecord.GetCarbohydrates(), Is.EqualTo(23));
            Assert.That(updatedRecord.GetFat(), Is.EqualTo(24));
            Assert.That(updatedRecord.GetFibre(), Is.EqualTo(25));
            Assert.That(updatedRecord.GetWater(), Is.EqualTo(800));
            
            
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
            Assert.That(firstRemainingRecord.GetProtein(), Is.EqualTo(10));
            Assert.That(firstRemainingRecord.GetCarbohydrates(), Is.EqualTo(11));
            Assert.That(firstRemainingRecord.GetFat(), Is.EqualTo(12));
            Assert.That(firstRemainingRecord.GetFibre(), Is.EqualTo(13));
            Assert.That(firstRemainingRecord.GetWater(), Is.EqualTo(500));
            
            var secondRemainingRecord = remainingRecords.First(x => x.GetID() == 3);
            Assert.That(secondRemainingRecord.GetName(), Is.EqualTo("Test Item 3"));
            Assert.That(secondRemainingRecord.GetCalories(), Is.EqualTo(300));
            Assert.That(secondRemainingRecord.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(secondRemainingRecord.GetCreationDate().Month, Is.EqualTo(9));
            Assert.That(secondRemainingRecord.GetCreationDate().Day, Is.EqualTo(7));
            Assert.That(secondRemainingRecord.GetCreationDate().Hour, Is.EqualTo(14));
            Assert.That(secondRemainingRecord.GetCreationDate().Minute, Is.EqualTo(18));
            Assert.That(secondRemainingRecord.GetCreationDate().Second, Is.EqualTo(22));
            Assert.That(secondRemainingRecord.GetProtein(), Is.EqualTo(18));
            Assert.That(secondRemainingRecord.GetCarbohydrates(), Is.EqualTo(19));
            Assert.That(secondRemainingRecord.GetFat(), Is.EqualTo(20));
            Assert.That(secondRemainingRecord.GetFibre(), Is.EqualTo(21));
            Assert.That(secondRemainingRecord.GetWater(), Is.EqualTo(700));
        }

    }
}
