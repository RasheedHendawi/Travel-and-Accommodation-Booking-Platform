using Application.Contracts;
using Application.DTOs.Hotels;
using Application.DTOs.Reviews;
using Asp.Versioning;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Utilites;

namespace TABP.Controllers
{
    /// <summary>
    /// Handles review-related actions for a hotel.
    /// </summary>
    [ApiController]
    [Route("api/hotels/{hotelId:guid}/reviews")]
    [ApiVersion("1.0")]
    [Authorize(Roles = UserRoles.Guest)]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        /// <summary>
        /// Retrieves the reviews for a specific hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <param name="request">The pagination and filter information for reviews.</param>
        /// <returns>A list of reviews for the specified hotel.</returns>
        /// <response code="200">Returns the request page of reviews, with pagination metadata included.</response>
        /// <response code="400">If the request parameters are invalid or missing.</response>
        /// <response code="404">If the hotel specified by ID was not found.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewsForHotel(Guid hotelId, [FromQuery] ReviewsGetRequest request)
        {
            var reviews = await _reviewService.GetReviewsForHotelAsync(hotelId, request);
            Response.Headers.AddPaginationData(reviews.PaginationMetadata);
            return Ok(reviews.Items);
        }
        /// <summary>
        /// Retrieves a specific review by its ID for a particular hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <param name="id">The ID of the review.</param>
        /// <returns>The review with the specified ID.</returns>
        /// <response code="200">The requested review.</response>
        /// <response code="404">If the hotel specified by ID was not found or the review with ID is not found in the hotel.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewById(Guid hotelId, Guid id)
        {
            var review = await _reviewService.GetReviewByIdAsync(hotelId, id);
            return Ok(review);
        }
        /// <summary>
        /// Creates a new review for a hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <param name="request">The review data to be created.</param>
        /// <returns>The created review.</returns>
        /// <response code="201">If the review was created successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not a guest).</response>
        /// <response code="404">If the hotel specified by ID is not found.</response>
        /// <response code="409">
        ///   If The guest did not book a room in the hotel yet, or
        ///   If the guest has already reviewed the hotel.
        /// </response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<IActionResult> CreateReviewForHotel(Guid hotelId, ReviewCreationRequest request)
        {
            var createdReview = await _reviewService.CreateReviewAsync(hotelId, request);
            return CreatedAtAction(nameof(GetReviewById), new { hotelId, id = createdReview.Id }, createdReview);
        }
        /// <summary>
        /// Updates an existing review for a hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <param name="id">The ID of the review to be updated.</param>
        /// <param name="request">The updated review data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the hotel was updated successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not a guest).</response>
        /// <response code="404">
        ///   If the hotel was not found or the review for the guest in the hotel is not found.
        /// </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateReviewForHotel(Guid hotelId, Guid id, ReviewUpdateRequest request)
        {
            await _reviewService.UpdateReviewAsync(hotelId, id, request);
            return NoContent();
        }
        /// <summary>
        /// Deletes a review for a hotel.
        /// </summary>
        /// <param name="hotelId">The ID of the hotel.</param>
        /// <param name="id">The ID of the review to be deleted.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the review was deleted successfully.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not a guest).</response>
        /// <response code="404">
        ///   If the hotel was not found or the review for the guest in the hotel is not found.
        /// </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteReviewForHotel(Guid hotelId, Guid id)
        {
            await _reviewService.DeleteReviewAsync(hotelId, id);
            return NoContent();
        }
    }
}
