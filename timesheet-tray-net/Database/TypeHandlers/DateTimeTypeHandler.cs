using System;
using System.Data;
using Dapper;

namespace timesheet_tray_net.Database;

public class DateTimeTypeHandler : SqlMapper.TypeHandler<DateTime>
{
    public static DateTimeTypeHandler Default { get; } = new();
    
    public override void SetValue(IDbDataParameter parameter, DateTime value) => parameter.Value = value;
    
    public override DateTime Parse(object value) => 
        DateTime.SpecifyKind(
            DateTime.ParseExact(
                (string)value, 
                "yyyy-MM-dd HH:mm:ss.FFFFFFF", 
                System.Globalization.CultureInfo.InvariantCulture), 
            DateTimeKind.Utc);
}