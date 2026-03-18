using FreelanceManager.Core.DTOs;
using FreelanceManager.Core.Models;
namespace FreelanceManager.Core.interfaces
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<PageResult<Invoice>> GetAllByUserIdAsync(string userId, DateTime? from, DateTime? to, string? status, int page = 1, int pageSize = 10);
        Task<Invoice?> GetInvoiceWithItemsAsync(int id);
    }
}