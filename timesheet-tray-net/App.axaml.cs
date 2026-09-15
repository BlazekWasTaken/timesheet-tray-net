using Avalonia;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using timesheet_tray_net.Database;
using timesheet_tray_net.Excel;
using timesheet_tray_net.ViewModels;

namespace timesheet_tray_net;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        var serilogLogger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(new CompactJsonFormatter(),"logs/log.json", rollingInterval: RollingInterval.Day)
            .WriteTo.Console()
            .CreateLogger();
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(serilogLogger);
        });
        
        serviceCollection.AddSingleton<DataContext>();
        serviceCollection.AddScoped<EntryRepository>();
        serviceCollection.AddScoped<EntryService>();
        
        serviceCollection.AddScoped<ExcelService>();
        
        serviceCollection.AddScoped<AppViewModel>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            context.Init().GetAwaiter().GetResult();
        }
        
        DataContext = serviceProvider.GetRequiredService<AppViewModel>();
    }
}