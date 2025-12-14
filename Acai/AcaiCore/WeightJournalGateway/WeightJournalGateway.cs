using Microsoft.Data.Sqlite;

namespace AcaiCore;

public class WeightJournalGateway : IWeightJournalGateway
{
    private ISqliteConnectionFactory _sqliteConnectionFactory;

    public WeightJournalGateway(ISqliteConnectionFactory sqliteConnectionFactory)
    {
        _sqliteConnectionFactory = sqliteConnectionFactory;
    }
    
    public WeightJournalEntryDTO CreateNewWeighIn(DateTime creationDate, float canonicalPounds, float? bodyFatPercentage, string? note)
    {
        long createdWeighInId = -1;
        DateTime createdWeighInDate = DateTime.MinValue;
        float createdCanonicalPounds = 0;
        float? createdBodyFatPercentage = null;
        string? createdNote = null;

        using (var connection = _sqliteConnectionFactory.CreateOpenConnection())
        {
            using (var createWeighInCommand = connection.CreateCommand())
            {
                createWeighInCommand.CommandText = "INSERT INTO weigh_in_entries (date, canonical_lbs, body_fat_percentage, note) VALUES " +
                                                   "(@weighInDate, @weighInPounds, @weighInBodyFat, @weighInNote) " +
                                                   "RETURNING id;";
                
                var dateParameter = new SqliteParameter("@weighInDate", creationDate.ToString("yyyy-MM-dd HH:mm:ss"));
                var poundsParameter = new SqliteParameter("@weighInPounds", canonicalPounds);
                var bodyFatParameter = new SqliteParameter("@weighInBodyFat", bodyFatPercentage != null ? bodyFatPercentage : DBNull.Value);
                var noteParameter = new SqliteParameter("@weighInNote", note != null ? note : DBNull.Value);
                createWeighInCommand.Parameters.AddRange(new List<SqliteParameter>()
                {
                    dateParameter,
                    poundsParameter,
                    bodyFatParameter,
                    noteParameter
                });
                createWeighInCommand.Prepare();
                
                createdWeighInId = (long)createWeighInCommand.ExecuteScalar();
            }

            using (var selectCreatedWeighInCommand = connection.CreateCommand())
            {
                selectCreatedWeighInCommand.CommandText = "SELECT date, canonical_lbs, body_fat_percentage, note " +
                                                          $"FROM weigh_in_entries WHERE id = {createdWeighInId} " +
                                                          "LIMIT 1;";

                using (var reader = selectCreatedWeighInCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        createdWeighInDate = reader.GetDateTime(0);
                        createdCanonicalPounds = reader.GetFloat(1);
                        createdBodyFatPercentage = reader.IsDBNull(2) ? null : reader.GetFloat(2);
                        createdNote = reader.IsDBNull(3) ? null : reader.GetString(3);
                    }
                }
            }
        }
        
        return new WeightJournalEntryDTO(createdWeighInId, createdWeighInDate, createdCanonicalPounds, createdBodyFatPercentage, createdNote);
    }

    public WeightJournalEntryDTO UpdateExistingWeighIn(long id, DateTime creationDate, float canonicalPounds, float? bodyFatPercentage, string? note)
    {
        throw new NotImplementedException();
    }

    public WeightJournalEntryDTO DeleteWeighIn(long id)
    {
        throw new NotImplementedException();
    }

    public List<WeightJournalEntryDTO> GetAllWeighIns()
    {
        var entries = new List<WeightJournalEntryDTO>();
        
        using (var connection = _sqliteConnectionFactory.CreateOpenConnection())
        {
            using (var retrieveAllWeighInsCommand = connection.CreateCommand())
            {
                retrieveAllWeighInsCommand.CommandText = "SELECT id, date, canonical_lbs, body_fat_percentage, note FROM weigh_in_entries;";

                using (var reader = retrieveAllWeighInsCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long id =  reader.GetInt64(0);
                        DateTime creationDate = reader.GetDateTime(1);
                        float canonicalPounds = reader.GetFloat(2);
                        float? bodyFatPercentage =  reader.IsDBNull(3) ? null : reader.GetFloat(3);
                        string? note =  reader.IsDBNull(4) ? null : reader.GetString(4);
                        
                        entries.Add(new WeightJournalEntryDTO(id, creationDate, canonicalPounds, bodyFatPercentage, note));
                    }
                }
            }
        }

        return entries;
    }
}