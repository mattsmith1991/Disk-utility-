using LibVLCSharp.Shared;
using ModernWpf;
using System.Windows;

namespace SynclerWindows
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Initialize LibVLC
            Core.Initialize();
            
            // Set Modern WPF theme
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            ThemeManager.Current.AccentColor = System.Windows.Media.Color.FromRgb(108, 92, 231);
            
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}