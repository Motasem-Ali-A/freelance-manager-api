using System.ComponentModel.DataAnnotations;

namespace FreelanceManager.Core.DTOs.Invoice;

public class UpdateInvoiceDto
{
    public DateTime DueDate { get; set; }
    [Required, MaxLength(20)]
    public string Status { get; set; } = string.Empty;
    [MaxLength(500)]
    public string Notes { get; set; } = string.Empty;
}