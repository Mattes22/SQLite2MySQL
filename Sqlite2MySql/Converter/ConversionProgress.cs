namespace Sqlite2MySql;

internal sealed class ConversionProgress
{
    public int CurrentTable { get; set; }
    public int TotalTables { get; set; }
    public string TableName { get; set; } = "";
}

internal sealed class ConversionResult
{
    public int TablesCount { get; set; }
    public long RowsCount { get; set; }
}
