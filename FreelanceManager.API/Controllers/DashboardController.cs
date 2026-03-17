using System.Security.Claims;
using FreelanceManager.Core.DTOs;
using FreelanceManager.Core.Enums;
using FreelanceManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreelanceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var invoicSummary = new InvoiceSummaryDto
            {
                Draft = await _context.Invoices.Where(i => i.UserId == userId && i.Status == InvoiceStatus.Draft).CountAsync(),
                Sent = await _context.Invoices.Where(i => i.UserId == userId && i.Status == InvoiceStatus.Sent).CountAsync(),
                Paid = await _context.Invoices.Where(i => i.UserId == userId && i.Status == InvoiceStatus.Paid).CountAsync(),
                Overdue = await _context.Invoices.Where(i => i.UserId == userId && i.Status == InvoiceStatus.Overdue).CountAsync()
            };

            var now = DateTime.UtcNow;
            var lastMonth = now.AddMonths(-1);
            var revenueSummary = new RevenueSummaryDto
            {
                ThisMonth = (await _context.Invoices
                    .Where(i => i.UserId == userId
                        && i.Status == InvoiceStatus.Paid
                        && i.IssueDate.Month == now.Month
                        && i.IssueDate.Year == now.Year)
                    .ToListAsync())
                    .Sum(i => i.TotalAmount),
                LastMonth = (await _context.Invoices
                    .Where(i => i.UserId == userId
                        && i.Status == InvoiceStatus.Paid
                        && i.IssueDate.Month == lastMonth.Month
                        && i.IssueDate.Year == lastMonth.Year)
                    .ToListAsync())
                    .Sum(i => i.TotalAmount),
                ThisYear = (await _context.Invoices
                    .Where(i => i.UserId == userId
                        && i.Status == InvoiceStatus.Paid
                        && i.IssueDate.Year == now.Year)
                    .ToListAsync())
                    .Sum(i => i.TotalAmount),
                Outstanding = (await _context.Invoices
                    .Where(i => i.UserId == userId
                        && i.Status == InvoiceStatus.Sent)
                    .ToListAsync())
                    .Sum(i => i.TotalAmount),
            };


            return Ok(new DashboardSummaryDto
            {
                TotalClients = await _context.Clients.Where(c => c.UserId == userId).CountAsync(),
                ActiveProjects = await _context.Projects
                    .Where(p => p.UserId == userId && p.Status == ProjectStatus.InProgress)
                    .CountAsync(),
                Invoices = invoicSummary,
                Revenues = revenueSummary,
                UpcomingDeadlines = await _context.Projects
                    .Where(p => p.UserId == userId
                        && p.EndDate >= now
                        && p.EndDate <= now.AddDays(7))
                    .CountAsync()
            });
        }
    }
}