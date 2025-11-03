using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.WeightJournalTableSchema
{
    [TestFixture]
    public class WhenTheSQLCreationQueryOfTheWeightJournalTableIsRequested
    {
        string _result;

        [OneTimeSetUp]
        public void Setup()
        {
            var subject = new AcaiCore.WeightJournalTableSchema();
            _result = subject.GetSQLTableCreationQuery();
        }

        [Test]
        public void ThenTheCorrectQueryStringIsReturned()
        {
            Assert.That(_result, Is.EqualTo("CREATE TABLE \"weigh_in_entries\" (\"id\" INTEGER NOT NULL UNIQUE,\"date\" TEXT NOT NULL,\"canonical_lbs\" REAL NOT NULL,\"body_fat_percentage\" REAL,\"note\" TEXT, PRIMARY KEY(\"id\" AUTOINCREMENT))"));
        }
    }
}
