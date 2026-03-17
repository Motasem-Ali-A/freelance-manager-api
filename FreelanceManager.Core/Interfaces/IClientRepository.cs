using FreelanceManager.Core.DTOs;
using FreelanceManager.Core.Models;
namespace FreelanceManager.Core.interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client?> GetClientWithProjectsAsync(int id);
        Task<PageResult<Client>> GetAllByUserIdAsync(string userId, string? status = null, string? search = null, int page = 1, int pageSize = 10);
    }
}