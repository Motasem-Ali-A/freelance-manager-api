using System.ComponentModel.DataAnnotations;

namespace FreelanceManager.Core.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required, MaxLength(20)]
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Notes { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }
}