using System.Security.Claims;
using FreelanceManager.Core.DTOs;
using FreelanceManager.Core.Exceptions;
using FreelanceManager.Core.interfaces;
using FreelanceManager.Core.Models;
using FreelanceManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreelanceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IInvoiceService _invoiceService;

        public InvoiceItemsController(ApplicationDbContext context, IInvoiceService invoiceService)
        {
            _context = context;
            _invoiceService = invoiceService;
        }

        [HttpPost]
        public async Task<IActionResult> AddInvoiceItem([FromBody] CreateInvoiceItemDto dto)
        {
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(i => i.Id == dto.InvoiceId);

            if (invoice == null)
                throw new NotFoundException($"Invoice with ID {dto.InvoiceId} not found");

            var item = new InvoiceItem
            {
                Description = dto.Description,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                Total = dto.Quantity * dto.UnitPrice,
                InvoiceId = dto.InvoiceId
            };

            _context.InvoiceItems.Add(item);
            await _context.SaveChangesAsync();

            invoice.InvoiceItems.Add(item);
            _invoiceService.CalculateTotals(invoice);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                item.Id,
                item.Description,
                item.Quantity,
                item.UnitPrice,
                item.Total,
                item.InvoiceId
            });
        }
    }
}