using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.FoodItemShortcutTableSchema
{
    [TestFixture]
    public class WhenAConnectionWithoutTheCurrentFoodItemShortcutTableSchemaIsValidated
    {
        bool _result = false;

        [OneTimeSetUp]
        public void Setup()
        {
            var connectionFactory = new TestingSqliteConnectionFactory();
            var subject = new AcaiCore.FoodItemShortcutTableSchema();

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
