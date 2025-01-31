using Application.Contracts;
using Application.DTOs.Images;
using Application.DTOs.RoomClass;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using TABP.Utilites;

namespace TABP.Controllers
{
    public class RoomClassController(IRoomClassService roomClassService) : ControllerBase
    {
        private readonly IRoomClassService _roomClassService = roomClassService;

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomClassForManagementResponse>>> GetAllRoomClasses(
            [FromQuery] ResourcesQueryRequest resourcesQueryRequest)
        {
            var roomClasses = await _roomClassService.GetAllRoomClassesAsync(resourcesQueryRequest);
            Response.Headers.AddPaginationData(roomClasses.PaginationMetadata);
            return Ok(roomClasses.Items);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoomClassForManagementResponse>> GetRoomClassById(Guid id)
        {
            var roomClass = await _roomClassService.GetRoomClassByIdAsync(id);
            return Ok(roomClass);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<ActionResult> CreateRoomClass(RoomClassCreationRequest roomClass)
        {
            await _roomClassService.CreateRoomClassAsync(roomClass);
            return Created();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRoomClass(Guid id, RoomClassUpdateRequest updatedRoomClass)
        {
            await _roomClassService.UpdateRoomClassAsync(id, updatedRoomClass);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRoomClass(Guid id)
        {
            await _roomClassService.DeleteRoomClassAsync(id);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{id:guid}/gallery")]
        public async Task<IActionResult> AddImageToRoomClass(Guid id, [FromBody] ImageCreationRequest image)
        {
            await _roomClassService.AddImageToRoomClassAsync(id, image);
            return NoContent();
        }
    }
}
