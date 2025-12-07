namespace AcaiCore;

public class WeightJournalGateway : IWeightJournalGateway
{
    public WeightJournalEntryDTO CreateNewWeighIn(DateTime creationDate, float canonicalPounds, float? bodyFatPercentage, string note)
    {
        throw new NotImplementedException();
    }

    public WeightJournalEntryDTO UpdateExistingWeighIn(long id, DateTime creationDate, float canonicalPounds, float? bodyFatPercentage, string note)
    {
        throw new NotImplementedException();
    }

    public WeightJournalEntryDTO DeleteWeighIn(long id)
    {
        throw new NotImplementedException();
    }
}