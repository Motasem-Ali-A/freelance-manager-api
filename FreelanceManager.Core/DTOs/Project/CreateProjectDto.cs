namespace FreelanceManager.Core.DTOs.Project;

public class CreateProjectDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal FixedPrice { get; set; }
    public string BillingType { get; set; } = string.Empty;
    public int ClientId { get; set; }
}