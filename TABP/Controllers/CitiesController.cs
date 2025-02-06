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
    [ApiController]
    [Route("api/cities")]
    [ApiVersion("1.0")]
    //[Authorize(Roles = UserRoles.Admin)]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;

        public CitiesController(ICityService cityService, IMapper mapper)
        {
            _cityService = cityService;
            _mapper = mapper;
        }

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

        [ProducesResponseType(typeof(IEnumerable<TrendingCityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("trending")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TrendingCityResponse>>> GetTrendingCities(
            [FromQuery] int count)
        {
            var cities = await _cityService.GetTrendingCitiesAsync(count);
            return Ok(cities);
        }
        
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
