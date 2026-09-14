using Avalonia;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
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
        
        // add database
        
        serviceCollection.AddScoped<AppViewModel>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        DataContext = serviceProvider.GetRequiredService<AppViewModel>();
    }
}