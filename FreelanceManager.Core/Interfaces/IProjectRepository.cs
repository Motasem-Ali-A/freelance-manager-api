using FreelanceManager.Core.Models;
namespace FreelanceManager.Core.interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<List<Project>> GetAllByClientIdAsync(int clientId);
        Task<List<Project>> GetAllByUserIdAsync(string userId);
    }
}