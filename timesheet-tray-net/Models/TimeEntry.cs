using System;

namespace timesheet_tray_net.Models;

public class TimeEntry
{
    public int Id { get; set; }
    public required DateTime EntryDate { get; set; }
    public required EntryType EntryType { get; set; }
}