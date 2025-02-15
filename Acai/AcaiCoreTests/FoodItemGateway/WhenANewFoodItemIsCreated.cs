using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodItemGateway
{
    [TestFixture]
    public class WhenANewFoodItemIsCreated
    {
        FoodItemDTO _result;

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
                }
            }

            var subject = new AcaiCore.FoodItemGateway(connectionFactory);
            _result = subject.CreateNewFoodItem("Test Item", 100, new DateTime(2024, 06, 22, 13, 14, 15), 30, 7, 8, 12, 500);
        }

        [Test]
        public void ThenAnIDForTheNewItemIsReturned()
        {
            Assert.That(_result.GetID(), Is.EqualTo(1));
        }

        [Test]
        public void ThenTheCorrectNameIsWritten()
        {
            Assert.That(_result.GetName(), Is.EqualTo("Test Item"));
        }

        [Test]
        public void ThenTheCorrectAmountOfCaloriesIsWritten()
        {
            Assert.That(_result.GetCalories(), Is.EqualTo(100));
        }

        [Test]
        public void ThenTheCorrectCreationDateIsWritten()
        {
            Assert.That(_result.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(_result.GetCreationDate().Month, Is.EqualTo(6));
            Assert.That(_result.GetCreationDate().Day, Is.EqualTo(22));

            Assert.That(_result.GetCreationDate().Hour, Is.EqualTo(13));
            Assert.That(_result.GetCreationDate().Minute, Is.EqualTo(14));
            Assert.That(_result.GetCreationDate().Second, Is.EqualTo(15));
        }
        
        [Test]
        public void ThenTheCorrectAmountOfProteinIsWritten()
        {
            Assert.That(_result.GetProtein(), Is.EqualTo(30));
        }
        
        [Test]
        public void ThenTheCorrectAmountOfCarbohydratesIsWritten()
        {
            Assert.That(_result.GetCarbohydrates(), Is.EqualTo(7));
        }
        
        [Test]
        public void ThenTheCorrectAmountOfFatIsWritten()
        {
            Assert.That(_result.GetFat(), Is.EqualTo(8));
        }
        
        [Test]
        public void ThenTheCorrectAmountOfFibreIsWritten()
        {
            Assert.That(_result.GetFibre(), Is.EqualTo(12));
        }
        
        [Test]
        public void ThenTheCorrectAmountOfWaterIsWritten()
        {
            Assert.That(_result.GetWater(), Is.EqualTo(500));
        }
    }
}
