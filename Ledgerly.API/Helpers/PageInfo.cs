namespace Ledgerly.API.Helpers
{
    //https://codewithmukesh.com/blog/pagination-in-aspnet-core-webapi/#google_vignette
    public class PageInfo
    {
        public PageInfo(int page, int pageSize, int totalRecords)
        {
            Page = page;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = Convert.ToInt32(Math.Ceiling((double)totalRecords / (double)pageSize));
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }
}
