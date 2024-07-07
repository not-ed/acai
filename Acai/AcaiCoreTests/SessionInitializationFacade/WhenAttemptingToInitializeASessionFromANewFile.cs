using NUnit.Framework;

namespace AcaiCoreTests.SessionInitializationFacade
{
    [TestFixture]
    public class WhenAttemptingToInitializeASessionFromANewFile
    {
        private readonly string _journalFilePath = "existing-journal-file.sqlite";
        private AcaiCore.SessionInitializationFacade _subject;
        private bool _result;

        [OneTimeSetUp]
        public void Setup()
        {
            _subject = new AcaiCore.SessionInitializationFacade();
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
        public void ThenNoFailureReasonIsGiven()
        {
            Assert.That(_subject.GetInitializationFailureReason, Is.EqualTo(AcaiCore.SessionInitializationFailureReason.NONE));
        }

        [Test]
        public void ThenNoSessionIsReturned()
        {
            Assert.That(_subject.GetSession(), Is.Null);
        }
    }
}
