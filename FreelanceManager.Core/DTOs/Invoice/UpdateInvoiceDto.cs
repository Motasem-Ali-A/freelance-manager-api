namespace FreelanceManager.Core.DTOs.Invoice;

public class UpdateInvoiceDto
{
    public DateTime DueDate {get; set;}
    public string Status {get; set;} = string.Empty;
    public string Notes {get; set;} = string.Empty;
}