namespace FreelanceManager.Core.DTOs
{
    public class PageResult<T>
    {
        public List<T> Data { get; set; } = null!;
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}