using System;

namespace timesheet_tray_net.Models;

public class TimeEntry
{
    public required DateTime Date { get; set; }
    public required EntryType EntryType { get; set; }
}