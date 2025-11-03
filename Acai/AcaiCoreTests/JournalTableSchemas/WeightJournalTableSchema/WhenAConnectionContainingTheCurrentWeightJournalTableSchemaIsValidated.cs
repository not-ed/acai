using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.WeightJournalTableSchema
{
    [TestFixture]
    public class WhenAConnectionContainingTheCurrentWeightJournalTableSchemaIsValidated
    {
        bool _result;

        [OneTimeSetUp]
        public void Setup()
        {
            var connectionFactory = new TestingSqliteConnectionFactory();
            var subject = new AcaiCore.WeightJournalTableSchema();

            using (var connection = connectionFactory.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE \"weigh_in_entries\" (\"id\" INTEGER NOT NULL UNIQUE,\"date\" TEXT NOT NULL,\"canonical_lbs\" REAL NOT NULL,\"body_fat_percentage\" REAL,\"note\" TEXT, PRIMARY KEY(\"id\" AUTOINCREMENT))";
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
