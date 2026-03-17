using System.Security.Claims;
using FreelanceManager.Core.DTOs;
using FreelanceManager.Core.DTOs.Invoice;
using FreelanceManager.Core.Enums;
using FreelanceManager.Core.Exceptions;
using FreelanceManager.Core.interfaces;
using FreelanceManager.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceService _invoiceService;
        public InvoicesController(IInvoiceRepository invoiceRepository, IInvoiceService invoiceService)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceService = invoiceService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInvoices([FromQuery] DateTime? from,
             [FromQuery] DateTime? to, [FromQuery] string? status,
             [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var invoices = await _invoiceRepository.GetAllByUserIdAsync(userId, from, to, status, page, pageSize);
            var responses = new List<InvoiceResponseDto>();
            _invoiceService.ApplyOverdueStatus(invoices.Data);
            foreach (var invoice in invoices.Data)
            {
                responses.Add
                (
                    new InvoiceResponseDto
                    {
                        Id = invoice.Id,
                        CreatedAt = invoice.CreatedAt,
                        InvoiceNumber = invoice.InvoiceNumber,
                        IssueDate = invoice.IssueDate,
                        DueDate = invoice.DueDate,
                        Status = invoice.Status.ToString(),
                        Notes = invoice.Notes,
                        Subtotal = invoice.Subtotal,
                        TaxRate = invoice.TaxRate,
                        TaxAmount = invoice.TaxAmount,
                        TotalAmount = invoice.TotalAmount,
                        ClientId = invoice.ClientId
                    }

                );
            }
            return Ok(new PageResult<InvoiceResponseDto>
            {
                Data = responses,
                TotalCount = invoices.TotalCount,
                Page = invoices.Page,
                PageSize = invoices.PageSize,
                TotalPages = invoices.TotalPages
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                throw new NotFoundException($"Invoice with ID {id} Not Found");
            _invoiceService.ApplyOverdueStatus(new List<Invoice> { invoice });
            return Ok(new InvoiceResponseDto
            {
                Id = invoice.Id,
                CreatedAt = invoice.CreatedAt,
                InvoiceNumber = invoice.InvoiceNumber,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                Status = invoice.Status.ToString(),
                Notes = invoice.Notes,
                Subtotal = invoice.Subtotal,
                TaxRate = invoice.TaxRate,
                TaxAmount = invoice.TaxAmount,
                TotalAmount = invoice.TotalAmount,
                ClientId = invoice.ClientId
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddInvoice([FromBody] CreateInvoiceDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var invoice = new Invoice
            {
                CreatedAt = DateTime.UtcNow,
                InvoiceNumber = await _invoiceService.GenerateInvoiceNumberAsync(userId),
                IssueDate = dto.IssueDate,
                DueDate = dto.DueDate,
                Status = InvoiceStatus.Draft,
                Notes = dto.Notes,
                Subtotal = 0,
                TaxRate = dto.TaxRate,
                TaxAmount = 0,
                TotalAmount = 0,
                ClientId = dto.ClientId,
                UserId = userId

            };
            await _invoiceRepository.AddAsync(invoice);
            _invoiceService.CalculateTotals(invoice);
            await _invoiceRepository.SaveChangesAsync();


            var response = new InvoiceResponseDto
            {
                Id = invoice.Id,
                CreatedAt = invoice.CreatedAt,
                InvoiceNumber = invoice.InvoiceNumber,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                Status = invoice.Status.ToString(),
                Notes = invoice.Notes,
                Subtotal = invoice.Subtotal,
                TaxRate = invoice.TaxRate,
                TaxAmount = invoice.TaxAmount,
                TotalAmount = invoice.TotalAmount,
                ClientId = invoice.ClientId
            };
            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] UpdateInvoiceDto dto)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                throw new NotFoundException($"Invoice with ID {id} Not Found");
            invoice.DueDate = dto.DueDate;
            invoice.Status = Enum.Parse<InvoiceStatus>(dto.Status);
            invoice.Notes = dto.Notes;
            _invoiceRepository.Update(invoice);
            await _invoiceRepository.SaveChangesAsync();
            return Ok(new InvoiceResponseDto
            {
                Id = invoice.Id,
                CreatedAt = invoice.CreatedAt,
                InvoiceNumber = invoice.InvoiceNumber,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                Status = invoice.Status.ToString(),
                Notes = invoice.Notes,
                Subtotal = invoice.Subtotal,
                TaxRate = invoice.TaxRate,
                TaxAmount = invoice.TaxAmount,
                TotalAmount = invoice.TotalAmount,
                ClientId = invoice.ClientId
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                throw new NotFoundException($"Invoice with ID {id} Not Found");
            _invoiceRepository.Delete(invoice);
            await _invoiceRepository.SaveChangesAsync();
            return NoContent();
        }

    }

}