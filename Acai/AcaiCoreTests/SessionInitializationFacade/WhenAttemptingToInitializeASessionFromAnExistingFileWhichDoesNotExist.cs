using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.SessionInitializationFacade
{
    [TestFixture]
    public class WhenAttemptingToInitializeASessionFromAnExistingFileWhichDoesNotExist
    {
        private AcaiCore.SessionInitializationFacade _subject;
        private bool _result;

        [OneTimeSetUp]
        public void Setup()
        {
            _subject = new AcaiCore.SessionInitializationFacade(new List<IJournalTableSchema>(), new TestingSqliteConnectionFactory());
            _result = _subject.InitializeSessionFromExistingJournalFileAtPath("nonexistent-journal-file.sqlite");
        }

        [Test]
        public void ThenTheInitializationIsUnsuccessful()
        {
            Assert.That(_result, Is.False);
        }

        [Test]
        public void ThenTheCorrectFailureReasonIsGiven()
        {
            Assert.That(_subject.GetInitializationFailureReason, Is.EqualTo(AcaiCore.SessionInitializationFailureReason.JOURNAL_FILE_DOES_NOT_EXIST));
        }

        [Test]
        public void ThenNoSessionIsReturned()
        {
            Assert.That(_subject.GetSession(), Is.Null);
        }
    }
}
