using System.IO;
using System.Text.Json;
using Avalonia.Styling;

namespace Sqlite2MySql;

internal static class AppSettings
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private static string ConfigPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Sqlite2MySql", "settings.json");

    public static ThemeVariant Theme { get; set; } = ThemeVariant.Default;
    public static string Language { get; set; } = "cs";

    public static void Load()
    {
        try
        {
            var path = ConfigPath;
            if (!File.Exists(path)) return;
            var json = File.ReadAllText(path);
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (root.TryGetProperty("theme", out var themeEl))
            {
                var theme = themeEl.GetString();
                Theme = theme == "Light" ? ThemeVariant.Light
                    : theme == "Dark" ? ThemeVariant.Dark
                    : ThemeVariant.Default;
            }
            if (root.TryGetProperty("language", out var langEl))
            {
                var lang = langEl.GetString();
                if (lang is "cs" or "en" or "de" or "pl") Language = lang;
            }
        }
        catch
        {
            // ignorovat poškozený config
        }
    }

    public static void Save()
    {
        try
        {
            var dir = Path.GetDirectoryName(ConfigPath)!;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            var themeStr = Theme == ThemeVariant.Light ? "Light"
                : Theme == ThemeVariant.Dark ? "Dark"
                : "Default";
            var obj = new { theme = themeStr, language = Language };
            var json = JsonSerializer.Serialize(obj, JsonOptions);
            File.WriteAllText(ConfigPath, json);
        }
        catch
        {
            // ignorovat
        }
    }
}
