using FreelanceManager.Core.DTOs;
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

        public async Task<PageResult<Invoice>> GetAllByUserIdAsync(string userId, DateTime? from, DateTime? to, string? status, int page = 1, int pageSize = 10)
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

            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PageResult<Invoice>
            {
                Data = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }

        public async Task<Invoice?> GetInvoiceWithItemsAsync(int id)
        {
            return await _context.Invoices
                    .Include(i => i.InvoiceItems)
                    .Include(i => i.Client)
                    .FirstOrDefaultAsync(i => i.Id == id);
                    
        }
    }
}