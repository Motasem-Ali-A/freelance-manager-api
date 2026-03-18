using FreelanceManager.Core.DTOs;
using FreelanceManager.Core.Models;
namespace FreelanceManager.Core.interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<PageResult<Project>> GetAllByUserIdAsync(string userId, int? clientId, string? status, string? billingType, int page = 1, int pageSize = 10);
        Task<Project?> GetProjectWithTimeEntriesAsync(int id);
    }
}