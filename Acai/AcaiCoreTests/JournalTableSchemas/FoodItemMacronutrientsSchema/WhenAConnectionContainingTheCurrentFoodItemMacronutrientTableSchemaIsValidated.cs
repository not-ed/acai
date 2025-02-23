using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodItemMacronutrientsSchema
{
    [TestFixture]
    public class WhenAConnectionContainingTheCurrentFoodItemMacronutrientTableSchemaIsValidated
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
                    
                    command.CommandText = "ALTER TABLE food_items ADD COLUMN protein REAL;" + 
                                          "ALTER TABLE food_items ADD COLUMN carbohydrates REAL;" +
                                          "ALTER TABLE food_items ADD COLUMN fat REAL;" +
                                          "ALTER TABLE food_items ADD COLUMN fibre REAL;" +
                                          "ALTER TABLE food_items ADD COLUMN water REAL;";
                    command.ExecuteNonQuery();
                }

                _result = subject.PresentInConnection(connection);
            }
        }

        [Test]
        public void ThenTheExpectedOutcomeIsTrue()
        {
            Assert.That(_result, Is.True);
        }
    }
}
