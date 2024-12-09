using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodItemShortcutTableSchema
{
    [TestFixture]
    public class WhenAConnectionContainingTheCurrentFoodItemShortcutTableSchemaIsValidated
    {
        bool _result = false;

        [OneTimeSetUp]
        public void Setup()
        {
            var connectionFactory = new TestingSqliteConnectionFactory();
            var subject = new AcaiCore.FoodItemShortcutTableSchema();

            using (var connection = connectionFactory.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE \"food_item_shortcuts\" (\"id\" INTEGER NOT NULL UNIQUE, \"name\" TEXT NOT NULL, \"calories\" REAL NOT NULL, PRIMARY KEY (\"id\" AUTOINCREMENT))";
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
