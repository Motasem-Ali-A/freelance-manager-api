namespace FreelanceManager.Core.DTOs{
    public class InvoiceSummaryDto
    {
        public int Draft { get; set; }
        public int Sent { get; set; }
        public int Paid { get; set; }
        public int Overdue { get; set; }
    }
}