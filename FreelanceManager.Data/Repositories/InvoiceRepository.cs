using FreelanceManager.Core.Enums;
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

        public async Task<List<Invoice>> GetAllByUserIdAsync(string userId, DateTime? from, DateTime? to, string? status)
        {
            var query = _context.Invoices.Where(i => i.UserId == userId);
            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<InvoiceStatus>(status, out var parsedStatus))
                    query = query.Where(i => i.Status == parsedStatus);
            }
            if (from.HasValue)
                query = query.Where(i => i.IssueDate >= from.Value);
            if (to.HasValue)
                query = query.Where(i => i.IssueDate <= to.Value);

            return await query.ToListAsync();
        }
    }
}