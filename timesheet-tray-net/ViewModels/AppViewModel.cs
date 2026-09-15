using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using timesheet_tray_net.Database;

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
        var last = entries.OrderBy(x => x.StartDate).LastOrDefault();
        if (last == null) return;
        IsStarted = last.FinishDate is null;
    }
    
    [RelayCommand]
    private async Task Start()
    {
        await _entryService.Create();
        IsStarted = true;
    }
    
    [RelayCommand]
    private async Task Stop()
    {
        var entries = await _entryService.GetAll();
        var last = entries.OrderBy(x => x.StartDate).LastOrDefault();
        if (last is null || last.FinishDate is not null) return;
        await _entryService.Update(last.Id);
        IsStarted = false;
    }

    [RelayCommand]
    private async Task Export()
    {
        foreach (var entry in await _entryService.GetAll())
        {
            Console.WriteLine($"Time: {entry.StartDate} - {entry.FinishDate}");
        }
    }

    [RelayCommand]
    private static void Quit() => Environment.Exit(0);
}