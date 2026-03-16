using FreelanceManager.Core.interfaces;
using FreelanceManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FreelanceManager.Data.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {

        public ClientRepository(ApplicationDbContext context) : base(context)
        {
        }
   
        public async Task<Client?> GetClientWithProjectsAsync(int id)
        {
            return await _context.Clients
                .Include(c => c.Projects)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Client>> GetAllByUserIdAsync(string userId, string? status = null, string? search = null)
        {
            var query = _context.Clients.Where(c => c.UserId ==userId);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(c => c.Name.Contains(search) || c.CompanyName.Contains(search));

            return await query.ToListAsync();
        }
        
    }
}