namespace Ledgerly.API.Helpers
{
    public class PaginationRequest
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public PropertyFilter Filter { get; set; } = new();
    }

    public class PropertyFilter
    {
        public string? Search { get; set; }
        public int? Status { get; set; }
        public List<SortOption> Sort { get; set; } = [];
    }

    public class SortOption
    {
        public string? Property { get; set; } = "id";
        public string Direction { get; set; } = "desc";
    }
}
