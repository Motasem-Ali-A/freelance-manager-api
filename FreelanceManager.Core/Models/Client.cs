using System.ComponentModel.DataAnnotations;
namespace FreelanceManager.Core.Models
{
    public class Client
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
        [MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Notes { get; set; } = string.Empty;
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = null!;

    }
}