using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.SessionInitializationFacade
{
    [TestFixture]
    public class WhenAttemptingToInitializeASessionFromAnExistingFile
    {
        private readonly string _journalFilePath = "existing-journal-file.sqlite";
        private readonly List<IJournalTableSchema> _tableSchemas = AcaiCore.JournalTableSchemas.All;

        TestingSqliteConnectionFactory _connectionFactory;
        private AcaiCore.SessionInitializationFacade _subject;
        private bool _result;

        [OneTimeSetUp]
        public void Setup()
        {
            File.Create(_journalFilePath).Close();

            _connectionFactory = new TestingSqliteConnectionFactory();
            using (var connection = _connectionFactory.CreateOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    foreach (var schema in _tableSchemas)
                    {
                        command.CommandText = schema.GetSQLTableCreationQuery();
                        command.ExecuteNonQuery();
                    }
                }
            }

            _subject = new AcaiCore.SessionInitializationFacade(_tableSchemas, _connectionFactory);
            _result = _subject.InitializeSessionFromExistingJournalFileAtPath(_journalFilePath);
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
