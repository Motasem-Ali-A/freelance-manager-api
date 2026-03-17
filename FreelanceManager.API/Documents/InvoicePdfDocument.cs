using FreelanceManager.Core.Models;
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
                //Business info
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Bill To: ").FontSize(20).Bold();
                    column.Item().Text(_invoice.Client.Name);
                    column.Item().Text(_invoice.Client.Email);
                });
            });
        }
        private void ComposeFooter(IContainer container) { }
    }
}