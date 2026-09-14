using System;
using System.Data;
using Dapper;

namespace timesheet_tray_net.Database;

public class DateTimeTypeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override void SetValue(IDbDataParameter parameter, DateTime value) => 
        parameter.Value = new DateTimeOffset(value).ToUnixTimeSeconds();
    
    public override DateTime Parse(object value) => 
        value is not int epoch ? DateTime.MinValue : DateTimeOffset.FromUnixTimeSeconds(epoch).DateTime;
}