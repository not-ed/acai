using AcaiCore;
using Microsoft.Data.Sqlite;
using Moq;
using NUnit.Framework;

namespace AcaiCoreTests.SessionInitializationFacade
{
    [TestFixture]
    public class WhenAttemptingToInitializeASessionFromAnExistingFileWhichIsMissingTables
    {
        private readonly string _journalFilePath = "existing-journal-file.sqlite";
        private AcaiCore.SessionInitializationFacade _subject;
        private bool _result;

        [OneTimeSetUp]
        public void Setup()
        {
            File.Create(_journalFilePath).Close();

            var mockTable = new Mock<IJournalTableSchema>();
            mockTable.Setup(x => x.PresentInConnection(It.IsAny<SqliteConnection>()))
                .Returns(false);
            var tableSchemas = new List<IJournalTableSchema>()
            {
                mockTable.Object
            };

            _subject = new AcaiCore.SessionInitializationFacade(tableSchemas, new TestingSqliteConnectionFactory());
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
        public void ThenTheInitializationIsUnsuccessful()
        {
            Assert.That(_result, Is.False);
        }

        [Test]
        public void ThenTheCorrectFailureReasonIsGiven()
        {
            Assert.That(_subject.GetInitializationFailureReason, Is.EqualTo(AcaiCore.SessionInitializationFailureReason.JOURNAL_FILE_IS_MISSING_TABLES));
        }

        [Test]
        public void ThenNoSessionIsReturned()
        {
            Assert.That(_subject.GetSession(), Is.Null);
        }
    }
}
