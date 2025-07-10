using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodJournalNoteGateway;

[TestFixture]
public class WhenAnExistingFoodJournalNoteIsUpdatedWithEscapableCharactersInTheContent
{
    
    [TestCase("Updated note with 'single quotes' in content")]
    [TestCase("Updated note with \"double quotes\" in content")]  
    [TestCase("Updated note with a percentage % symbol in content")]
    public void ThenTheContentIsUpdatedAndRetrievedCorrectly(string updatedContentWithEscapableCharacters)
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
            }
        }
        
        var subject = new AcaiCore.FoodJournalNoteGateway(connectionFactory);
        var result = subject.CreateOrUpdateNoteForDate(new DateTime(2025, 6, 20), updatedContentWithEscapableCharacters);
        
        Assert.That(result.GetContent(), Is.EqualTo(updatedContentWithEscapableCharacters));
        Assert.That(result.GetID(), Is.EqualTo(1));
        Assert.That(result.GetDate().Date, Is.EqualTo(new DateTime(2025, 6, 20).Date));
    }
}