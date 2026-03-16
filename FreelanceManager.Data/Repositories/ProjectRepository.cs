using FreelanceManager.Core.Enums;
using FreelanceManager.Core.interfaces;
using FreelanceManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FreelanceManager.Data.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Project>> GetAllByUserIdAsync(string userId, int? clientId, string? status, string? billingType)
        {
            var query = _context.Projects.Where(p => p.UserId == userId);
            if (clientId.HasValue)
                query = query.Where(p => p.ClientId == clientId.Value);
            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<ProjectStatus>(status, out var parsedStatus))
                    query = query.Where(p => p.Status == parsedStatus);
            }
            if (!string.IsNullOrEmpty(billingType))
            {
                if (Enum.TryParse<BillingType>(billingType, out var parsedBillingType))
                    query = query.Where(p => p.BillingType == parsedBillingType);
            }

            return await query.ToListAsync();
        }
        public async Task<Project?> GetProjectWithTimeEntriesAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.TimeEntries)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}