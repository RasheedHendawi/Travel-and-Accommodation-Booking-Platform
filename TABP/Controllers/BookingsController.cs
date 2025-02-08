using Application.Contracts;
using Application.DTOs.Bookings;
using Application.DTOs.Shared;
using Asp.Versioning;
using Azure;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Utilites;

namespace TABP.Controllers
{
    /// <summary>
    ///     Controller for managing user bookings.
    /// </summary>
    /// <param name="bookingService"></param>
    [ApiController]
    [Route("api/user/bookings")]
    [ApiVersion("1.0")]
    [Authorize(Roles = UserRoles.Guest)]
    public class BookingsController(IBookingsService bookingService) : ControllerBase
    {
        /// <summary>
        /// Create a new booking for the current user.
        /// </summary>
        /// <param name="bookingCreationRequest">The booking details.</param>
        /// <returns>A newly created booking.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateBooking(
            BookingCreationRequest bookingCreationRequest)
        {
            var createdBooking = await bookingService.CreateBookingAsync(bookingCreationRequest);
            return CreatedAtAction(nameof(GetBooking), new { id = createdBooking.Id }, createdBooking);
        }

        /// <summary>
        /// Deletes an existing booking specified by ID for the current user.
        /// </summary>
        /// <param name="id">The unique identifier of the booking.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            await bookingService.DeleteBookingAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Gets the invoice of a booking as a PDF for the current user.
        /// </summary>
        /// <param name="id">The unique identifier of the booking.</param>
        /// <returns>The invoice PDF file.</returns>
        [HttpGet("{id:guid}/invoice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<FileResult> GetInvoiceAsPdf(Guid id)
        {
            var pdf = await bookingService.GetInvoiceAsPdfAsync(id);
            return File(pdf, "application/pdf", "invoice.pdf");
        }

        /// <summary>
        /// Gets a booking specified by ID for the current user.
        /// </summary>
        /// <param name="id">The unique identifier of the booking.</param>
        /// <returns>The booking details.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingResponse>> GetBooking(Guid id)
        {
            var booking = await bookingService.GetBookingAsync(id);
            return Ok(booking);
        }

        /// <summary>
        /// Gets a page of bookings for the current user.
        /// </summary>
        /// <param name="bookingsGetRequest">The request parameters for fetching bookings.</param>
        /// <returns>A list of bookings.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<BookingResponse>>> GetBookings(
            [FromQuery] BookingsGetRequest bookingsGetRequest)
        {
            var bookings = await bookingService.GetBookingsAsync(bookingsGetRequest);
            Response.Headers.AddPaginationData(bookings.PaginationMetadata);
            return Ok(bookings.Items);
        }
    }
}
