using Application.Contracts;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Cities;
using Application.DTOs.Images;
using TABP.Utilites;
using Domain.Models;

namespace TABP.Controllers
{
    /// <summary>
    /// Controller for managing city-related operations.
    /// </summary>
    [ApiController]
    [Route("api/cities")]
    [ApiVersion("1.0")]
    [Authorize(Roles = UserRoles.Admin)]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;
        /// <summary>
        /// Initializes a new instance of the <see cref="CitiesController"/> class.
        /// </summary>
        /// <param name="cityService">Service for city operations.</param>
        /// <param name="mapper">Mapper instance.</param>
        public CitiesController(ICityService cityService, IMapper mapper)
        {
            _cityService = cityService;
        }
        /// <summary>
        /// Retrieves a list of cities for management.
        /// </summary>
        /// <param name="request">Filter parameters for retrieving cities.</param>
        /// <returns>A list of cities for management with pagination metadata.</returns>
        /// <response code="200">Returns the request page of cities for management, with pagination metadata included.</response>
        /// <response code="400">If the request parameters are invalid or missing.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        [ProducesResponseType(typeof(IEnumerable<CityForManagementResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityForManagementResponse>>> GetCitiesForManagement(
            [FromQuery] CitiesGetHandler request)
        {
            var cities = await _cityService.GetCitiesForManagementAsync(request);
            Response.Headers.AddPaginationData(cities.PaginationMetadata);

            return Ok(cities.Items);
        }
        /// <summary>
        /// Retrieves the top trending cities.
        /// </summary>
        /// <param name="count">The number of trending cities to retrieve.</param>
        /// <returns>A list of trending cities.</returns>
        /// <response code="200">Returns TOP N most visited cities.</response>
        /// <response code="400">
        ///   If the provided count (N) is negative
        ///   or greater than 100.
        /// </response>
        [ProducesResponseType(typeof(IEnumerable<TrendingCityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("trending")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TrendingCityResponse>>> GetTrendingCities(
            [FromQuery] TrendingCityRequest request)
        {
            var cities = await _cityService.GetTrendingCitiesAsync(request);
            return Ok(cities);
        }
        /// <summary>
        /// Creates a new city.
        /// </summary>
        /// <param name="request">City creation details.</param>
        /// <returns>The newly created city.</returns>
        /// <response code="201">If the city was created successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="409">If a city with the same post office exists.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<IActionResult> CreateCity(CityCreationRequest request)
        {
            await _cityService.CreateCityAsync(request);
            return Created();
        }
        /// <summary>
        /// Updates an existing city.
        /// </summary>
        /// <param name="id">The unique identifier of the city.</param>
        /// <param name="request">Updated city details.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the city was updated successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the city was not found.</response>
        /// <response code="409">If a city with the same post office exists.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCity(Guid id, CityUpdateRequest request)
        {
            await _cityService.UpdateCityAsync(id, request);
            return NoContent();
        }
        /// <summary>
        /// Deletes a city by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the city.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">If the thumbnail was set successfully.</response>
        /// <response code="400">If the thumbnail is invalid (not .jpg, .jpeg, or .png).</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the city specified by ID is not found.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            await _cityService.DeleteCityAsync(id);
            return NoContent();
        }
        /// <summary>
        /// Sets the thumbnail image for a city.
        /// </summary>
        /// <param name="id">The unique identifier of the city.</param>
        /// <param name="request">Image upload request.</param>
        /// <returns>No content if the update is successful.</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:guid}/thumbnail")]
        public async Task<IActionResult> SetCityThumbnail(Guid id, [FromForm] ImageCreationRequest request)
        {
            await _cityService.SetCityThumbnailAsync(id, request);
            return NoContent();
        }
    }
}
