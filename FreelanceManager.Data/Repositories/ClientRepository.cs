using FreelanceManager.Core.DTOs;
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

        public async Task<PageResult<Client>> GetAllByUserIdAsync(string userId, string? status = null, string? search = null, int page = 1, int pageSize = 10)
        {
            var query = _context.Clients.Where(c => c.UserId ==userId);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(c => c.Name.Contains(search) || c.CompanyName.Contains(search));

            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PageResult<Client>
            {
                Data = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }
        
    }
}