using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodItemTableSchema
{
    [TestFixture]
    public class WhenAConnectionContainingTheCurrentFoodItemTableSchemaIsValidated
    {
        bool _result = false;

        [SetUp]
        public void Setup()
        {
            var connectionFactory = new TestingSqliteConnectionFactory();
            var subject = new AcaiCore.FoodItemTableSchema();

            using (var connection = connectionFactory.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE \"food_items\" (\"id\" INTEGER NOT NULL UNIQUE,\"name\" TEXT NOT NULL,\"calories\" REAL NOT NULL,\"created_at\" TEXT NOT NULL, PRIMARY KEY(\"id\" AUTOINCREMENT))";
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
