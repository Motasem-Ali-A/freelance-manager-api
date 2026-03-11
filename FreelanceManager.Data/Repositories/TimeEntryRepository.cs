using FreelanceManager.Core.interfaces;
using FreelanceManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FreelanceManager.Data.Repositories
{
    public class TimeEntryRepository : Repository<TimeEntry>, ITimeEntryRepository
    {
        public TimeEntryRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public async Task<List<TimeEntry>> GetAllByProjectIdAsync(int projectId)
        {
            return await _context.TimeEntries
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }
    }
}