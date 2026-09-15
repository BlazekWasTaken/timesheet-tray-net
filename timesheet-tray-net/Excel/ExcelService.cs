using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using timesheet_tray_net.Models;

namespace timesheet_tray_net.Excel;

public class ExcelService
{
    public void SaveEntries(List<TimeEntry> entries)
    {
        var years = entries.Select(x => x.StartDate.Year).Distinct().ToList();

        using var workbook = new XLWorkbook();
        
        foreach (var year in years)
        {
            var months = entries
                .Where(x => x.StartDate.Year == year)
                .Select(x => x.StartDate.Month).Distinct().ToList();
            foreach (var month in months)
            {
                var worksheet = workbook.Worksheets.Add("Sample Sheet");
                
                
            }
        }
    }
}