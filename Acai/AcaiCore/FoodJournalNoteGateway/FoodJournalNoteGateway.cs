using Microsoft.Data.Sqlite;

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
        long createdNoteId = -1;
        DateTime createdNoteDate = DateTime.MinValue;
        string createdNoteContent = string.Empty;

        using (var connection = sqliteConnectionFactory.CreateOpenConnection())
        {
            using (var checkExistingNoteCommand = connection.CreateCommand())
            {
                checkExistingNoteCommand.CommandText = "SELECT id FROM food_journal_notes WHERE date = @noteDate LIMIT 1;";

                var noteDateParameter = new SqliteParameter("@noteDate", date);
                noteDateParameter.Value = date.ToString("yyyy-MM-dd");
                checkExistingNoteCommand.Parameters.Add(noteDateParameter);
                
                var existingNoteId = checkExistingNoteCommand.ExecuteScalar();
                if (existingNoteId != null)
                {
                    createdNoteId = (long)existingNoteId;
                }
            }

            var noteAlreadyExistsForDate = createdNoteId != -1;

            if (!noteAlreadyExistsForDate)
            {
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = "INSERT INTO food_journal_notes (date, content) VALUES (@noteDate, @noteContent) RETURNING id;";
                
                    var noteContentParameter = new SqliteParameter("@noteContent",SqliteType.Text);
                    noteContentParameter.Value = content;
                    
                    var noteDateParameter = new SqliteParameter("@noteDate",SqliteType.Text);
                    noteDateParameter.Value = date.ToString("yyyy-MM-dd");
                
                    insertCommand.Parameters.AddRange(new List<SqliteParameter>()
                    {
                        noteContentParameter,
                        noteDateParameter
                    });
                    insertCommand.Prepare();
                
                    createdNoteId = (long)insertCommand.ExecuteScalar();
                }
            }
            else
            {
                using (var updateCommand = connection.CreateCommand())
                {
                    updateCommand.CommandText = "UPDATE food_journal_notes SET (content) = (@noteContent) WHERE id = @noteId;";
                    
                    var noteIdParameter = new SqliteParameter("@noteId",SqliteType.Real);
                    noteIdParameter.Value = createdNoteId;
                    
                    var noteContentParameter = new SqliteParameter("@noteContent",SqliteType.Text);
                    noteContentParameter.Value = content;
                    
                    updateCommand.Parameters.AddRange(new List<SqliteParameter>()
                    {
                        noteIdParameter,
                        noteContentParameter
                    });
                    updateCommand.Prepare();
                    updateCommand.ExecuteNonQuery();
                }
            }

            using (var selectNewNoteCommand = connection.CreateCommand())
            {
                selectNewNoteCommand.CommandText = $"SELECT date, content FROM food_journal_notes WHERE id = {createdNoteId} LIMIT 1;";

                using (var reader = selectNewNoteCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        createdNoteDate = reader.GetDateTime(0);
                        createdNoteContent = reader.GetString(1);
                    }
                }
            }
        }

        return new FoodJournalNoteDTO(1,createdNoteDate,createdNoteContent);
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