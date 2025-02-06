using Application.Contracts;
using Application.DTOs.Bookings;
using Asp.Versioning;
using Azure;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Utilites;

namespace TABP.Controllers
{
    [ApiController]
    [Route("api/user/bookings")]
    [ApiVersion("1.0")]
    //[Authorize(Roles = UserRoles.Guest)]
    public class BookingsController(IBookingsService bookingService) : ControllerBase
    {
        /// <summary>
        /// Create a new booking for the current user.
        /// </summary>
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
        /// Delete an existing booking specified by ID for the current user.
        /// </summary>
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
        /// Get the invoice of a booking as a PDF for the current user.
        /// </summary>
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
        /// Get a booking specified by ID for the current user.
        /// </summary>
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
        /// Get a page of bookings for the current user.
        /// </summary>
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
