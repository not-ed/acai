namespace AcaiCore;

public class FoodJournalNoteGateway : IFoodJournalNoteGateway
{
    private ISqliteConnectionFactory sqliteConnectionFactory;

    public FoodJournalNoteGateway(ISqliteConnectionFactory connectionFactory)
    {
        this.sqliteConnectionFactory = connectionFactory;
    }
    
    public FoodJournalNoteDTO CreateOrUpdateNoteForDate(DateTime date, string content)
    {
        throw new NotImplementedException();
    }

    public FoodJournalNoteDTO? GetNoteForDate(DateTime date)
    {
        FoodJournalNoteDTO? note = null;
        
        using (var connection = sqliteConnectionFactory.CreateOpenConnection())
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT id, date, content FROM food_journal_notes WHERE date(date) = date('{date.ToString("yyyy-MM-dd")}');";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long noteId = reader.GetInt64(0);
                        DateTime noteDate = reader.GetDateTime(1);
                        string noteContent = reader.GetString(2);
                        
                        note = new FoodJournalNoteDTO(noteId, noteDate, noteContent);
                    }
                }
            }
        }
        
        return note;
    }
}