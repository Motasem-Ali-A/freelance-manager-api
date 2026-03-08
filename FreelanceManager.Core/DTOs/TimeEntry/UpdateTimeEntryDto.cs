namespace FreelanceManager.Core.DTOs.TimeEntry;

public class UpdateTimeEntryDto
{
    public decimal HoursWorked { get; set; }
    public string Description { get; set; } = string.Empty;
}
