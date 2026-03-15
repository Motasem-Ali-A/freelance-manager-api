using System.ComponentModel.DataAnnotations;
using FreelanceManager.Core.Enums;
namespace FreelanceManager.Core.Models
{
    public class Project
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal FixedPrice { get; set; }
        public BillingType BillingType { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
        public ICollection<TimeEntry> TimeEntries {get; set; }= new List<TimeEntry>();
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = null!;
    }
}