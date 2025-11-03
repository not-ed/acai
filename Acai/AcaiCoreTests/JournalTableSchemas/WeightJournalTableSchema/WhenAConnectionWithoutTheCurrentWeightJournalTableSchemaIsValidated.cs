using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.JournalTableSchemas.WeightJournalTableSchema
{
    [TestFixture]
    public class WhenAConnectionWithoutTheCurrentWeightJournalTableSchemaIsValidated
    {
        bool _result;

        [OneTimeSetUp]
        public void Setup()
        {
            var connectionFactory = new TestingSqliteConnectionFactory();
            var subject = new AcaiCore.WeightJournalTableSchema();

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
