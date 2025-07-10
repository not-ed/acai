using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.FoodJournalNoteGateway;

[TestFixture]
public class WhenAFoodJournalNoteForAParticularDateIsRetrievedThatDoesNotExist
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
            }
        }
        
        var subject = new AcaiCore.FoodJournalNoteGateway(connectionFactory);
        _result = subject.GetNoteForDate(new DateTime(2025, 6, 17));
    }

    [Test]
    public void ThenNullIsReturned()
    {
        Assert.That(_result, Is.Null);
    }
}