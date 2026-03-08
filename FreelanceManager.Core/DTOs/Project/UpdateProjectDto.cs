namespace FreelanceManager.Core.DTOs.Project;

public class UpdateProjectDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime EndDate { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal FixedPrice { get; set; }


}