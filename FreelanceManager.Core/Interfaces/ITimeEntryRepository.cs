using FreelanceManager.Core.Models;
namespace FreelanceManager.Core.interfaces
{
    public interface ITimeEntryRepository : IRepository<TimeEntry>
    {
        Task<List<TimeEntry>> GetAllByProjectIdAsync(int projectId);
        Task<List<TimeEntry>> GetAllByUserIdAsync(string userId);
    }
}