using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace timesheet_tray_net.ViewModels;

public partial class AppViewModel : ViewModelBase
{
    private const string Start = "start working";
    private const string Stop = "stop working";

    [ObservableProperty] public partial bool IsStarted { get; set; } = false;
    [ObservableProperty] public partial string CurrentAction { get; set; } = Start;
    [ObservableProperty] public partial string QuitText { get; set; } = "quit";

    public AppViewModel()
    {
        // get last entry from database, fill variables
    }
    
    [RelayCommand]
    private void StartStop()
    {
        var time = DateTime.UtcNow;
        
    }

    [RelayCommand]
    private static void Quit() => Environment.Exit(0);
}