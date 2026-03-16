using System.ComponentModel.DataAnnotations;

namespace FreelanceManager.Core.DTOs.Invoice;

public class CreateInvoiceDto
{
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    [MaxLength(500)]
    public string Notes { get; set; } = string.Empty;
    [Range(0, 1)]
    public decimal TaxRate { get; set; }
    [Required, Range(1, int.MaxValue)]
    public int ClientId { get; set; }
}