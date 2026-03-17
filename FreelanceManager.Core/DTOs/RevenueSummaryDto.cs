namespace FreelanceManager.Core.DTOs
{
    public class RevenueSummaryDto
    {
        public decimal ThisMonth { get; set; }
        public decimal LastMonth { get; set; }
        public decimal ThisYear { get; set; }
        public decimal Outstanding { get; set; }
    }
}