using System.Diagnostics;

namespace Sqlite2MySql;

internal static class MySqlClient
{
    public static void ApplyDump(string sqlPath, string mysqlBin, string host, int port, string user, string? password, string? database)
    {
        var args = new List<string>
        {
            "--protocol=tcp",
            "--host", host,
            "--user", user,
            "--port", port.ToString(),
            "--default-character-set=utf8mb4",
            "--comments",
            "--connect-timeout=5",
            $"--password={password ?? string.Empty}"
        };

        if (!string.IsNullOrWhiteSpace(database))
        {
            args.AddRange(new[] { "--database", database });
        }

        var psi = new ProcessStartInfo
        {
            FileName = mysqlBin,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = false,
            RedirectStandardError = false
        };
        foreach (var arg in args)
        {
            psi.ArgumentList.Add(arg);
        }
        if (!string.IsNullOrWhiteSpace(password))
        {
            psi.Environment["MYSQL_PWD"] = password;
        }

        using var process = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start mysql client.");
        using var input = process.StandardInput;
        using var file = File.OpenRead(sqlPath);
        file.CopyTo(input.BaseStream);
        input.Flush();
        input.Close();

        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"MySQL client failed with exit code {process.ExitCode}.");
        }
    }

    public static void TestConnection(string mysqlBin, string host, int port, string user, string? password, string? database)
    {
        var args = new List<string>
        {
            "--protocol=tcp",
            "--host", host,
            "--user", user,
            "--port", port.ToString(),
            "--default-character-set=utf8mb4",
            "--comments",
            "--connect-timeout=5",
            "-e", "SELECT 1;",
            $"--password={password ?? string.Empty}"
        };

        if (!string.IsNullOrWhiteSpace(database))
        {
            args.AddRange(new[] { "--database", database });
        }

        var psi = new ProcessStartInfo
        {
            FileName = mysqlBin,
            UseShellExecute = false,
            RedirectStandardInput = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        foreach (var arg in args)
        {
            psi.ArgumentList.Add(arg);
        }
        if (!string.IsNullOrWhiteSpace(password))
        {
            psi.Environment["MYSQL_PWD"] = password;
        }

        using var process = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start mysql client.");
        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Connection test failed with exit code {process.ExitCode}.");
        }
    }
}
