using FreelanceManager.Core.interfaces;
using FreelanceManager.Core.Models;
using FreelanceManager.Core.Enums;
namespace FreelanceManager.API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public void ApplyOverdueStatus(List<Invoice> invoices)
        {
            foreach (var invoice in invoices)
            {
                if (DateTime .UtcNow> invoice.DueDate && invoice.Status == InvoiceStatus.Sent)
                {
                    invoice.Status = InvoiceStatus.Overdue;
                }
            }
        }

        public void CalculateTotals(Invoice invoice)
        {
           invoice.Subtotal = invoice.InvoiceItems.Sum(item => item.Quantity * item.UnitPrice);
           invoice.TaxAmount = invoice.Subtotal*invoice.TaxRate;
           invoice.TotalAmount = invoice.Subtotal+ invoice.TaxAmount;
        }

        public async Task<string> GenerateInvoiceNumberAsync(string userId)
        {
            var result = (await _invoiceRepository.GetAllByUserIdAsync(userId)).Count +1;
            return($"INV-{result:D4}");
        }
    }
}