using FreelanceManager.Core.Models;
namespace FreelanceManager.Core.interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<List<Project>> GetAllByUserIdAsync(string userId, int? clientId, string? status, string? billingType);
        Task<Project?> GetProjectWithTimeEntriesAsync(int id);
    }
}