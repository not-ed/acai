using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodItemGateway
{
    [TestFixture]
    class WhenAnExistingFoodItemIsUpdatedWithEscapableCharactersInItsName
    {
        private FoodItemDTO _result;
        private AcaiCore.FoodItemGateway _subject;
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

                    command.CommandText = "INSERT INTO food_items (id, name, calories, created_at) VALUES " +
                                          "(1, 'Test Item 1', 100, '2024-09-07 12:16:20');";
                    command.ExecuteNonQuery();
                }
            }

            _subject = new AcaiCore.FoodItemGateway(_connectionFactory);
        }

        [Test]
        [TestCase("Something with 'single quotes' in it")]
        [TestCase("Something with \"quotes\" in it")]
        [TestCase("Something with a percentage % in it")]
        public void ThenTheCharactersAreCorrectlyEscapedIntheUpdatedRecord(string expectedItemName)
        {
            using (var connection = _connectionFactory.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM food_items;";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO food_items (id, name, calories, created_at) VALUES " +
                                          "(1, 'Test Item 1', 100, '2024-09-07 12:16:20');";
                    command.ExecuteNonQuery();
                }
            }

            var result = _subject.UpdateExistingFoodItem(1, expectedItemName, 100, new DateTime(2024, 06, 22, 13, 14, 15));
            Assert.That(result.GetName() == expectedItemName);
        }
    }
}
