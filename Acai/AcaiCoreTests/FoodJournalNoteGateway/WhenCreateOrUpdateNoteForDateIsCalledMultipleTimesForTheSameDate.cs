using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodJournalNoteGateway;

[TestFixture]
public class WhenCreateOrUpdateNoteForDateIsCalledMultipleTimesForTheSameDate
{
    private FoodJournalNoteDTO _firstResult;
    private FoodJournalNoteDTO _secondResult;
    private FoodJournalNoteDTO _thirdResult;
    
    private TestingSqliteConnectionFactory _connectionFactory;

    [OneTimeSetUp]
    public void Setup()
    {
        _connectionFactory = new TestingSqliteConnectionFactory();
        using (var connection = _connectionFactory.CreateOpenConnection())
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = new FoodJournalNotesTableSchema().GetSQLTableCreationQuery();
                command.ExecuteNonQuery();
            }
        }
        
        var subject = new AcaiCore.FoodJournalNoteGateway(_connectionFactory);
        
        _firstResult = subject.CreateOrUpdateNoteForDate(new DateTime(2025, 6, 20), "First note content");
        _secondResult = subject.CreateOrUpdateNoteForDate(new DateTime(2025, 6, 20), "Second note content - updated");
        _thirdResult = subject.CreateOrUpdateNoteForDate(new DateTime(2025, 6, 20), "Third and final note content - updated again");
    }

    [Test]
    public void ThenAllCallsReturnTheSameID()
    {
        Assert.That(_firstResult.GetID(), Is.EqualTo(_secondResult.GetID()));
        Assert.That(_secondResult.GetID(), Is.EqualTo(_thirdResult.GetID()));
    }
    
    [Test]
    public void ThenTheContentIsUpdatedEachTime()
    {
        Assert.That(_firstResult.GetContent(), Is.EqualTo("First note content"));
        Assert.That(_secondResult.GetContent(), Is.EqualTo("Second note content - updated"));
        Assert.That(_thirdResult.GetContent(), Is.EqualTo("Third and final note content - updated again"));
    }

    [Test]
    public void ThenAllCallsReturnTheSameDate()
    {
        Assert.That(_firstResult.GetDate().Date, Is.EqualTo(new DateTime(2025, 6, 20).Date));
        Assert.That(_secondResult.GetDate().Date, Is.EqualTo(new DateTime(2025, 6, 20).Date));
        Assert.That(_thirdResult.GetDate().Date, Is.EqualTo(new DateTime(2025, 6, 20).Date));
    }

    [Test]
    public void ThenOnlyOneRecordExistsInTheDatabase()
    {
        using (var connection = _connectionFactory.CreateOpenConnection())
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM food_journal_notes WHERE date(date) = date('2025-06-20')";
                var count = (long)command.ExecuteScalar();
                Assert.That(count, Is.EqualTo(1));
            }
        }
    }
}