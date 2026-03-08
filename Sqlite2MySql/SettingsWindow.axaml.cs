using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace Sqlite2MySql;

internal sealed partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        DataContext = Application.Current?.Resources["AppStrings"];
        LoadCurrent();
    }

    private void LoadCurrent()
    {
        var s = Strings.Instance!;
        ThemeCombo!.ItemsSource = new List<string> { s.ThemeSystem, s.ThemeLight, s.ThemeDark };
        ThemeCombo.SelectedIndex = AppSettings.Theme == ThemeVariant.Light ? 1
            : AppSettings.Theme == ThemeVariant.Dark ? 2
            : 0;

        LanguageCombo!.ItemsSource = new List<string> { s.LangCs, s.LangEn, s.LangDe, s.LangPl };
        LanguageCombo.SelectedIndex = AppSettings.Language switch { "en" => 1, "de" => 2, "pl" => 3, _ => 0 };
    }

    private void OnOk(object? sender, RoutedEventArgs e)
    {
        var themeIndex = ThemeCombo!.SelectedIndex;
        AppSettings.Theme = themeIndex == 1 ? ThemeVariant.Light
            : themeIndex == 2 ? ThemeVariant.Dark
            : ThemeVariant.Default;

        var newLang = LanguageCombo!.SelectedIndex switch { 1 => "en", 2 => "de", 3 => "pl", _ => "cs" };
        AppSettings.Language = newLang;
        AppSettings.Save();

        Strings.Instance?.SetLanguage(newLang);

        if (Application.Current != null)
            Application.Current.RequestedThemeVariant = AppSettings.Theme;

        Close(true);
    }

    private void OnCancel(object? sender, RoutedEventArgs e) => Close(false);
}
