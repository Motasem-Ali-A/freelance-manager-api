namespace FreelanceManager.Core.DTOs
{
    public class DashboardSummaryDto
    {
        public int TotalClients { get; set; }
        public int ActiveProjects { get; set; }
        public InvoiceSummaryDto Invoices { get; set; } = null!;
        public RevenueSummaryDto Revenues { get; set; } = null!;
        public int UpcomingDeadlines { get; set; }
    }
}