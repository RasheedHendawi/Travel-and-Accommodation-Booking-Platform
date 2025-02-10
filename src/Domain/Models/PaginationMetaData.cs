

namespace Domain.Models
{
    public record PaginationMetaData ( int TotalItem, int CurrentPage, int PageSize)
    {
        public int PageCount => (int) Math.Ceiling(TotalItem / (double) PageSize);
    }
}
