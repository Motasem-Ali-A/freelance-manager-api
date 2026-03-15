using FreelanceManager.Core.Models;

namespace FreelanceManager.Core.interfaces
{
    public interface IInvoiceService
    {
        Task<string> GenerateInvoiceNumberAsync(string userId);
        void  CalculateTotals(Invoice invoice);
        void ApplyOverdueStatus(List<Invoice> invoices);
    }
}
