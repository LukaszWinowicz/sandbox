using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuilderPattern;


// Model danych
public class Sale
{
    public DateTime Date { get; set; }
    public string Product { get; set; }
    public decimal Amount { get; set; }
}

// Product - Produkt końcowy
public class SalesReport
{
    public string Title { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<Sale> Sales { get; set; } = new();
    public decimal Total => Sales.Sum(s => s.Amount);
    public List<string> Sections { get; set; } = new();
    public string Body => string.Join(Environment.NewLine + Environment.NewLine, Sections);
}


// Abstract Builder - Interfejs budowniczego
public interface ISalesReportBuilder
{
    void SetTitle(string title);
    void SetDateRange(DateTime from, DateTime to);
    void SetSales(List<Sale> sales);

    void BuildHeader();
    void BuildChartSection();
    void BuildTableSection();
    void BuildSummary();

    SalesReport GetReport();
}

// Concrete Builder A - konkretny budowniczy
public class TextSalesReportBuilder : ISalesReportBuilder
{
    private SalesReport _report = new();

    public void SetTitle(string title) => _report.Title = title;

    public void SetDateRange(DateTime from, DateTime to)
    {
        _report.From = from;
        _report.To = to;
    }

    public void SetSales(List<Sale> sales) => _report.Sales = sales;

    public void BuildHeader()
    {
        _report.Sections.Add($"RAPORT: {_report.Title}\nZakres: {_report.From:yyyy-MM-dd} - {_report.To:yyyy-MM-dd}");
    }

    public void BuildChartSection()
    {
        // Na potrzeby przykładu: tekstowy wykres słupkowy
        var lines = new List<string> { "WYKRES SPRZEDAŻY:" };
        foreach (var group in _report.Sales.GroupBy(s => s.Product))
        {
            var sum = group.Sum(s => s.Amount);
            var bar = new string('█', (int)(sum / 10)); // prosta wizualizacja
            lines.Add($"{group.Key,-10}: {bar} {sum:C}");
        }
        _report.Sections.Add(string.Join("\n", lines));
    }

    public void BuildTableSection()
    {
        var sb = new StringBuilder();
        sb.AppendLine("SZCZEGÓŁY SPRZEDAŻY:");
        sb.AppendLine($"{"Data",-12} | {"Produkt",-10} | {"Kwota",8}");
        sb.AppendLine(new string('-', 36));
        foreach (var sale in _report.Sales)
        {
            sb.AppendLine($"{sale.Date:yyyy-MM-dd} | {sale.Product,-10} | {sale.Amount,8:C}");
        }
        _report.Sections.Add(sb.ToString());
    }

    public void BuildSummary()
    {
        _report.Sections.Add($"PODSUMOWANIE:\nLiczba transakcji: {_report.Sales.Count}\nSuma sprzedaży: {_report.Total:C}");
    }

    public SalesReport GetReport() => _report;
}

// Director - Dyrektor budujący pełny raport
public class SalesReportDirector
{
    public void ConstructFull(ISalesReportBuilder builder, string title, DateTime from, DateTime to, List<Sale> sales)
    {
        builder.SetTitle(title);
        builder.SetDateRange(from, to);
        builder.SetSales(sales);

        builder.BuildHeader();
        builder.BuildChartSection();
        builder.BuildTableSection();
        builder.BuildSummary();
    }
}


// Concrete Builder B - konkretny budowniczy
public class MarkdownSalesReportBuilder : ISalesReportBuilder
{
    private SalesReport _report = new();

    public void SetTitle(string title) => _report.Title = title;

    public void SetDateRange(DateTime from, DateTime to)
    {
        _report.From = from;
        _report.To = to;
    }

    public void SetSales(List<Sale> sales) => _report.Sales = sales;

    public void BuildHeader()
    {
        _report.Sections.Add($"# {_report.Title}\n\n**Zakres dat:** {_report.From:yyyy-MM-dd} – {_report.To:yyyy-MM-dd}\n");
    }

    public void BuildChartSection()
    {
        var grouped = _report.Sales
            .GroupBy(s => s.Product)
            .Select(g => new { Product = g.Key, Total = g.Sum(s => s.Amount) })
            .ToList();

        var max = grouped.Max(g => g.Total);
        var chart = new StringBuilder("## Wykres sprzedaży (tekstowy)\n\n");

        foreach (var item in grouped)
        {
            var bar = new string('█', (int)(item.Total / max * 20));
            chart.AppendLine($"- **{item.Product}**: {bar} ({item.Total:C})");
        }

        _report.Sections.Add(chart.ToString());
    }

    public void BuildTableSection()
    {
        var sb = new StringBuilder();
        sb.AppendLine("## Szczegóły sprzedaży\n");
        sb.AppendLine("| Data | Produkt | Kwota |");
        sb.AppendLine("|------|---------|--------|");

        foreach (var sale in _report.Sales)
        {
            sb.AppendLine($"| {sale.Date:yyyy-MM-dd} | {sale.Product} | {sale.Amount:C} |");
        }

        _report.Sections.Add(sb.ToString());
    }

    public void BuildSummary()
    {
        _report.Sections.Add($@"## Podsumowanie

- Liczba transakcji: {_report.Sales.Count}
- Suma sprzedaży: {_report.Total:C}
");
    }

    public SalesReport GetReport() => _report;
}
