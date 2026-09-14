using System;
using System.Data;
using Dapper;
using timesheet_tray_net.Models;

namespace timesheet_tray_net.Database;

public class EntryTypeTypeHandler : SqlMapper.TypeHandler<EntryType>
{
    public override void SetValue(IDbDataParameter parameter, EntryType value) => 
        parameter.Value = value.ToString();
    
    public override EntryType Parse(object value) =>
        value is not string s ? EntryType.Default : Enum.Parse<EntryType>(s);
}