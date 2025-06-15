using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodJournalNotesTableSchema
{
    [TestFixture]
    public class WhenTheSQLCreationQueryOfTheFoodJournalNotesTableIsRequested
    {
        string _result;
        
        [OneTimeSetUp]
        public void Setup()
        {
            var subject = new AcaiCore.FoodJournalNotesTableSchema();
            _result = subject.GetSQLTableCreationQuery();
        }

        [Test]
        public void ThenTheCorrectQueryStringIsReturned()
        {
            Assert.That(_result, Is.EqualTo("CREATE TABLE \"food_journal_notes\" (\"id\" INTEGER NOT NULL UNIQUE,\"date\" TEXT NOT NULL UNIQUE,\"content\" TEXT NOT NULL, PRIMARY KEY(\"id\" AUTOINCREMENT))"));
        }
    }
}