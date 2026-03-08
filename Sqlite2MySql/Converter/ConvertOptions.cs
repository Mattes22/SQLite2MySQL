namespace Sqlite2MySql;

internal sealed class ConvertOptions
{
    public required string SQLitePath { get; set; }
    public string? OutputPath { get; set; }
    public bool IncludeData { get; set; } = true;
    public int BatchSize { get; set; } = 500;
    public string? Database { get; set; }
    public IProgress<ConversionProgress>? Progress { get; set; }
}
