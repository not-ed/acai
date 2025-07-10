namespace AcaiCore;

public interface IFoodJournalNoteGateway
{
    public FoodJournalNoteDTO CreateOrUpdateNoteForDate(DateTime date, string content);
    public FoodJournalNoteDTO? GetNoteForDate(DateTime date);
}

public class FoodJournalNoteDTO
{
    private readonly long _id;
    private readonly DateTime _date;
    private readonly string _content;

    public FoodJournalNoteDTO(long id, DateTime date, string content)
    {
        _id = id;
        _date = date;
        _content = content;
    }

    public long GetID()
    {
        return _id;
    }

    public DateTime GetDate()
    {
        return _date;
    }

    public string GetContent()
    {
        return _content;
    }
}