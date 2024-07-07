using NUnit.Framework;

namespace AcaiCoreTests.SessionInitializationFacade
{
    [TestFixture]
    public class WhenAttemptingToGetASessionWhichHasNotBeenInitializedYet
    {
        AcaiCore.AcaiSession _result;

        [OneTimeSetUp]
        public void Setup()
        {
            var subject = new AcaiCore.SessionInitializationFacade();
            _result = subject.GetSession();
        }

        [Test]
        public void ThenNoSessionIsReturned()
        {
            Assert.That(_result, Is.Null);
        }
    }
}
