using System.ComponentModel.DataAnnotations;

namespace FreelanceManager.Core.DTOs.Project;

public class UpdateProjectDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Required]
    public string Status { get; set; } = string.Empty;
    public DateTime EndDate { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal FixedPrice { get; set; }


}