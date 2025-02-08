using Application.Contracts;
using Application.DTOs.Hotels;
using Application.DTOs.Images;
using Asp.Versioning;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using TABP.Utilites;

namespace TABP.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    [ApiVersion("1.0")]
    [Authorize(Roles = UserRoles.Admin)]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelsService;
        
        public HotelsController(IHotelService hotelsService)
        {
            _hotelsService = hotelsService;
        }
        /// <summary>
        /// Retrieves a list of hotels for management purposes, with optional filtering and pagination.
        /// </summary>
        /// <param name="request">The request parameters for pagination and filtering.</param>
        /// <returns>A list of hotels for management.</returns>
        /// <response code="200">Returns the request page of hotels for management, with pagination metadata included.</response>
        /// <response code="400">If the request parameters are invalid or missing.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HotelGetFromManagment>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<HotelGetFromManagment>>> GetHotelsForManagement([FromQuery] HotelGetRequest request)
        {
            var hotels = await _hotelsService.GetHotelsForManagementAsync(request);
            Response.Headers.AddPaginationData(hotels.PaginationMetadata);
            return Ok(hotels.Items);
        }

        /// <summary>
        /// Retrieves hotels based on search and filter criteria.
        /// </summary>
        /// <param name="hotelSearchRequest">The search and filter criteria for hotels.</param>
        /// <returns>A list of hotels based on the search and filter criteria.</returns>
        /// <response code="200">requested page of hotels based on the provided criteria, with pagination metadata included.</response>
        /// <response code="400">If the request data is invalid or missing.</response>
        [ProducesResponseType(typeof(IEnumerable<HotelSearchResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<HotelSearchResponse>>> SearchAndFilterHotels(
            [FromQuery] HotelSearchRequest hotelSearchRequest)
        {
            var hotels = await _hotelsService.SearchAndFilterHotelsAsync(hotelSearchRequest);

            Response.Headers.AddPaginationData(hotels.PaginationMetadata);

            return Ok(hotels.Items);
        }

        /// <summary>
        /// Retrieves featured hotel deals.
        /// </summary>
        /// <param name="request">The number of featured deals to retrieve.</param>
        /// <returns>A list of featured hotel deals.</returns>
        /// <response code="200">Returns the requested number of hotel featured deals.</response>
        /// <response code="400">If the count is less than 1 or greater than 100.</response>
        [HttpGet("featured-deals")]
        [ProducesResponseType(typeof(IEnumerable<HotelFeaturedDealResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HotelFeaturedDealResponse>>> GetFeaturedDeals([FromQuery] FeaturedDealsRequest request)
        {
            var featuredDeals = await _hotelsService.GetFeaturedDealsAsync(request);
            return Ok(featuredDeals);
        }
        /// <summary>
        /// Retrieves a specific hotel by its ID.
        /// </summary>
        /// <param name="id">The ID of the hotel to retrieve.</param>
        /// <returns>The hotel with the specified ID.</returns>
        /// <response code="200">Returns the requested hotel specified by ID.</response>
        /// <response code="404">If the hotel with the specified ID is not found.</response>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(HotelGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelGetResponse>> GetHotel(Guid id)
        {
            var hotel = await _hotelsService.GetHotelByIdAsync(id);
            return Ok(hotel);
        }
        /// <summary>
        /// Creates a new hotel.
        /// </summary>
        /// <param name="request">The data required to create a new hotel.</param>
        /// <returns>The created hotel with a 201 status code.</returns>
        /// <response code="200">Returns the requested room classes for the specified hotel, with pagination metadata included..</response>
        /// <response code="404">If the specified hotel by ID was not found.</response>
        /// <response code="201">If the hotel was created successfully.</response>
        /// <response code="400">If the request data is invalid or missing.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="409">If there is an hotel in the same geographical location (longitude and latitude)</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] HotelCreationRequest request)
        {
            var hotelId = await _hotelsService.CreateHotelAsync(request);
            return CreatedAtAction(nameof(GetHotel), new { id = hotelId }, null);
        }

        /// <summary>
        /// Updates an existing hotel by its ID.
        /// </summary>
        /// <param name="id">The ID of the hotel to update.</param>
        /// <param name="request">The updated data for the hotel.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the hotel was updated successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the hotel was not found.</response>
        /// <response code="409">If there is an hotel in the same geographical location (longitude and latitude)</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateHotel(Guid id, [FromBody] HotelUpdateRequest request)
        {
            await _hotelsService.UpdateHotelAsync(id, request);
            return NoContent();
        }
        /// <summary>
        /// Deletes a specific hotel by its ID.
        /// </summary>
        /// <param name="id">The ID of the hotel to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">If the hotel was deleted successfully.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the hotel was not found.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteHotel(Guid id)
        {
            await _hotelsService.DeleteHotelAsync(id);
            return NoContent();
        }
        /// <summary>
        /// Sets the thumbnail image for a hotel.
        /// </summary>
        /// <param name="id">The ID of the hotel.</param>
        /// <param name="image">The image to set as the thumbnail.</param>
        /// <returns>No content if the operation is successful.</returns>
        [HttpPut("{id:guid}/thumbnail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetHotelThumbnail(Guid id, [FromForm] ImageCreationRequest image)
        {
            await _hotelsService.SetHotelThumbnailAsync(id, image);
            return NoContent();
        }
        /// <summary>
        /// Adds an image to the hotel's gallery.
        /// </summary>
        /// <param name="id">The ID of the hotel.</param>
        /// <param name="image">The image to add to the gallery.</param>
        /// <returns>No content if the image is successfully added.</returns>
        /// <response code="204">If the image was added to the gallery successfully.</response>
        /// <response code="400">If the image is invalid (not .jpg, .jpeg, or .png).</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the hotel specified by ID is not found.</response>
        [HttpPost("{id:guid}/gallery")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddImageToHotelGallery(Guid id, [FromForm] ImageCreationRequest image)
        {
            await _hotelsService.AddImageToGalleryAsync(id, image);
            return NoContent();
        }
    }
}
