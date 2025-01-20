using Domain.Enums;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Infrastructure.Utilites
{
    public static class SortingExtision
    {
        public static IQueryable<TItem> Sort<TItem>(
          this IQueryable<TItem> queryable,
          Expression<Func<TItem, object>> sortColumnExpression,
          SortMethod sortOrder)
        {
            return sortOrder switch
            {
                SortMethod.Ascending => queryable.OrderBy(sortColumnExpression),
                SortMethod.Descending => queryable.OrderByDescending(sortColumnExpression),
                _ => throw new InvalidEnumArgumentException()
            };
        }
    }
}
