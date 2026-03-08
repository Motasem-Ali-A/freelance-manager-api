namespace FreelanceManager.Core.DTOs.TimeEntry;

public class TimeEntryResponseDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Date { get; set; }
    public decimal HoursWorked { get; set; }
    public string Description { get; set; } = string.Empty;
    public int ProjectId { get; set; }
}
