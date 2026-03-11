using FreelanceManager.Core.interfaces;
using FreelanceManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FreelanceManager.Data.Repositories
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Invoice>> GetAllByClientIdAsync(int clientId)
        {
            return await _context.Invoices
                .Where(i => i.ClientId == clientId)
                .ToListAsync();
        }
    }
}