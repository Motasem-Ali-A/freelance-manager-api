namespace FreelanceManager.Core.DTOs.Invoice;

public class CreateInvoiceDto
{
    public DateTime IssueDate {get; set;}
    public DateTime DueDate {get; set;}
    public string Notes {get; set;} = string.Empty;
    public decimal TaxRate {get; set;}
    public int ClientId {get; set;}
}