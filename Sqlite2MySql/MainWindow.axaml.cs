using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

namespace Sqlite2MySql;

internal sealed partial class MainWindow : Window
{
    private bool _busy;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = Application.Current?.Resources["AppStrings"];
    }

    private void OnMenuOpenSqlite(object? sender, RoutedEventArgs e) => OnChooseSqlite(sender, e);

    private void OnMenuExit(object? sender, RoutedEventArgs e) => Close();

    private async void OnMenuAbout(object? sender, RoutedEventArgs e)
    {
        var s = Strings.Instance!;
        var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        var versionStr = version != null ? $"{version.Major}.{version.Minor}.{version.Build}" : "1.0";
        var author = "Matěj Zavadil";
        var contact = "info@zavadilmatej.cz";
        var license = "MIT License"; // např. MIT, GPL v3, Apache 2.0
        var copyrightYear = DateTime.Now.Year;
        var copyright = $"© {copyrightYear} {author}";
        var message = $"SQLite ➜ MySQL Converter\n\n{s.AboutDescription}\n\n{s.AboutVersionLabel}: {versionStr}\n{s.AboutAuthorLabel}: {author}\n{s.AboutContactLabel}: {contact}\n{s.AboutLicenseLabel}: {license}\n{s.AboutCopyrightLabel}: {copyright}";
        await ShowMessageAsync(message, s.AboutTitle, sizeToContent: true);
    }

    private async void OnMenuSettings(object? sender, RoutedEventArgs e)
    {
        var settings = new SettingsWindow();
        await settings.ShowDialog(this);
    }

    private async void OnChooseSqlite(object? sender, RoutedEventArgs e)
    {
        var s = Strings.Instance!;
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = s.DialogPickSqlite,
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType(s.FileTypeSqlite) { Patterns = new[] { "*.sqlite", "*.db" } },
                new FilePickerFileType(s.FileTypeAll) { Patterns = new[] { "*.*" } }
            }
        });

        if (files.Count >= 1 && files[0].TryGetLocalPath() is { } path)
        {
            SqlitePath!.Text = path;
            if (string.IsNullOrWhiteSpace(OutputPath!.Text))
            {
                OutputPath.Text = DefaultOutputPath(path);
            }
        }
    }

    private async void OnChooseOutput(object? sender, RoutedEventArgs e)
    {
        var s = Strings.Instance!;
        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = s.DialogSaveDump,
            DefaultExtension = "sql",
            FileTypeChoices = new[]
            {
                new FilePickerFileType(s.FileTypeSql) { Patterns = new[] { "*.sql" } },
                new FilePickerFileType(s.FileTypeAll) { Patterns = new[] { "*.*" } }
            }
        });

        if (file?.TryGetLocalPath() is { } path)
        {
            OutputPath!.Text = path;
        }
    }

    private async void OnChooseMysqlBin(object? sender, RoutedEventArgs e)
    {
        var s = Strings.Instance!;
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = s.DialogPickMysql,
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("mysql.exe") { Patterns = new[] { "mysql.exe", "mysql" } },
                new FilePickerFileType(s.FileTypeAll) { Patterns = new[] { "*.*" } }
            }
        });

        if (files.Count >= 1 && files[0].TryGetLocalPath() is { } path)
        {
            MysqlBinPath!.Text = path;
        }
    }

    private static string GetMysqlBin(string? path) => string.IsNullOrWhiteSpace(path) ? "mysql" : path.Trim();

    private static void EnsureMysqlBinExists(string mysqlBin)
    {
        if (mysqlBin == "mysql") return;
        if (!File.Exists(mysqlBin))
            throw new InvalidOperationException(string.Format(Strings.Instance?.Get("MsgMysqlNotFound") ?? "{0}", mysqlBin));
    }

    private async void OnTestConnection(object? sender, RoutedEventArgs e)
    {
        if (_busy) return;
        SetBusy(true);
        SetStatus("StatusTesting");

        var mysqlBin = GetMysqlBin(MysqlBinPath!.Text);
        var host = MysqlHost!.Text?.Trim() ?? "localhost";
        var port = (int)(MysqlPort!.Value ?? 3306);
        var user = MysqlUser!.Text?.Trim() ?? "root";
        var password = string.IsNullOrWhiteSpace(MysqlPassword!.Text) ? null : MysqlPassword.Text;
        var database = string.IsNullOrWhiteSpace(Database!.Text) ? null : Database.Text?.Trim();

        try
        {
            EnsureMysqlBinExists(mysqlBin);
            await Task.Run(() =>
            {
                MySqlClient.TestConnection(mysqlBin, host, port, user, password, database);
            });

            var s = Strings.Instance!;
            await ShowMessageAsync(s.MsgConnectionOk, s.MsgOk);
            SetStatus("StatusOk");
        }
        catch (Exception ex)
        {
            var s = Strings.Instance!;
            await ShowMessageAsync(string.Format(s.Get("MsgConnectionFailed"), ex.Message), s.MsgError, isError: true);
            SetStatus("StatusConnectionError");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void OnRunConversion(object? sender, RoutedEventArgs e)
    {
        if (_busy) return;

        var sqlitePath = SqlitePath!.Text?.Trim();
        var s = Strings.Instance!;
        if (string.IsNullOrWhiteSpace(sqlitePath))
        {
            await ShowMessageAsync(s.MsgSelectSqlite, s.MsgError, isError: true);
            return;
        }
        if (BatchSize!.Value is null or <= 0)
        {
            await ShowMessageAsync(s.MsgBatchSizeInvalid, s.MsgError, isError: true);
            return;
        }
        if (ApplyToMysql!.IsChecked == true && string.IsNullOrWhiteSpace(Database!.Text?.Trim()))
        {
            await ShowMessageAsync(s.MsgDatabaseRequired, s.MsgError, isError: true);
            return;
        }

        var output = OutputPath!.Text?.Trim();
        if (string.IsNullOrWhiteSpace(output))
        {
            output = DefaultOutputPath(sqlitePath);
            OutputPath.Text = output;
        }

        SetBusy(true);
        SetStatus("StatusConverting");

        var mysqlBin = GetMysqlBin(MysqlBinPath!.Text);
        var mysqlHost = MysqlHost!.Text?.Trim() ?? "localhost";
        var mysqlPort = (int)(MysqlPort!.Value ?? 3306);
        var mysqlUser = MysqlUser!.Text?.Trim() ?? "root";
        var mysqlPassword = string.IsNullOrWhiteSpace(MysqlPassword!.Text) ? null : MysqlPassword.Text;
        var mysqlDatabase = Database!.Text?.Trim();

        try
        {
            if (ApplyToMysql!.IsChecked == true)
                EnsureMysqlBinExists(mysqlBin);

            var includeData = IncludeData!.IsChecked != false;
            var batchSize = (int)(BatchSize.Value ?? 500);
            var database = IncludeDatabaseHeader!.IsChecked == true ? Database!.Text?.Trim() : null;
            var apply = ApplyToMysql!.IsChecked == true;

            ConversionProgressBar!.IsVisible = true;
            ConversionProgressBar.IsIndeterminate = true;

            var progress = new Progress<ConversionProgress>(p =>
            {
                Dispatcher.UIThread.Post(() =>
                {
                    ConversionProgressBar.IsIndeterminate = false;
                    ConversionProgressBar.Maximum = p.TotalTables;
                    ConversionProgressBar.Value = p.CurrentTable;
                    Strings.Instance?.SetStatusMessage(string.Format(s.StatusConvertingTable, p.CurrentTable, p.TotalTables, p.TableName));
                });
            });

            var convertOptions = new ConvertOptions
            {
                SQLitePath = sqlitePath,
                OutputPath = output,
                IncludeData = includeData,
                BatchSize = batchSize,
                Database = database,
                Progress = progress
            };

            var result = await Task.Run(() =>
            {
                var res = Converter.ConvertDatabase(convertOptions);
                if (apply)
                    MySqlClient.ApplyDump(output!, mysqlBin, mysqlHost, mysqlPort, mysqlUser, mysqlPassword, mysqlDatabase!);
                return res;
            });

            var summary = string.Format(s.MsgSummaryTablesRows, result.TablesCount, result.RowsCount);
            var msg = string.Format(s.MsgSavedTo, output) + " " + summary;
            if (apply) msg += s.MsgAndImported;
            await ShowMessageWithOpenFolderAsync(msg, s.MsgDone, output!);
            SetStatus("StatusDone");
        }
        catch (Exception ex)
        {
            await ShowMessageAsync(string.Format(s.Get("MsgConvertFailed"), ex.Message), s.MsgError, isError: true);
            SetStatus("StatusConvertError");
        }
        finally
        {
            SetBusy(false);
            ConversionProgressBar!.IsVisible = false;
        }
    }

    private void SetBusy(bool busy)
    {
        _busy = busy;
        ConvertButton!.IsEnabled = !busy;
        TestButton!.IsEnabled = !busy;
    }

    private void SetStatus(string statusKey)
    {
        Dispatcher.UIThread.Post(() => Strings.Instance?.SetCurrentStatus(statusKey));
    }

    private async Task ShowMessageAsync(string message, string title, bool isError = false, bool sizeToContent = false)
    {
        var okLabel = Strings.Instance?.MsgOk ?? "OK";
        var panel = new StackPanel { Margin = new Thickness(20), Spacing = 16 };
        panel.Children.Add(new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap, MaxWidth = 450 });
        var okButton = new Button { Content = okLabel, HorizontalAlignment = HorizontalAlignment.Right };
        var window = new Window
        {
            Title = title,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = panel,
            SizeToContent = sizeToContent ? SizeToContent.WidthAndHeight : SizeToContent.Manual,
            MinWidth = 280,
            MinHeight = 120
        };
        if (!sizeToContent)
        {
            window.Width = 420;
            window.Height = 180;
        }
        else
        {
            window.MaxWidth = 520;
        }
        okButton.Click += (_, _) => window.Close();
        panel.Children.Add(okButton);
        await window.ShowDialog(this);
    }

    private async Task ShowMessageWithOpenFolderAsync(string message, string title, string outputFilePath)
    {
        var s = Strings.Instance!;
        var panel = new StackPanel { Margin = new Thickness(20), Spacing = 16 };
        panel.Children.Add(new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap, MaxWidth = 450 });
        var buttonPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Spacing = 8 };
        var openFolderButton = new Button { Content = s.ButtonOpenFolder };
        var okButton = new Button { Content = s.MsgOk };
        var window = new Window
        {
            Title = title,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = panel,
            SizeToContent = SizeToContent.WidthAndHeight,
            MinWidth = 280,
            MaxWidth = 520
        };
        openFolderButton.Click += (_, _) =>
        {
            OpenOutputFolder(outputFilePath);
            window.Close();
        };
        okButton.Click += (_, _) => window.Close();
        buttonPanel.Children.Add(openFolderButton);
        buttonPanel.Children.Add(okButton);
        panel.Children.Add(buttonPanel);
        await window.ShowDialog(this);
    }

    private static void OpenOutputFolder(string outputFilePath)
    {
        try
        {
            var dir = Path.GetDirectoryName(outputFilePath);
            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir)) return;
            Process.Start(new ProcessStartInfo
            {
                FileName = dir,
                UseShellExecute = true
            });
        }
        catch
        {
            // ignorovat
        }
    }

    private static string DefaultOutputPath(string sqlitePath)
    {
        var ext = System.IO.Path.GetExtension(sqlitePath);
        if (string.IsNullOrWhiteSpace(ext))
        {
            return sqlitePath + ".sql";
        }
        return sqlitePath[..^ext.Length] + ".sql";
    }
}
