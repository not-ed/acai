using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodItemShortcutMacronutrientsSchema
{
    [TestFixture]
    public class WhenAConnectionContainingTheCurrentFoodItemShortcutMacronutrientTableSchemaIsValidated
    {
        bool _result = false;

        [OneTimeSetUp]
        public void Setup()
        {
            var connectionFactory = new TestingSqliteConnectionFactory();
            var subject = new AcaiCore.FoodItemShortcutMacronutrientsSchema();

            using (var connection = connectionFactory.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = new AcaiCore.FoodItemShortcutTableSchema().GetSQLTableCreationQuery();
                    command.ExecuteNonQuery();
                    
                    command.CommandText = "ALTER TABLE food_item_shortcuts ADD COLUMN protein REAL;" + 
                                          "ALTER TABLE food_item_shortcuts ADD COLUMN carbohydrates REAL;" +
                                          "ALTER TABLE food_item_shortcuts ADD COLUMN fat REAL;" +
                                          "ALTER TABLE food_item_shortcuts ADD COLUMN fibre REAL;" +
                                          "ALTER TABLE food_item_shortcuts ADD COLUMN water REAL;";
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
