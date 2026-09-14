using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using timesheet_tray_net.Models;

namespace timesheet_tray_net.Database;

public class EntryRepository(DataContext context)
{
    public async Task<IEnumerable<TimeEntry>> GetAll()
    {
        using var connection = context.CreateConnection();
        var sql = """
                      SELECT * FROM TimeEntries
                  """;
        return await connection.QueryAsync<TimeEntry>(sql);
    }
    
    public async Task<TimeEntry?> GetById(int id)
    {
        using var connection = context.CreateConnection();
        var sql = """
                      SELECT * FROM TimeEntries 
                      WHERE Id = @id
                  """;
        return await connection.QuerySingleOrDefaultAsync<TimeEntry>(sql, new { id });
    }
    
    public async Task Create(TimeEntry timeEntry)
    {
        using var connection = context.CreateConnection();
        var sql = """
                      INSERT INTO TimeEntry (EntryDate, EntryType)
                      VALUES (@EntryDate, @EntryType)
                  """;
        await connection.ExecuteAsync(sql, timeEntry);
    }
}