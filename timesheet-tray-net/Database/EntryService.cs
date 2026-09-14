using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using timesheet_tray_net.Models;

namespace timesheet_tray_net.Database;

public class EntryService(EntryRepository repository)
{
    public async Task<IEnumerable<TimeEntry>> GetAll()
    {
        return await repository.GetAll();
    }

    public async Task<TimeEntry?> GetById(int id)
    {
        return await repository.GetById(id);
    }
    
    public async Task Create(EntryType type)
    {
        var user = new TimeEntry
        {
            EntryDate = DateTime.UtcNow,
            EntryType = type
        };
        await repository.Create(user);
    }
}