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

        public async Task<List<Project>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Set<Project>().Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<List<Project>> GetAllByClientIdAsync(int clientId)
        {
            return await _context.Projects
                .Where(p => p.ClientId == clientId)
                .ToListAsync();
        }
    }
}