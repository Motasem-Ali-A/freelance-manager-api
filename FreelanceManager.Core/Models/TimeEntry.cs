using System.ComponentModel.DataAnnotations;
namespace FreelanceManager.Core.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Date { get; set; }
        public decimal HoursWorked { get; set; }
        [Required, MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}