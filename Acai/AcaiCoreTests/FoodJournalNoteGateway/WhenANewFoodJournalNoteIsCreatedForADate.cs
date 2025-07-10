using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodJournalNoteGateway;

[TestFixture]
public class WhenANewFoodJournalNoteIsCreatedForADate
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
        _result = subject.CreateOrUpdateNoteForDate(new DateTime(2025, 6, 20), "This is a test note for today's food journal.");
    }

    [Test]
    public void ThenAnIDForTheNewNoteIsReturned()
    {
        Assert.That(_result.GetID(), Is.EqualTo(1));
    }
    
    [Test]
    public void ThenTheCorrectContentIsReturned()
    {
        Assert.That(_result.GetContent(), Is.EqualTo("This is a test note for today's food journal."));
    }

    [Test]
    public void ThenTheCorrectDateIsReturned()
    {
        Assert.That(_result.GetDate().Year, Is.EqualTo(2025));
        Assert.That(_result.GetDate().Month, Is.EqualTo(6));
        Assert.That(_result.GetDate().Day, Is.EqualTo(20));
    }
}