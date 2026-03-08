using FreelanceManager.Core.DTOs.Invoice;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private static List<InvoiceResponseDto> _invoices = new List<InvoiceResponseDto>
        {
            new InvoiceResponseDto
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                InvoiceNumber = "INV - 10063",
                IssueDate = new DateTime(2026 , 05 , 01),
                DueDate= new DateTime(2026 , 05 , 15),
                Status = "Sent",
                Notes = " ",
                Subtotal = 100,
                TaxRate = 0.05M,
                TaxAmount = 5,
                TotalAmount = 105,
                ClientId = 1
            },
            new InvoiceResponseDto
            {
                Id = 2,
                CreatedAt = DateTime.UtcNow,
                InvoiceNumber = "INV - 10083",
                IssueDate = new DateTime(2026 , 05 , 10),
                DueDate= new DateTime(2026 , 05 , 24),
                Status = "Sent",
                Notes = " ",
                Subtotal = 110,
                TaxRate = 0.05M,
                TaxAmount = 5,
                TotalAmount = 115.5M,
                ClientId = 2
            }

        };
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_invoices);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var invoice = _invoices.FirstOrDefault(I => I.Id == id);
            if (invoice == null)
                return NotFound($"Invoice with ID {id} Not Found");
            return Ok(invoice);
        }
        [HttpPost]
        public IActionResult AddInvoice([FromBody] CreateInvoiceDto dto)
        {
            var invoice = new InvoiceResponseDto
            {
                Id = _invoices.Count + 1,
                CreatedAt = DateTime.UtcNow,
                InvoiceNumber = "INV - 10095",
                IssueDate = dto.IssueDate,
                DueDate = dto.DueDate,
                Status = "Draft",
                Notes = dto.Notes,
                Subtotal = 115,
                TaxRate = dto.TaxRate,
                TaxAmount = 0,
                TotalAmount = 105.5M,
                ClientId = dto.ClientId
            };
            _invoices.Add(invoice);
            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateInvoice(int id, [FromBody] UpdateInvoiceDto dto)
        {
            var invoice = _invoices.FirstOrDefault(I => I.Id == id);
            if (invoice == null)
                return NotFound($"Invoice with ID {id} Not Found");
            invoice.DueDate = dto.DueDate;
            invoice.Status = dto.Status;
            invoice.Notes = dto.Notes;
            return Ok(invoice);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteInvoice(int id)
        {
            var invoice = _invoices.FirstOrDefault(I => I.Id == id);
            if (invoice == null)
                return NotFound($"Invoice with ID {id} Not Found");
            _invoices.Remove(invoice);
            return NoContent();
        }

    }

}