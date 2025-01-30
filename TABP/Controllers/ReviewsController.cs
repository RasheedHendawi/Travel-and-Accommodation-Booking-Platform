using Application.Contracts;
using Application.DTOs.Hotels;
using Application.DTOs.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TABP.Controllers
{
    [ApiController]
    [Route("api/hotels/{hotelId:guid}/reviews")]
    [Authorize(Roles = "Guest")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewsForHotel(Guid hotelId, [FromQuery] ReviewsGetRequest request)
        {
            var reviews = await _reviewService.GetReviewsForHotelAsync(hotelId, request);
            return Ok(reviews);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewById(Guid hotelId, Guid id)
        {
            var review = await _reviewService.GetReviewByIdAsync(hotelId, id);
            return Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReviewForHotel(Guid hotelId, ReviewCreationRequest request)
        {
            var createdReview = await _reviewService.CreateReviewAsync(hotelId, request);
            return CreatedAtAction(nameof(GetReviewById), new { hotelId, id = createdReview.Id }, createdReview);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateReviewForHotel(Guid hotelId, Guid id, ReviewUpdateRequest request)
        {
            await _reviewService.UpdateReviewAsync(hotelId, id, request);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteReviewForHotel(Guid hotelId, Guid id)
        {
            await _reviewService.DeleteReviewAsync(hotelId, id);
            return NoContent();
        }
    }
}
