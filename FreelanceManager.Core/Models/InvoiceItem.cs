using System.ComponentModel.DataAnnotations;
namespace FreelanceManager.Core.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;
    }
}