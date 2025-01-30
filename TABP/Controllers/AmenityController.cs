using Application.Contracts;
using Application.DTOs.Amenities;
using Asp.Versioning;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TABP.Controllers
{
    [ApiController]
    [Route("api/amenities")]
    [ApiVersion("1.0")]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;

        public AmenityController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AmenityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AmenityResponse>>> GetAmenities([FromQuery] AmenitiesGetRequest request)
        {
            var amenities = await _amenityService.GetAmenitiesAsync(request);
            return Ok(amenities);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(AmenityResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AmenityResponse>> GetAmenity(Guid id)
        {
            var amenity = await _amenityService.GetAmenityByIdAsync(id);
            return Ok(amenity);
        }

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
