namespace Sqlite2MySql;

internal sealed class CliOptions
{
    public string? SQLitePath { get; set; }
    public string? OutputPath { get; set; }
    public bool SchemaOnly { get; set; }
    public int BatchSize { get; set; } = 500;
    public bool Gui { get; set; }
    public string? Database { get; set; }
    public bool Apply { get; set; }
    public string MySqlHost { get; set; } = "localhost";
    public int MySqlPort { get; set; } = 3306;
    public string MySqlUser { get; set; } = "root";
    public string? MySqlPassword { get; set; }
    public bool MySqlPasswordPrompt { get; set; }
    public string MySqlBin { get; set; } = "mysql";
    public string? Error { get; set; }
}

internal static class Cli
{
    public static CliOptions Parse(string[] args)
    {
        var options = new CliOptions();
        int i = 0;
        while (i < args.Length)
        {
            var arg = args[i];
            if (!arg.StartsWith("-", StringComparison.Ordinal))
            {
                options.SQLitePath ??= arg;
                i++;
                continue;
            }

            switch (arg)
            {
                case "-o":
                case "--output":
                    if (!TryReadValue(args, ref i, out var output))
                    {
                        options.Error = "Missing value for --output.";
                        return options;
                    }
                    options.OutputPath = output;
                    break;
                case "--schema-only":
                    options.SchemaOnly = true;
                    i++;
                    break;
                case "--batch-size":
                    if (!TryReadValue(args, ref i, out var batchValue) || !int.TryParse(batchValue, out var batch) || batch <= 0)
                    {
                        options.Error = "Invalid value for --batch-size.";
                        return options;
                    }
                    options.BatchSize = batch;
                    break;
                case "--gui":
                    options.Gui = true;
                    i++;
                    break;
                case "-d":
                case "--database":
                    if (!TryReadValue(args, ref i, out var database))
                    {
                        options.Error = "Missing value for --database.";
                        return options;
                    }
                    options.Database = database;
                    break;
                case "--apply":
                    options.Apply = true;
                    i++;
                    break;
                case "--mysql-host":
                    if (!TryReadValue(args, ref i, out var host))
                    {
                        options.Error = "Missing value for --mysql-host.";
                        return options;
                    }
                    options.MySqlHost = host;
                    break;
                case "--mysql-port":
                    if (!TryReadValue(args, ref i, out var portValue) || !int.TryParse(portValue, out var port) || port <= 0)
                    {
                        options.Error = "Invalid value for --mysql-port.";
                        return options;
                    }
                    options.MySqlPort = port;
                    break;
                case "--mysql-user":
                    if (!TryReadValue(args, ref i, out var user))
                    {
                        options.Error = "Missing value for --mysql-user.";
                        return options;
                    }
                    options.MySqlUser = user;
                    break;
                case "--mysql-password":
                    if (!TryReadValue(args, ref i, out var password))
                    {
                        options.Error = "Missing value for --mysql-password.";
                        return options;
                    }
                    options.MySqlPassword = password;
                    break;
                case "--mysql-password-prompt":
                    options.MySqlPasswordPrompt = true;
                    i++;
                    break;
                case "--mysql-bin":
                    if (!TryReadValue(args, ref i, out var bin))
                    {
                        options.Error = "Missing value for --mysql-bin.";
                        return options;
                    }
                    options.MySqlBin = bin;
                    break;
                default:
                    options.Error = $"Unknown option: {arg}";
                    return options;
            }
        }

        return options;
    }

    private static bool TryReadValue(string[] args, ref int index, out string value)
    {
        if (index + 1 >= args.Length)
        {
            value = string.Empty;
            return false;
        }
        value = args[index + 1];
        index += 2;
        return true;
    }
}
