using FreelanceManager.Core.Models;
namespace FreelanceManager.Core.interfaces
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<List<Invoice>> GetAllByClientIdAsync(int clientId);
    }
}