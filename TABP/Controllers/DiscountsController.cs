using Application.Contracts;
using Application.DTOs.Discounts;
using Asp.Versioning;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Utilites;

namespace TABP.Controllers
{
    [ApiController]
    [Route("api/room-classes/{roomClassId:guid}/discounts")]
    [ApiVersion("1.0")]
    [Authorize(Roles = UserRoles.Admin)]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DiscountResponse>> GetDiscount(Guid roomClassId, Guid id)
        {
            var discount = await _discountService.GetDiscountByIdAsync(roomClassId, id);
            return Ok(discount);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<IActionResult> CreateDiscount(Guid roomClassId, DiscountCreationRequest request)
        {
            var createdDiscount = await _discountService.CreateDiscountAsync(roomClassId, request);
            return CreatedAtAction(nameof(GetDiscount), new { roomClassId, id = createdDiscount.Id },
                createdDiscount); 
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDiscount(Guid roomClassId, Guid id)
        {
            await _discountService.DeleteDiscountAsync(roomClassId, id);
            return NoContent();
        }
    }
}
    
