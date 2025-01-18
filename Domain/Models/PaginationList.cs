namespace Domain.Models
{
    public record PaginatedList<TItem>(IEnumerable<TItem> Items, PaginationMetaData PaginationMetadata);
}
