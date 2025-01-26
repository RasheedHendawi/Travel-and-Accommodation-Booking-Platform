using Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Utilites
{
    public static class PageExension
    {
        public static IQueryable<TItem> GetPage<TItem>(
          this IQueryable<TItem> queryable,
          int pageNumber, int pageSize)
        {
            return queryable.Skip(pageSize * (pageNumber - 1))
              .Take(pageSize);
        }

        public static async Task<PaginationMetaData> GetPaginationDataAsync<TItem>(
          this IQueryable<TItem> queryable,
          int pageNumber, int pageSize)
        {
            return new PaginationMetaData(
              await queryable.CountAsync(),
              pageNumber,
              pageSize);
        }
    }
}
