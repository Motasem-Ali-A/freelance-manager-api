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
            container.Row(row =>
            {
                //Business info
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text(_user.BusinessName).FontSize(20).Bold();
                    column.Item().Text(_user.Email);
                });

                //Invoice details
                row.ConstantItem(150).Column(column =>
                {
                    column.Item().Text("INVOICE").FontSize(20).Bold().AlignRight();
                    column.Item().Text(_invoice.InvoiceNumber).AlignRight();
                    column.Item().Text($"Issued: {_invoice.IssueDate:dd/MM/yyyy}").AlignRight();
                    column.Item().Text($"Due: {_invoice.DueDate:dd/MM/yyyy}").AlignRight();
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
                    column.Item().Text("Bill To: ").FontSize(20).Bold();
                    column.Item().Text(_invoice.Client.Name);
                    column.Item().Text(_invoice.Client.Email);

                    column.Item().PaddingTop(20);
                    // Items table
                    column.Item().Element(ComposeTable);
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
                    header.Cell().Text("Description").Bold();
                    header.Cell().Text("Qty").Bold();
                    header.Cell().Text("Unit Price").Bold();
                    header.Cell().Text("Total").Bold();
                });

                foreach (var item in _invoice.InvoiceItems)
                {
                    table.Cell().Text(item.Description);
                    table.Cell().Text(item.Quantity.ToString());
                    table.Cell().Text($"${item.UnitPrice}");
                    table.Cell().Text($"${item.Total}");
                }
            });
        }
        private void ComposeFooter(IContainer container) { }
    }
}