using FreelanceManager.Core.Models;
namespace FreelanceManager.Core.interfaces
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<List<Invoice>> GetAllByUserIdAsync(string userId, DateTime? from, DateTime? to, string? status);
    }
}