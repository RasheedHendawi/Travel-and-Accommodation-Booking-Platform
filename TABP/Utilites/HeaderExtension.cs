using Domain.Models;
using System.Text.Json;
namespace TABP.Utilites
{
    public static class HeaderExtension
    {
        public static void AddPaginationData(this IHeaderDictionary headers,
          PaginationMetaData paginationMetadata)
        {
            headers["x-pagination"] = JsonSerializer.Serialize(paginationMetadata);
        }
    }
}
