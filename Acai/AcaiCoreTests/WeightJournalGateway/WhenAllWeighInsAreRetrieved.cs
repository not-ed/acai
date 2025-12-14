using AcaiCore;
using NUnit.Framework;

namespace AcaiCoreTests.WeightJournalGateway;

public class WhenAllWeighInsAreRetrieved
{
    private List<WeightJournalEntryDTO> _result;
    
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
                
                command.CommandText = "INSERT INTO weigh_in_entries (id, date, canonical_lbs, body_fat_percentage, note) VALUES " +
                                      "(1, '2025-12-10 13:14:15', 175, 25, 'Test note 1')," +
                                      "(2, '2024-11-09 16:17:18', 175.5, null, 'Test note 2')," +
                                      "(3, '2023-10-08 19:20:21', 180, 25, null);";
                command.ExecuteNonQuery();
            }
        }
        
        var subject = new AcaiCore.WeightJournalGateway(connectionFactory);
        _result = subject.GetAllWeighIns();
    }

    [Test]
    public void ThenTheCorrectNumberOfWeighInsAreReturned()
    {
        Assert.That(_result.Count, Is.EqualTo(3));
    }

    [Test]
    public void ThenEachRetrievedWeighInIsPopulatedWithTheCorrectValues()
    {
        var firstEntry = _result.FirstOrDefault(x => x.GetID() == 1);
        var secondEntry = _result.FirstOrDefault(x => x.GetID() == 2);
        var thirdEntry = _result.FirstOrDefault(x => x.GetID() == 3);
        
        Assert.Multiple(() =>
        {
            Assert.That(firstEntry.GetCreationDate().Year, Is.EqualTo(2025));
            Assert.That(firstEntry.GetCreationDate().Month, Is.EqualTo(12));
            Assert.That(firstEntry.GetCreationDate().Day, Is.EqualTo(10));
            Assert.That(firstEntry.GetCreationDate().Hour, Is.EqualTo(13));
            Assert.That(firstEntry.GetCreationDate().Minute, Is.EqualTo(14));
            Assert.That(firstEntry.GetCreationDate().Second, Is.EqualTo(15));
            Assert.That(firstEntry.GetCanonicalPounds(), Is.EqualTo(175));
            Assert.That(firstEntry.GetBodyFatPercentage(), Is.EqualTo(25));
            Assert.That(firstEntry.GetNote(), Is.EqualTo("Test note 1"));
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(secondEntry.GetCreationDate().Year, Is.EqualTo(2024));
            Assert.That(secondEntry.GetCreationDate().Month, Is.EqualTo(11));
            Assert.That(secondEntry.GetCreationDate().Day, Is.EqualTo(9));
            Assert.That(secondEntry.GetCreationDate().Hour, Is.EqualTo(16));
            Assert.That(secondEntry.GetCreationDate().Minute, Is.EqualTo(17));
            Assert.That(secondEntry.GetCreationDate().Second, Is.EqualTo(18));
            Assert.That(secondEntry.GetCanonicalPounds(), Is.EqualTo(175.5));
            Assert.That(secondEntry.GetBodyFatPercentage(), Is.Null);
            Assert.That(secondEntry.GetNote(), Is.EqualTo("Test note 2"));
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(thirdEntry.GetCreationDate().Year, Is.EqualTo(2023));
            Assert.That(thirdEntry.GetCreationDate().Month, Is.EqualTo(10));
            Assert.That(thirdEntry.GetCreationDate().Day, Is.EqualTo(8));
            Assert.That(thirdEntry.GetCreationDate().Hour, Is.EqualTo(19));
            Assert.That(thirdEntry.GetCreationDate().Minute, Is.EqualTo(20));
            Assert.That(thirdEntry.GetCreationDate().Second, Is.EqualTo(21));
            Assert.That(thirdEntry.GetCanonicalPounds(), Is.EqualTo(180));
            Assert.That(thirdEntry.GetBodyFatPercentage(), Is.EqualTo(25));
            Assert.That(thirdEntry.GetNote(), Is.Null);
        });
    }
}