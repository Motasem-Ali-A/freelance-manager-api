using FreelanceManager.Core.DTOs;
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

        public async Task<PageResult<Project>> GetAllByUserIdAsync(string userId, int? clientId, string? status, string? billingType, int page = 1, int pageSize = 10)
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

            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PageResult<Project>
            {
                Data = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }
        public async Task<Project?> GetProjectWithTimeEntriesAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.TimeEntries)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}