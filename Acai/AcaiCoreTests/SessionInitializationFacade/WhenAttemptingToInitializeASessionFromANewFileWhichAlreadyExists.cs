using NUnit.Framework;

namespace AcaiCoreTests.SessionInitializationFacade
{
    [TestFixture]
    public class WhenAttemptingToInitializeASessionFromANewFileWhichAlreadyExists
    {
        private readonly string _existingJournalFilePath = "existing-journal-file.sqlite";
        private AcaiCore.SessionInitializationFacade _subject;
        private bool _result;

        [SetUp]
        public void Setup()
        {
            File.Create("existing-journal-file.sqlite").Close();

            _subject = new AcaiCore.SessionInitializationFacade();
            _result = _subject.InitializeSessionFromNewJournalFileAtPath(_existingJournalFilePath);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            if (File.Exists(_existingJournalFilePath))
            {
                File.Delete(_existingJournalFilePath);
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
            Assert.That(_subject.GetInitializationFailureReason, Is.EqualTo(AcaiCore.SessionInitializationFailureReason.JOURNAL_FILE_ALREADY_EXISTS));
        }

        [Test]
        public void ThenNoSessionIsReturned()
        {
            Assert.That(_subject.GetSession(), Is.Null);
        }
    }
}
