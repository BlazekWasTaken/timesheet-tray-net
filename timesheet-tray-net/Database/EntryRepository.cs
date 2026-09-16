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
                      INSERT INTO TimeEntries (StartDate)
                      VALUES (@StartDate)
                  """;
        await connection.ExecuteAsync(sql, timeEntry);
    }
    
    public async Task Update(TimeEntry timeEntry)
    {
        using var connection = context.CreateConnection();
        var sql = """
                      UPDATE TimeEntries
                      SET StartDate = @StartDate,
                          FinishDate = @FinishDate
                      WHERE Id = @Id
                  """;
        await connection.ExecuteAsync(sql, timeEntry);
    }
    
    public async Task Delete(int id)
    {
        using var connection = context.CreateConnection();
        var sql = """
                      DELETE FROM TimeEntries
                      WHERE Id = @id
                  """;
        await connection.ExecuteAsync(sql, new { id });
    }
}