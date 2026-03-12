using FreelanceManager.Core.DTOs.Invoice;
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
        public InvoicesController(IInvoiceRepository invoiceRepository )
        {
            _invoiceRepository = invoiceRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            var responses = new List<InvoiceResponseDto>();
            foreach (var invoice in invoices)
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
                        Status = invoice.Status,
                        Notes = invoice.Notes,
                        Subtotal = invoice.Subtotal,
                        TaxRate = invoice.TaxRate,
                        TaxAmount = invoice.TaxAmount,
                        TotalAmount = invoice.TotalAmount,
                        ClientId = invoice.ClientId
                    }

                );
            }
            return Ok(responses);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                return NotFound($"Invoice with ID {id} Not Found");
            return Ok(new InvoiceResponseDto
            {
                Id = invoice.Id,
                CreatedAt = invoice.CreatedAt,
                InvoiceNumber = invoice.InvoiceNumber,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                Status = invoice.Status,
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
            var invoice = new Invoice
            {
                CreatedAt = DateTime.UtcNow,
                InvoiceNumber = "",
                IssueDate = dto.IssueDate,
                DueDate = dto.DueDate,
                Status = "Draft",
                Notes = dto.Notes,
                Subtotal = 0,
                TaxRate = dto.TaxRate,
                TaxAmount = 0,
                TotalAmount = 0,
                ClientId = dto.ClientId
            };
            await _invoiceRepository.AddAsync(invoice);
            await _invoiceRepository.SaveChangesAsync();

            var response = new InvoiceResponseDto
            {
                Id = invoice.Id,
                CreatedAt = invoice.CreatedAt,
                InvoiceNumber = invoice.InvoiceNumber,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                Status = invoice.Status,
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
                return NotFound($"Invoice with ID {id} Not Found");
            invoice.DueDate = dto.DueDate;
            invoice.Status = dto.Status;
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
                Status = invoice.Status,
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
                return NotFound($"Invoice with ID {id} Not Found");
            _invoiceRepository.Delete(invoice);
            await _invoiceRepository.SaveChangesAsync();
            return NoContent();
        }

    }

}