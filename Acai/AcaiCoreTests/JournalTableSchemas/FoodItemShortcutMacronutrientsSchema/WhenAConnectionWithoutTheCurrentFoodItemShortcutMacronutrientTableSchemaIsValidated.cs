using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodItemShortcutMacronutrientsSchema
{
    [TestFixture]
    public class WhenAConnectionWithoutTheCurrentFoodItemShortcutMacronutrientTableSchemaIsValidated
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
