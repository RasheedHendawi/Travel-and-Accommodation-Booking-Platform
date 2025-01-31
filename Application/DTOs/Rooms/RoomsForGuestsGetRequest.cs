using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Rooms
{
    public class RoomsForGuestsGetRequest
    {
        public string? SearchTerm { get; init; }
        public SortMethod? SortOrder { get; init; }
        public string? SortColumn { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public DateOnly CheckIn { get; init; }
        public DateOnly CheckOut { get; init; }
    }
}
