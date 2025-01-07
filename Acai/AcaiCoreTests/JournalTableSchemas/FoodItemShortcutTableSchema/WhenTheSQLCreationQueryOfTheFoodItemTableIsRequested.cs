using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodItemShortcutTableSchema
{
    [TestFixture]
    public class WhenTheSQLCreationQueryOfTheFoodItemShortcutTableIsRequested
    {
        string _result;
        
        [OneTimeSetUp]
        public void Setup()
        {
            var subject = new AcaiCore.FoodItemShortcutTableSchema();
            _result = subject.GetSQLTableCreationQuery();
        }

        [Test]
        public void ThenTheCorrectQueryStringIsReturned()
        {
            Assert.That(_result, Is.EqualTo("CREATE TABLE \"food_item_shortcuts\" (\"id\" INTEGER NOT NULL UNIQUE, \"name\" TEXT NOT NULL, \"calories\" REAL NOT NULL, PRIMARY KEY (\"id\" AUTOINCREMENT))"));
        }
    }
}
