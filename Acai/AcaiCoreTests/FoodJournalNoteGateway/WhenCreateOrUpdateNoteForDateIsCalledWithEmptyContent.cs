using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodJournalNoteGateway;

[TestFixture]
public class WhenCreateOrUpdateNoteForDateIsCalledWithEmptyContent
{
    private FoodJournalNoteDTO _result;

    [OneTimeSetUp]
    public void Setup()
    {
        var connectionFactory = new TestingSqliteConnectionFactory();
        using (var connection = connectionFactory.CreateOpenConnection())
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = new FoodJournalNotesTableSchema().GetSQLTableCreationQuery();
                command.ExecuteNonQuery();
            }
        }
        
        var subject = new AcaiCore.FoodJournalNoteGateway(connectionFactory);
        _result = subject.CreateOrUpdateNoteForDate(new DateTime(2025, 6, 20), string.Empty);
    }

    [Test]
    public void ThenAnIDForTheNoteIsReturned()
    {
        Assert.That(_result.GetID(), Is.EqualTo(1));
    }
    
    [Test]
    public void ThenEmptyContentIsReturned()
    {
        Assert.That(_result.GetContent(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void ThenTheCorrectDateIsReturned()
    {
        Assert.That(_result.GetDate().Date, Is.EqualTo(new DateTime(2025, 6, 20).Date));
    }
}