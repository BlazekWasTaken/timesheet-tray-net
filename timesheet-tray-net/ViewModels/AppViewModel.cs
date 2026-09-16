using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using timesheet_tray_net.Database;
using timesheet_tray_net.Excel;

namespace timesheet_tray_net.ViewModels;

public partial class AppViewModel : ViewModelBase
{
    public event EventHandler Loaded;
    private readonly ILogger<AppViewModel> _logger;
    private readonly EntryService _entryService;
    private readonly Window _window;
    
    [ObservableProperty] public partial string StartText { get; set; } = "start working";
    [ObservableProperty] public partial string StopText { get; set; } = "stop working";
    [ObservableProperty] public partial string ExportText { get; set; } = "export";
    [ObservableProperty] public partial string QuitText { get; set; } = "quit";

    [ObservableProperty] public partial bool IsStarted { get; set; } = false;

    public AppViewModel(ILogger<AppViewModel> logger, EntryService service, Window window)
    {
        _logger = logger;
        _entryService = service;
        _window = window;
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
        var finishDate = DateTime.UtcNow;
        var length = finishDate.Subtract(last.StartDate).TotalMinutes;
        if (length >= 5)
            await _entryService.Update(last.Id, finishDate);
        else
            await _entryService.Delete(last.Id);
        
        IsStarted = false;
    }

    [RelayCommand]
    private async Task Export()
    {
        var fileType = new FilePickerFileType("spreadsheet file") { Patterns = ["*.xlsx"] };
        var topLevel = TopLevel.GetTopLevel(_window);
        if (topLevel is null) return;
        var storageProvider = topLevel.StorageProvider;
        var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save timesheet",
            SuggestedFileName = "timesheet-export",
            SuggestedFileType = fileType,
            FileTypeChoices = [ fileType ],
            DefaultExtension = Path.GetExtension(fileType.Patterns![0])
        });
        if (file is null) return;
        
        var entries = await _entryService.GetAll();
        await using var stream = await file.OpenWriteAsync();
        var saveSuccess = ExcelService.SaveEntries([.. entries], stream);
        _logger.LogInformation("Export success: {success}", saveSuccess);
        if (!saveSuccess) return;
        
        var launcher = topLevel.Launcher;
        var launchSuccess = await launcher.LaunchFileAsync(file);
        _logger.LogInformation("File launch success: {LaunchSuccess}", launchSuccess);
    }

    [RelayCommand]
    private static void Quit() => Environment.Exit(0);
}