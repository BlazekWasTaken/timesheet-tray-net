using System;

namespace timesheet_tray_net.Models;

public class TimeEntry
{
    public int Id { get; set; }
    public required DateTime StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
}