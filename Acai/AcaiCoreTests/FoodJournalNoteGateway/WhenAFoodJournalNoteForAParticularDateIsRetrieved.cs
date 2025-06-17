using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodJournalNoteGateway;

[TestFixture]
public class WhenAFoodJournalNoteForAParticularDateIsRetrieved
{
    private FoodJournalNoteDTO? _result;

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
                
                command.CommandText = "INSERT INTO food_journal_notes (id, date, content) VALUES " +
                                      "(3, '2025-06-17', 'Test note content with **bold** and *italic* formatting.')";
                command.ExecuteNonQuery();
            }
        }
        
        var subject = new AcaiCore.FoodJournalNoteGateway(connectionFactory);
        _result = subject.GetNoteForDate(new DateTime(2025, 6, 17));
    }

    [Test]
    public void ThenAnIDForTheNewItemIsReturned()
    {
        Assert.That(_result.GetID(), Is.EqualTo(3));
    }
    
    [Test]
    public void ThenTheCorrectContentIsReturned()
    {
        Assert.That(_result.GetContent(), Is.EqualTo("Test note content with **bold** and *italic* formatting."));
    }

    [Test]
    public void ThenTheCorrectDateIsReturned()
    {
        Assert.That(_result.GetDate().Year, Is.EqualTo(2025));
        Assert.That(_result.GetDate().Month, Is.EqualTo(6));
        Assert.That(_result.GetDate().Day, Is.EqualTo(17));
    }
}