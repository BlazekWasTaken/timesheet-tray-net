using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace timesheet_tray_net.Converters;

public class TimeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DateTime time) 
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        var difference = DateTime.UtcNow.Subtract(time);
        return difference.TotalDays > 1 ? 
            $"{difference.TotalDays} days" : 
            $"{difference.Hours:00}:{difference.Minutes:00}:{difference.Seconds:00}";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}