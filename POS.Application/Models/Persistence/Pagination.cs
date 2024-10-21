namespace POS.Application.Models.Persistence
{
    public class PaginationRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public PaginationRequest()
        {
            Page = 1;
            PageSize = 10;
        }
    }

    public class PaginationResponse<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Content { get; set; }
    }
}
