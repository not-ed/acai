using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodItemTableSchema
{
    [TestFixture]
    public class WhenTheSQLCreationQueryOfTheFoodItemTableIsRequested
    {
        string _result;
        
        [OneTimeSetUp]
        public void Setup()
        {
            var subject = new AcaiCore.FoodItemTableSchema();
            _result = subject.GetSQLTableCreationQuery();
        }

        [Test]
        public void ThenTheCorrectQueryStringIsReturned()
        {
            Assert.That(_result, Is.EqualTo("CREATE TABLE \"food_items\" (\"id\" INTEGER NOT NULL UNIQUE,\"name\" TEXT NOT NULL,\"calories\" REAL NOT NULL,\"created_at\" TEXT NOT NULL, PRIMARY KEY(\"id\" AUTOINCREMENT))"));
        }
    }
}
