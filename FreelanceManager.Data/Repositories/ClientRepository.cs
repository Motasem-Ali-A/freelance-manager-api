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

        public async Task<List<Client>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Set<Client>().Where(c => c.UserId == userId).ToListAsync();
        }
        
    }
}