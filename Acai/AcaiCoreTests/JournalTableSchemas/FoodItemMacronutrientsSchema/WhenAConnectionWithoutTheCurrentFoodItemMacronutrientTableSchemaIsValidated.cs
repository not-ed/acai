using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodItemMacronutrientsSchema
{
    [TestFixture]
    public class WhenAConnectionWithoutTheCurrentFoodItemMacronutrientTableSchemaIsValidated
    {
        bool _result = false;

        [OneTimeSetUp]
        public void Setup()
        {
            var connectionFactory = new TestingSqliteConnectionFactory();
            var subject = new AcaiCore.FoodItemMacronutrientsSchema();

            using (var connection = connectionFactory.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = new AcaiCore.FoodItemTableSchema().GetSQLTableCreationQuery();
                    command.ExecuteNonQuery();
                }
                
                _result = subject.PresentInConnection(connection);
            }
        }

        [Test]
        public void ThenTheExpectedOutcomeIsFalse()
        {
            Assert.That(_result, Is.False);
        }
    }
}
