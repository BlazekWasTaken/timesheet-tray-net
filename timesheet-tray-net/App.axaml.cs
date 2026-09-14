using Avalonia;
using Avalonia.Markup.Xaml;
using timesheet_tray_net.ViewModels;

namespace timesheet_tray_net;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new AppViewModel();
    }
}