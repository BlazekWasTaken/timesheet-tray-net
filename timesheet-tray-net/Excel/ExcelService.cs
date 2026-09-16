using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using timesheet_tray_net.Models;

namespace timesheet_tray_net.Excel;

public class ExcelService
{
    public static bool SaveEntries(List<TimeEntry> entries, Stream fileStream)
    {
        entries.ForEach(x =>
        {
            x.StartDate = x.StartDate.ToLocalTime();
            x.FinishDate = x.FinishDate?.ToLocalTime();
        });
        
        var years = entries.Select(x => x.StartDate.Year).Distinct().ToList();

        using var workbook = new XLWorkbook();
        
        foreach (var year in years)
        {
            var months = entries
                .Where(x => x.StartDate.Year == year)
                .Select(x => x.StartDate.Month).Distinct().ToList();
            foreach (var month in months)
            {
                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                var worksheet = workbook.Worksheets.Add($"{year} {monthName}");

                var monthEntries = entries
                    .Where(x =>
                        x.StartDate.Year == year &&
                        x.StartDate.Month == month)
                    .Select(x => new
                    {
                        Day = x.StartDate.Day,
                        Start = x.StartDate.TimeOfDay,
                        Finish = x.FinishDate?.TimeOfDay,
                        Hours = Math.Round(x.FinishDate?.Subtract(x.StartDate).TotalHours ?? 0, 2)
                    });

                var table = worksheet.Cell(2, 2).InsertTable(monthEntries);
                table.ShowTotalsRow = true;

                table.Fields
                    .FirstOrDefault(x => x.Name == "Hours")?.TotalsRowFunction = XLTotalsRowFunction.Sum;
                
                worksheet.Columns().AdjustToContents();
            }
        }

        try
        {
            workbook.SaveAs(fileStream);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}