using Application.Contracts;
using Application.DTOs.Images;
using Application.DTOs.RoomClass;
using Asp.Versioning;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Utilites;

namespace TABP.Controllers
{
    /// <summary>
    /// Controller for managing room classes.
    /// </summary>
    [ApiController]
    [Route("api/room-classes")]
    [ApiVersion("1.0")]
    [Authorize(Roles = UserRoles.Admin)]
    public class RoomClassController(IRoomClassService roomClassService) : ControllerBase
    {
        private readonly IRoomClassService _roomClassService = roomClassService;
        /// <summary>
        /// Retrieves all room classes with optional filtering and pagination.
        /// </summary>
        /// <param name="resourcesQueryRequest">Query parameters for filtering and pagination.</param>
        /// <returns>A list of room classes.</returns>
        /// <response code="200">Returns the request page of room classes for management, with pagination metadata included.</response>
        /// <response code="400">If the request parameters are invalid or missing.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
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
        /// <summary>
        /// Retrieves a specific room class by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the room class.</param>
        /// <returns>The room class details.</returns>
        /// <response code="200">Returns the requested room class</response>
        /// <response code="400">If the request parameters are invalid or missing.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoomClassForManagementResponse>> GetRoomClassById(Guid id)
        {
            var roomClass = await _roomClassService.GetRoomClassByIdAsync(id);
            return Ok(roomClass);
        }
        /// <summary>
        /// Creates a new room class.
        /// </summary>
        /// <param name="roomClass">The details of the new room class.</param>
        /// <returns>The created room class.</returns>
        /// <response code="201">If the room class was created successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If one of the provided amenities is not found.</response>
        /// <response code="409">If there is a room class with the same name in the same hotel.</response>
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
        /// <summary>
        /// Updates an existing room class.
        /// </summary>
        /// <param name="id">The unique identifier of the room class.</param>
        /// <param name="updatedRoomClass">The updated room class details.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the room class was updated successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the room class was not found.</response>
        /// <response code="409">If there is a room class with the same name in the same hotel.</response>
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
        /// <summary>
        /// Deletes a room class by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the room class.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the room class was deleted successfully.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the room class was not found.</response>
        /// <response code="409">If there are rooms in the room class.</response>
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
        /// <summary>
        /// Adds an image to a specific room class gallery.
        /// </summary>
        /// <param name="id">The unique identifier of the room class.</param>
        /// <param name="image">The image details to be added.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the image was added to the gallery successfully.</response>
        /// <response code="400">If the image is invalid (not .jpg, .jpeg, or .png).</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the room class specified by ID is not found.</response>
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
