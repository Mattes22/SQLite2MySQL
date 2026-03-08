using Avalonia;

namespace Sqlite2MySql;

internal static class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        // Bez argumentů nebo s --gui spustit přímo GUI
        if (args.Length == 0)
        {
            return BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        var options = Cli.Parse(args);
        if (options.Error != null)
        {
            Console.Error.WriteLine(options.Error);
            return 1;
        }

        if (options.Gui)
        {
            return BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        return RunCli(options);
    }

    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();

    private static int RunCli(CliOptions options)
    {

        if (string.IsNullOrWhiteSpace(options.SQLitePath))
        {
            Console.Error.WriteLine("Please provide a SQLite database path.");
            return 1;
        }

        if (options.Apply && string.IsNullOrWhiteSpace(options.Database))
        {
            Console.Error.WriteLine("When using --apply please provide --database so the import targets a schema.");
            return 1;
        }

        var tempOutput = string.Empty;
        var targetOutput = options.OutputPath;
        if (options.Apply && string.IsNullOrWhiteSpace(targetOutput))
        {
            tempOutput = Path.Combine(Path.GetTempPath(), $"sqlite2mysql_{Guid.NewGuid():N}.sql");
            targetOutput = tempOutput;
        }

        var password = options.MySqlPassword;
        if (options.Apply && options.MySqlPasswordPrompt && string.IsNullOrEmpty(password))
        {
            password = PasswordPrompt.ReadHidden("MySQL password: ");
        }

        var convertOptions = new ConvertOptions
        {
            SQLitePath = options.SQLitePath!,
            OutputPath = targetOutput,
            IncludeData = !options.SchemaOnly,
            BatchSize = options.BatchSize,
            Database = options.Database
        };

        Converter.ConvertDatabase(convertOptions);

        if (options.Apply)
        {
            MySqlClient.ApplyDump(
                targetOutput!,
                options.MySqlBin,
                options.MySqlHost,
                options.MySqlPort,
                options.MySqlUser,
                password,
                options.Database
            );
        }

        if (!string.IsNullOrWhiteSpace(tempOutput) && File.Exists(tempOutput))
        {
            try
            {
                File.Delete(tempOutput);
            }
            catch
            {
                // ignore
            }
        }

        return 0;
    }
}
