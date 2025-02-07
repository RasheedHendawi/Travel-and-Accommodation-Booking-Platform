using Application.Contracts;
using Application.DTOs.Amenities;
using Application.DTOs.Shared;
using Asp.Versioning;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Utilites;

namespace TABP.Controllers
{
    /// <summary>
    /// Controller for managing amenities.
    /// </summary>
    [ApiController]
    [Route("api/amenities")]
    [ApiVersion("1.0")]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmenityController"/> class.
        /// </summary>
        /// <param name="amenityService">Service for handling amenity operations.</param>
        public AmenityController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        /// <summary>
        /// Retrieves a list of amenities based on the specified criteria.
        /// </summary>
        /// <param name="request">The request parameters for filtering amenities.</param>
        /// <returns>A list of amenities with pagination metadata in the response headers.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AmenityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AmenityResponse>>> GetAmenities([FromQuery] AmenitiesGetRequest request)
        {
            var amenities = await _amenityService.GetAmenitiesAsync(request);
            Response.Headers.AddPaginationData(amenities.PaginationMetadata);
            return Ok(amenities.Items);
        }

        /// <summary>
        /// Retrieves an amenity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the amenity.</param>
        /// <returns>The requested amenity.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(AmenityResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AmenityResponse>> GetAmenity(Guid id)
        {
            var amenity = await _amenityService.GetAmenityByIdAsync(id);
            return Ok(amenity);
        }

        /// <summary>
        /// Creates a new amenity.
        /// </summary>
        /// <param name="request">The details of the amenity to create.</param>
        /// <returns>The newly created amenity.</returns>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateAmenity([FromBody] AmenityCreationRequest request)
        {
            var createdAmenity = await _amenityService.CreateAmenityAsync(request);
            return CreatedAtAction(nameof(GetAmenity), new { id = createdAmenity.Id }, createdAmenity);
        }

        /// <summary>
        /// Updates an existing amenity.
        /// </summary>
        /// <param name="id">The unique identifier of the amenity to update.</param>
        /// <param name="request">The updated amenity details.</param>
        /// <returns>No content if the update is successful.</returns>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateAmenity(Guid id, [FromBody] AmenityUpdateRequest request)
        {
            await _amenityService.UpdateAmenityAsync(id, request);
            return NoContent();
        }
    }
}
