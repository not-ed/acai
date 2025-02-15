using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodItemGateway
{
    [TestFixture]
    public class WhenANewFoodItemIsCreatedWithEscapableCharactersInItsName
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
                    command.CommandText = new FoodItemTableSchema().GetSQLTableCreationQuery();
                    command.ExecuteNonQuery();
                    
                    command.CommandText = new FoodItemMacronutrientsSchema().GetSQLTableCreationQuery();
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
            var subject = new AcaiCore.FoodItemGateway(_connectionFactory);
            var result = subject.CreateNewFoodItem(expectedItemName, 100, new DateTime(2024, 06, 22, 13, 14, 15), null, null, null, null, null);

            Assert.That(result.GetName(), Is.EqualTo(expectedItemName));
        }
    }
}