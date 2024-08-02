using System.Collections.Generic;

namespace Nop.Core.Domain.Reporting;

#nullable enable

public class Table
{
    public List<string> Headers { get; set; } = new();
    public List<List<string>> Rows { get; set; } = new();
}

public class StyledCell
{
    public string Value { get; set; } = string.Empty;
    public string Style { get; set; } = string.Empty;

    public StyledCell(string value, string style = "")
    {
        Value = value;
        Style = style;
    }

    public StyledCell() { }
}

public class StyledRow
{
    public List<StyledCell> Cells { get; set; } = new();
    public string Style { get; set; } = string.Empty;

    public StyledRow(List<StyledCell> cells, string style = "")
    {
        Cells = cells;
        Style = style;
    }

    public StyledRow() { }
}

public class StyledTable
{
    public List<StyledCell> Headers { get; set; } = new();
    public List<StyledRow> Rows { get; set; } = new();
    public string TableStyle { get; set; } = string.Empty;
    public string HeaderRowStyle { get; set; } = string.Empty;
}

public class ReportData
{
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? FeatureValue { get; set; }
    public string? StringValue { get; set; }
    public Table? TableValue { get; set; }
    public StyledTable? StyledTableValue { get; set; }
}



