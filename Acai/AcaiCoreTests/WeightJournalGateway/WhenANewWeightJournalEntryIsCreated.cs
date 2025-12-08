using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.WeightJournalGateway;

public class WhenANewWeightJournalEntryIsCreated
{
    WeightJournalEntryDTO _result;
    
    [OneTimeSetUp]
    public void Setup()
    {
        var connectionFactory = new TestingSqliteConnectionFactory();
        using (var connection = connectionFactory.CreateOpenConnection())
        {
            using (var command = connection.CreateCommand())
            {
                foreach (var schema in AcaiCore.JournalTableSchemas.All)
                {
                    command.CommandText = schema.GetSQLTableCreationQuery();
                    command.ExecuteNonQuery();
                }
            }
        }
        
        var subject = new AcaiCore.WeightJournalGateway(connectionFactory);
        _result = subject.CreateNewWeighIn(new DateTime(2025, 12, 7, 18, 35, 32), 175, 30, "This is a test note.");
    }

    [Test]
    public void ThenAnIdForTheNewEntryIsWritten()
    {
        Assert.That(_result.GetID(), Is.EqualTo(1));
    }
    
    [Test]
    public void ThenTheCorrectCreationDateIsWritten()
    {
        Assert.That(_result.GetCreationDate().Year, Is.EqualTo(2025));
        Assert.That(_result.GetCreationDate().Month, Is.EqualTo(12));
        Assert.That(_result.GetCreationDate().Day, Is.EqualTo(7));

        Assert.That(_result.GetCreationDate().Hour, Is.EqualTo(18));
        Assert.That(_result.GetCreationDate().Minute, Is.EqualTo(35));
        Assert.That(_result.GetCreationDate().Second, Is.EqualTo(32));
    }
    
    [Test]
    public void ThenTheCorrectAmountOfCanonicalPoundsIsWritten()
    {
        Assert.That(_result.GetCanonicalPounds(), Is.EqualTo(175));
    }
    
    [Test]
    public void ThenTheCorrectBodyFatPercentageIsWritten()
    {
        Assert.That(_result.GetBodyFatPercentage(), Is.EqualTo(30));
    }
    
    [Test]
    public void ThenTheNoteIsWritten()
    {
        Assert.That(_result.GetNote(), Is.EqualTo("This is a test note."));
    }
}