using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodItemTableSchema
{
    [TestFixture]
    public class WhenAConnectionWithoutTheCurrentFoodItemTableSchemaIsValidated
    {
        bool _result = false;

        [SetUp]
        public void Setup()
        {
            var connectionFactory = new TestingSqliteConnectionFactory();
            var subject = new AcaiCore.FoodItemTableSchema();

            using (var connection = connectionFactory.CreateOpenConnection())
            {
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
