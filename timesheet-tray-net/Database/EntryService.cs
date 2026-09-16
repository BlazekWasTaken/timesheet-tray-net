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
    
    public async Task<TimeEntry> Create()
    {
        var entry = new TimeEntry
        {
            StartDate = DateTime.UtcNow
        };
        await repository.Create(entry);
        return entry;
    }

    public async Task<TimeEntry> Update(int id, DateTime finishDate)
    {
        var entry = await repository.GetById(id);
        if (entry == null)
            throw new KeyNotFoundException("User not found");

        entry.FinishDate = finishDate;
        await repository.Update(entry);
        return entry;
    }
    
    public async Task Delete(int id)
    {
        await repository.Delete(id);
    }
}