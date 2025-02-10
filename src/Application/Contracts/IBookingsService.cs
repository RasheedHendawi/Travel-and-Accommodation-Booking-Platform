

using Application.DTOs.Bookings;
using Application.DTOs.Shared;
using Domain.Models;

namespace Application.Contracts
{
    public interface IBookingsService
    {
        Task<BookingResponse> CreateBookingAsync(BookingCreationRequest request);
        Task DeleteBookingAsync(Guid bookingId);
        Task<byte[]> GetInvoiceAsPdfAsync(Guid bookingId);
        Task<BookingResponse> GetBookingAsync(Guid bookingId);
        Task<PaginatedList<BookingResponse>> GetBookingsAsync(BookingsGetRequest request);
    }
}
