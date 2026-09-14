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
        
        // create database tables if they don't exist
        using var connection = CreateConnection();
        
        var sql = """
                      CREATE TABLE IF NOT EXISTS 
                      TimeEntries (
                          Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          EntryDate TEXT NOT NULL,
                          EntryType INTEGER NOT NULL
                      );
                  """;
        
        await connection.ExecuteAsync(sql);
    }
}