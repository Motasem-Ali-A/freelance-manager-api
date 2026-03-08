namespace FreelanceManager.Core.Models
{
    public class Client
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    }
}