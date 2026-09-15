using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;

namespace timesheet_tray_net.Database;

public class DataContext
{
    public IDbConnection CreateConnection()
    {
        return new SqliteConnection("DataSource=LocalDatabase.db");
    }

    public async Task Init()
    {
        SqlMapper.AddTypeHandler(DateTimeTypeHandler.Default);
        
        using var connection = CreateConnection();
        
        var sql = """
                      CREATE TABLE IF NOT EXISTS 
                      TimeEntries (
                          Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          StartDate TEXT NOT NULL,
                          FinishDate TEXT
                      );
                  """;
        
        await connection.ExecuteAsync(sql);
    }
}