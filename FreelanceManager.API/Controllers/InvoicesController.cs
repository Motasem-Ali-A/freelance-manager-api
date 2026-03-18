using System.Reflection.Metadata;
using System.Security.Claims;
using FreelanceManager.API.Documents;
using FreelanceManager.Core.DTOs;
using FreelanceManager.Core.DTOs.Invoice;
using FreelanceManager.Core.Enums;
using FreelanceManager.Core.Exceptions;
using FreelanceManager.Core.interfaces;
using FreelanceManager.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace FreelanceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Tags("Invoices")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceService _invoiceService;
        private readonly UserManager<AppUser> _userManager;
        public InvoicesController(IInvoiceRepository invoiceRepository, IInvoiceService invoiceService, UserManager<AppUser> userManager)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceService = invoiceService;
            _userManager = userManager;
        }

        /// <summary>
        /// Get all invoices for the authenticated user
        /// </summary>
        /// <param name="from">Filter by date (all invoices after the date)</param>
        /// <param name="to">Filter by date (all invoices before the date)</param>
        /// <param name="status">Filter by status (Draft/Sent/Paid/Overdue)</param>
        /// <returns>A paginated list of invoices</returns>
        [HttpGet]
        [ProducesResponseType(typeof(InvoiceResponseDto), 200)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Get an invoice by ID
        /// </summary>
        /// <param name="id">The invoice ID</param>
        /// <returns>invoice details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InvoiceResponseDto), 200)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Add an invoice
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(InvoiceResponseDto), 201)]
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

        /// <summary>
        /// Update the information of certian invoice
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(InvoiceResponseDto), 200)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Delete an invoice and all of it's associated invoice items
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                throw new NotFoundException($"Invoice with ID {id} Not Found");
            _invoiceRepository.Delete(invoice);
            await _invoiceRepository.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Create a downloadable PDF file detailing the invoice
        /// </summary>
        /// <param name="id">The invoice ID</param>
        /// <returns>PDF file</returns>
        [HttpGet("{id}/pdf")]
        [ProducesResponseType(typeof(File), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Pdf(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var invoice = await _invoiceRepository.GetInvoiceWithItemsAsync(id);
            var user = await _userManager.FindByIdAsync(userId);
            if (invoice == null)
                throw new NotFoundException($"Invoice with ID {id} not found");
            if (user == null)
                return Unauthorized();
            var document = new InvoicePdfDocument(invoice, user);
            var pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"Invoice-{invoice.InvoiceNumber}.pdf");
        }


    }

}