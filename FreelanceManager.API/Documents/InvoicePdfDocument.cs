using FreelanceManager.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace FreelanceManager.API.Documents
{
    public class InvoicePdfDocument : IDocument
    {
        private readonly Invoice _invoice;
        private readonly AppUser _user;

        public InvoicePdfDocument(Invoice invoice, AppUser user)
        {
            _invoice = invoice;
            _user = user;
        }
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }
        private void ComposeHeader(IContainer container)
        {
            container.Background("#2C3E50").Padding(20).Row(row =>
            {
                //Business info
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text(_user.BusinessName).FontSize(20).Bold().FontColor("#FFFFFF");
                    column.Item().Text(_user.Email ?? "").FontColor("#FFFFFF");
                });

                //Invoice details
                row.ConstantItem(150).Column(column =>
                {
                    column.Item().Text("INVOICE").FontSize(20).Bold().FontColor("#FFFFFF").AlignRight();
                    column.Item().Text(_invoice.InvoiceNumber).FontColor("#FFFFFF").AlignRight();
                    column.Item().Text($"Issued: {_invoice.IssueDate:dd/MM/yyyy}").FontColor("#FFFFFF").AlignRight();
                    column.Item().Text($"Due: {_invoice.DueDate:dd/MM/yyyy}").FontColor("#FFFFFF").AlignRight();
                });
            });
        }
        private void ComposeContent(IContainer container)
        {
            container.Row(row =>
            {
                //Bill To section
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Bill To:").FontSize(14).Bold();
                    column.Item().Text(_invoice.Client.Name);
                    column.Item().Text(_invoice.Client.Email ?? "");

                    column.Item().PaddingTop(20);
                    // Items table
                    column.Item().Element(ComposeTable);

                    column.Item().PaddingTop(10);
                    // Totals
                    column.Item().Element(ComposeTotals);
                });

            });
        }

        private void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3); // Description
                    columns.RelativeColumn(1); // Quantity
                    columns.RelativeColumn(1); // Unit Price
                    columns.RelativeColumn(1); // Total
                });
                table.Header(header =>
                {
                    header.Cell().Background("#2C3E50").Padding(5).Text("Description").Bold().FontColor("#FFFFFF");
                    header.Cell().Background("#2C3E50").Padding(5).Text("Qty").Bold().FontColor("#FFFFFF");
                    header.Cell().Background("#2C3E50").Padding(5).Text("Unit Price").Bold().FontColor("#FFFFFF");
                    header.Cell().Background("#2C3E50").Padding(5).Text("Total").Bold().FontColor("#FFFFFF");
                });

                foreach (var item in _invoice.InvoiceItems)
                {
                    table.Cell().BorderBottom(1).BorderColor("#DDDDDD").Padding(5).Text(item.Description);
                    table.Cell().BorderBottom(1).BorderColor("#DDDDDD").Padding(5).Text(item.Quantity.ToString());
                    table.Cell().BorderBottom(1).BorderColor("#DDDDDD").Padding(5).Text($"${item.UnitPrice}");
                    table.Cell().BorderBottom(1).BorderColor("#DDDDDD").Padding(5).Text($"${item.Total}");
                }
            });
        }

        private void ComposeTotals(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text($"Subtotal: ${_invoice.Subtotal}").AlignRight();
                column.Item().Text($"Tax ({_invoice.TaxRate * 100}%): ${_invoice.TaxAmount}").AlignRight();
                column.Item().Text($"Total: ${_invoice.TotalAmount}").Bold().FontSize(14).AlignRight();
            });
        }
        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.Span("Page ");
                text.CurrentPageNumber();
                text.Span(" of ");
                text.TotalPages();
            });
        }
    }
}