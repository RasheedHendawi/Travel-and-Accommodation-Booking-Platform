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

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HotelGetFromManagment>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<HotelGetFromManagment>>> GetHotelsForManagement([FromQuery] HotelGetRequest request)
        {
            var hotels = await _hotelsService.GetHotelsForManagementAsync(request);
            Response.Headers.AddPaginationData(hotels.PaginationMetadata);
            return Ok(hotels.Items);
        }

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

        [HttpGet("featured-deals")]
        [ProducesResponseType(typeof(IEnumerable<HotelFeaturedDealResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HotelFeaturedDealResponse>>> GetFeaturedDeals([FromQuery] int count)
        {
            var featuredDeals = await _hotelsService.GetFeaturedDealsAsync(count);
            return Ok(featuredDeals);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(HotelGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelGetResponse>> GetHotel(Guid id)
        {
            var hotel = await _hotelsService.GetHotelByIdAsync(id);
            return Ok(hotel);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateHotel([FromBody] HotelCreationRequest request)
        {
            var hotelId = await _hotelsService.CreateHotelAsync(request);
            return CreatedAtAction(nameof(GetHotel), new { id = hotelId }, null);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateHotel(Guid id, [FromBody] HotelUpdateRequest request)
        {
            await _hotelsService.UpdateHotelAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteHotel(Guid id)
        {
            await _hotelsService.DeleteHotelAsync(id);
            return NoContent();
        }

        [HttpPut("{id:guid}/thumbnail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetHotelThumbnail(Guid id, [FromForm] ImageCreationRequest image)
        {
            await _hotelsService.SetHotelThumbnailAsync(id, image);
            return NoContent();
        }

        [HttpPost("{id:guid}/gallery")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddImageToHotelGallery(Guid id, [FromForm] ImageCreationRequest image)
        {
            await _hotelsService.AddImageToGalleryAsync(id, image);
            return NoContent();
        }
    }
}
