using Application.Contracts;
using Application.DTOs.Discounts;
using Asp.Versioning;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Utilites;

namespace TABP.Controllers
{
    /// <summary>
    /// Controller for managing discounts related to room classes.
    /// </summary>
    [ApiController]
    [Route("api/room-classes/{roomClassId:guid}/discounts")]
    [ApiVersion("1.0")]
    [Authorize(Roles = UserRoles.Admin)]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscountsController"/> class.
        /// </summary>
        /// <param name="discountService">Service for discount operations.</param>
        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }
        /// <summary>
        /// Retrieves a list of discounts for a specific room class.
        /// </summary>
        /// <param name="roomClassId">The unique identifier of the room class.</param>
        /// <param name="request">Filter parameters for retrieving discounts.</param>
        /// <returns>A list of discounts with pagination metadata.</returns>
        /// <response code="200">Returns the request page of amenities, with pagination metadata included.</response>
        /// <response code="400">If the request parameters are invalid or missing.</response>
        /// <response code="404">If the room class with ID is not found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DiscountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DiscountResponse>>> GetAmenities(
            Guid roomClassId,
            [FromQuery] DiscountsGetRequest request)
        {
            var discounts = await _discountService.GetAmenitiesAsync(roomClassId, request);
            Response.Headers.AddPaginationData(discounts.PaginationMetadata);
            return Ok(discounts.Items);
        }
        /// <summary>
        /// Retrieves a specific discount by ID.
        /// </summary>
        /// <param name="roomClassId">The unique identifier of the room class.</param>
        /// <param name="id">The unique identifier of the discount.</param>
        /// <returns>The requested discount.</returns>
        /// <response code="200">Returns the discount specified by ID.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DiscountResponse>> GetDiscount(Guid roomClassId, Guid id)
        {
            var discount = await _discountService.GetDiscountByIdAsync(roomClassId, id);
            return Ok(discount);
        }
        /// <summary>
        /// Creates a new discount for a room class.
        /// </summary>
        /// <param name="roomClassId">The unique identifier of the room class.</param>
        /// <param name="request">The details of the discount to create.</param>
        /// <returns>The newly created discount.</returns>
        /// <response code="201">If the discount was created successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the room class is not found.</response>
        /// <response code="409">If another discount intersects with the new discount in date intervals</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPost]
        public async Task<IActionResult> CreateDiscount(Guid roomClassId, DiscountCreationRequest request)
        {
            var createdDiscount = await _discountService.CreateDiscountAsync(roomClassId, request);
            return CreatedAtAction(nameof(GetDiscount), new { roomClassId, id = createdDiscount.Id },
                createdDiscount); 
        }
        /// <summary>
        /// Deletes a discount by ID.
        /// </summary>
        /// <param name="roomClassId">The unique identifier of the room class.</param>
        /// <param name="id">The unique identifier of the discount to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">If the discount is deleted successfully.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">
        ///   If the room class is not found or
        ///   discount is not found in room class.
        /// </response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteDiscount(Guid roomClassId, Guid id)
        {
            await _discountService.DeleteDiscountAsync(roomClassId, id);
            return NoContent();
        }
    }
}
    
