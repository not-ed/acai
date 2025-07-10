using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodJournalNotesTableSchema
{
    [TestFixture]
    public class WhenAConnectionContainingTheCurrentFoodJournalNotesTableSchemaIsValidated
    {
        bool _result = false;

        [OneTimeSetUp]
        public void Setup()
        {
            var connectionFactory = new TestingSqliteConnectionFactory();
            var subject = new AcaiCore.FoodJournalNotesTableSchema();

            using (var connection = connectionFactory.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE \"food_journal_notes\" (\"id\" INTEGER NOT NULL UNIQUE,\"date\" TEXT NOT NULL UNIQUE,\"content\" TEXT NOT NULL, PRIMARY KEY(\"id\" AUTOINCREMENT))";
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