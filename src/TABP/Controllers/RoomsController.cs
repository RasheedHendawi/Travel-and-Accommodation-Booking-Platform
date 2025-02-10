using Application.Contracts;
using Application.DTOs.Rooms;
using Asp.Versioning;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Utilites;

namespace TABP.Controllers
{
    /// <summary>
    /// Controller for managing rooms within a room class.
    /// </summary>
    [ApiController]
    [Route("api/room-classes/{roomClassId:guid}/rooms")]
    [ApiVersion("1.0")]
    [Authorize(Roles = UserRoles.Admin)]
    public class RoomsController(IRoomService roomService) : ControllerBase
    {
        /// <summary>
        /// Retrieves a paginated list of rooms for management.
        /// </summary>
        /// <param name="roomClassId">The ID of the room class.</param>
        /// <param name="request">Query parameters for filtering rooms.</param>
        /// <returns>A list of rooms for management.</returns>
        /// <response code="200">Returns the request page of rooms, with pagination metadata included.</response>
        /// <response code="400">If the request parameters are invalid or missing.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the room class is not found.</response>
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
        /// <summary>
        /// Retrieves a paginated list of available rooms for guests.
        /// </summary>
        /// <param name="roomClassId">The ID of the room class.</param>
        /// <param name="request">Query parameters for available rooms.</param>
        /// <returns>A list of available rooms for guests.</returns>
        /// <response code="200">Returns the request page of available rooms, with pagination metadata included.</response>
        /// <response code="400">If the request parameters are invalid or missing.</response>
        /// <response code="404">If the room class is not found.</response>
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
        /// <summary>
        /// Creates a new room within a specified room class.
        /// </summary>
        /// <param name="roomClassId">The ID of the room class.</param>
        /// <param name="request">The room creation request.</param>
        /// <response code="201">If the room was created successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the room class is not found.</response>
        /// <response code="409">If there is a room with the same number in the room class of the room class.</response>
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
        /// <summary>
        /// Updates an existing room within a room class.
        /// </summary>
        /// <param name="roomClassId">The ID of the room class.</param>
        /// <param name="id">The ID of the room to update.</param>
        /// <param name="request">The room update request.</param>
        /// <response code="204">If the room was updated successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the room class is not found or room is not found in room class.</response>
        /// <response code="409">If there is a room with the same number in the room class of the room class.</response>
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
        /// <summary>
        /// Deletes a room from a room class.
        /// </summary>
        /// <param name="roomClassId">The ID of the room class.</param>
        /// <param name="id">The ID of the room to delete.</param>
        /// <response code="204">If the room was deleted successfully.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the room class is not found or room is not found in room class.</response>
        /// <response code="409">If there are bookings to the room.</response>
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