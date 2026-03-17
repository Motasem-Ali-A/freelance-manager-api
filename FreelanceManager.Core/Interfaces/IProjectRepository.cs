using FreelanceManager.Core.DTOs;
using FreelanceManager.Core.Models;
namespace FreelanceManager.Core.interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<PageResult<Project>> GetAllByUserIdAsync(string userId, int? clientId, string? status, string? billingType, int page, int pageSize);
        Task<Project?> GetProjectWithTimeEntriesAsync(int id);
    }
}