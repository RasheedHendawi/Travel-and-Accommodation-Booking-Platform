using System.Linq.Expressions;
using Domain.Enums;
namespace Domain.Models
{
    public record Query<TEntity>(
      Expression<Func<TEntity, bool>> Filter,
      SortMethod SortOrder,
      string? SortColumn,
      int PageNumber,
      int PageSize);
}