using System.ComponentModel.DataAnnotations;

namespace FreelanceManager.Core.DTOs.TimeEntry;

public class CreateTimeEntryDto
{
    public DateTime Date { get; set; }
    [Range(0,24)]
    public decimal HoursWorked { get; set; }
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    [Required,Range(1, int.MaxValue)]
    public int ProjectId {get; set;}
}
