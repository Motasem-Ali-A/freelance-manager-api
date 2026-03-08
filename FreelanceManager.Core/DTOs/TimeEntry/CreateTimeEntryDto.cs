namespace FreelanceManager.Core.DTOs.TimeEntry;

public class CreateTimeEntryDto
{
    public DateTime Date { get; set; }
    public decimal HoursWorked { get; set; }
    public string Description { get; set; } = string.Empty;
    public int ProjectId {get; set;}
}
