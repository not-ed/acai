namespace AcaiCore;

public interface IWeightJournalGateway
{
    public WeightJournalEntryDTO CreateNewWeighIn(long id, DateTime creationDate, float canonicalPounds, float? bodyFatPercentage, string note);
    public WeightJournalEntryDTO UpdateExistingWeighIn(long id, DateTime creationDate, float canonicalPounds, float? bodyFatPercentage, string note);
    public WeightJournalEntryDTO DeleteWeighIn(long id);
}

public class WeightJournalEntryDTO
{
    private readonly long _id;
    private readonly DateTime _creationDate;
    private readonly float _canonicalPounds;
    private readonly float? _bodyFatPercentage;
    private readonly string _note;

    public WeightJournalEntryDTO(long id, DateTime creationDate, float canonicalPounds, float? bodyFatPercentage, string note)
    {
        this._id = id;
        this._creationDate = creationDate;
        this._canonicalPounds = canonicalPounds;
        this._bodyFatPercentage = bodyFatPercentage;
        this._note = note;
    }
    
    public long GetID() { return _id; }
    public DateTime GetCreationDate() { return _creationDate; }
    public float GetCanonicalPounds() { return _canonicalPounds; }
    public float? GetBodyFatPercentage() { return _bodyFatPercentage; }
    public string GetNote() { return _note; }
}