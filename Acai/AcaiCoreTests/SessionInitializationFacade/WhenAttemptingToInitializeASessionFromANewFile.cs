using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.SessionInitializationFacade
{
    [TestFixture]
    public class WhenAttemptingToInitializeASessionFromANewFile
    {
        private readonly string _journalFilePath = "existing-journal-file.sqlite";
        private readonly List<IJournalTableSchema> _tableSchemas = new List<IJournalTableSchema> {
            new FoodItemTableSchema(),
        };
        private readonly ISqliteConnectionFactory _connectionFactory = new TestingSqliteConnectionFactory();

        private AcaiCore.SessionInitializationFacade _subject;
        private bool _result;

        [OneTimeSetUp]
        public void Setup()
        {
            _subject = new AcaiCore.SessionInitializationFacade(_tableSchemas, _connectionFactory);
            _result = _subject.InitializeSessionFromNewJournalFileAtPath(_journalFilePath);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            if (File.Exists(_journalFilePath))
            {
                File.Delete(_journalFilePath);
            }
        }

        [Test]
        public void ThenTheInitializationIsSuccessful()
        {
            Assert.That(_result, Is.True);
        }

        [Test]
        public void ThenANewJournalFileHasSuccessfullyBeenCreated()
        {
            Assert.That(File.Exists(_journalFilePath), Is.True);
        }

        [Test]
        public void TheNewJournalFileHasHadAllRequiredTablesCreatedForIt()
        {
            using (var connection = _connectionFactory.CreateOpenConnection())
            {
                foreach (var table in _tableSchemas)
                {
                    Assert.That(table.PresentInConnection(connection));
                }
            }
        }

        [Test]
        public void ThenNoFailureReasonIsGiven()
        {
            Assert.That(_subject.GetInitializationFailureReason, Is.EqualTo(AcaiCore.SessionInitializationFailureReason.NONE));
        }

        [Test]
        public void ThenASessionIsReturned()
        {
            Assert.That(_subject.GetSession(), Is.Not.Null);
        }
    }
}
