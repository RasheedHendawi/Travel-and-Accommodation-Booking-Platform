using Application.Contracts;
using Application.DTOs.Rooms;
using Asp.Versioning;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Utilites;

namespace TABP.Controllers
{

    [ApiController]
    [Route("api/room-classes/{roomClassId:guid}/rooms")]
    [ApiVersion("1.0")]
    [Authorize(Roles = UserRoles.Admin)]
    public class RoomsController(IRoomService roomService) : ControllerBase
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomForManagementResponse>>> GetRoomsForManagement(
            Guid roomClassId,
            [FromQuery] RoomsGetRequest request)
        {
            var rooms = await roomService.GetRoomsForManagementAsync(roomClassId, request);
            Response.Headers.AddPaginationData(rooms.PaginationMetadata);

            return Ok(rooms.Items);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RoomForGuestResponse>>> GetRoomsForGuests(
            Guid roomClassId,
            [FromQuery] RoomsForGuestsGetRequest request)
        {
            var rooms = await roomService.GetRoomsForGuestsAsync(roomClassId, request);
            Response.Headers.AddPaginationData(rooms.PaginationMetadata);

            return Ok(rooms.Items);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<IActionResult> CreateRoomInRoomClass(
            Guid roomClassId,
            RoomCreationRequest request)
        {
            await roomService.CreateRoomAsync(roomClassId, request);
            return Created();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRoomInRoomClass(
            Guid roomClassId, Guid id,
            RoomUpdateRequest request)
        {
            await roomService.UpdateRoomAsync(roomClassId, id, request);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRoomInRoomClass(Guid roomClassId, Guid id)
        {
            await roomService.DeleteRoomAsync(roomClassId, id);
            return NoContent();
        }
    }
}