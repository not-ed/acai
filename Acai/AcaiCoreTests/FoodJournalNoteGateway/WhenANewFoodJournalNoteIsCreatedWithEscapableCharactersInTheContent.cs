using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodJournalNoteGateway;

[TestFixture]
public class WhenANewFoodJournalNoteIsCreatedWithEscapableCharactersInTheContent
{
    
    [TestCase("Note with 'single quotes' in the content")]
    [TestCase("Note with \"double quotes\" in the content")]  
    [TestCase("Note with a percentage % symbol in the content")]
    public void ThenTheContentIsStoredAndRetrievedCorrectly(string contentWithEscapableCharacters)
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
        var result = subject.CreateOrUpdateNoteForDate(new DateTime(2025, 6, 20), contentWithEscapableCharacters);
        
        Assert.That(result.GetContent(), Is.EqualTo(contentWithEscapableCharacters));
        Assert.That(result.GetID(), Is.EqualTo(1));
        Assert.That(result.GetDate().Date, Is.EqualTo(new DateTime(2025, 6, 20).Date));
    }
}