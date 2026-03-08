using Avalonia;

namespace Sqlite2MySql;

internal sealed class App : Application
{
    public override void Initialize()
    {
        Avalonia.Themes.Fluent.FluentTheme fluent = new();
        Styles.Add(fluent);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        AppSettings.Load();
        RequestedThemeVariant = AppSettings.Theme;
        Resources["AppStrings"] = new Strings();

        if (ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
