using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using timesheet_tray_net.Database;
using timesheet_tray_net.Models;

namespace timesheet_tray_net.ViewModels;

public partial class AppViewModel : ViewModelBase
{
    public event EventHandler Loaded;
    private readonly EntryService _entryService;
    
    [ObservableProperty] public partial string StartText { get; set; } = "start working";
    [ObservableProperty] public partial string StopText { get; set; } = "stop working";
    [ObservableProperty] public partial string ExportText { get; set; } = "export";
    [ObservableProperty] public partial string QuitText { get; set; } = "quit";

    [ObservableProperty] public partial bool IsStarted { get; set; } = false;

    public AppViewModel(EntryService service)
    {
        _entryService = service;
        Loaded += async (_, _) => await Initialize();
        Loaded.Invoke(this, EventArgs.Empty);
    }

    private async Task Initialize()
    {
        var entries = await _entryService.GetAll();
        var last = entries.OrderBy(x => x.EntryDate).LastOrDefault();
        if (last == null) return;
        IsStarted = last.EntryType switch
        {
            EntryType.Start => true,
            EntryType.Stop => false,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    [RelayCommand]
    private async Task Start()
    {
        await _entryService.Create(EntryType.Start);
        IsStarted = true;
    }
    
    [RelayCommand]
    private async Task Stop()
    {
        await _entryService.Create(EntryType.Stop);
        IsStarted = false;
    }

    [RelayCommand]
    private async Task Export()
    {
        foreach (var entry in await _entryService.GetAll())
        {
            Console.WriteLine($"Time: {entry.EntryDate} - {entry.EntryType}");
        }
    }

    [RelayCommand]
    private static void Quit() => Environment.Exit(0);
}