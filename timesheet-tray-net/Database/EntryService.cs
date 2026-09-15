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
    
    public async Task Create()
    {
        var user = new TimeEntry
        {
            StartDate = DateTime.UtcNow
        };
        await repository.Create(user);
    }

    public async Task Update(int id)
    {
        var entry = await repository.GetById(id);
        if (entry == null)
            throw new KeyNotFoundException("User not found");

        entry.FinishDate = DateTime.UtcNow;
        await repository.Update(entry);
    }
}