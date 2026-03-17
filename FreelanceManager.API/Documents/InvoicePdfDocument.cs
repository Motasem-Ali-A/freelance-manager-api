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
        private void ComposeHeader(IContainer container) { }
        private void ComposeContent(IContainer container) { }
        private void ComposeFooter(IContainer container) { }
    }
}