using System.ComponentModel.DataAnnotations;

namespace FreelanceManager.Core.DTOs.Project;

public class CreateProjectDto
{
    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    [Range(0,10000)]
    public decimal HourlyRate { get; set; }
     [Range(0,1000000)]
    public decimal FixedPrice { get; set; }
    [Required, MaxLength(20)]
    public string BillingType { get; set; } = string.Empty;
    [Required, Range(1, int.MaxValue)]
    public int ClientId { get; set; }
}