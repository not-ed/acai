using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodJournalNoteGateway;

[TestFixture]
public class WhenAnExistingFoodJournalNoteIsUpdatedForADate
{
    private FoodJournalNoteDTO _result;
    private int _originalNoteId;

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
                                      "(1, '2025-06-20', 'Original note content.')";
                command.ExecuteNonQuery();
                _originalNoteId = 1;
            }
        }
        
        var subject = new AcaiCore.FoodJournalNoteGateway(connectionFactory);
        _result = subject.CreateOrUpdateNoteForDate(new DateTime(2025, 6, 20), "Updated note content with new information.");
    }

    [Test]
    public void ThenTheSameIDAsTheOriginalNoteIsReturned()
    {
        Assert.That(_result.GetID(), Is.EqualTo(_originalNoteId));
    }
    
    [Test]
    public void ThenTheUpdatedContentIsReturned()
    {
        Assert.That(_result.GetContent(), Is.EqualTo("Updated note content with new information."));
    }

    [Test]
    public void ThenTheCorrectDateIsReturned()
    {
        Assert.That(_result.GetDate().Year, Is.EqualTo(2025));
        Assert.That(_result.GetDate().Month, Is.EqualTo(6));
        Assert.That(_result.GetDate().Day, Is.EqualTo(20));
    }
}