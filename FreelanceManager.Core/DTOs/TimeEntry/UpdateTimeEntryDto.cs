using System.ComponentModel.DataAnnotations;

namespace FreelanceManager.Core.DTOs.TimeEntry;

public class UpdateTimeEntryDto
{
    [Range(0,24)]
    public decimal HoursWorked { get; set; }
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
}
