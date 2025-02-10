using Application.Contracts;
using Application.DTOs.Owners;
using Application.DTOs.Shared;
using Asp.Versioning;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Utilites;

namespace TABP.Controllers
{
    /// <summary>
    /// Handles operations related to hotel owners.
    /// </summary>
    [ApiController]
    [Route("api/owners")]
    [Authorize(Roles = UserRoles.Admin)]
    [ApiVersion("1.0")]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        private readonly IMapper _mapper;

        public OwnersController(IOwnerService ownerService, IMapper mapper)
        {
            _ownerService = ownerService;
            _mapper = mapper;
        }
        /// <summary>
        /// Retrieves a list of owners with optional filtering and pagination.
        /// </summary>
        /// <param name="ownersGetRequest">The request parameters for pagination and filtering.</param>
        /// <returns>A list of owners based on the provided filters.</returns>
        /// <response code="200">Returns the request page of owners, with pagination metadata included.</response>
        /// <response code="400">If the request parameters are invalid or missing.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OwnersResponse>>> GetOwners(
            [FromQuery] OwnersGetRequest ownersGetRequest)
        {
            var owners = await _ownerService.GetOwnersAsync(ownersGetRequest);
            Response.Headers.AddPaginationData(owners.PaginationMetadata);

            return Ok(owners.Items);
        }
        /// <summary>
        /// Retrieves a specific owner by ID.
        /// </summary>
        /// <param name="id">The ID of the owner to retrieve.</param>
        /// <returns>The owner with the specified ID.</returns>
        /// <response code="200">Returns the owner with the given ID.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the owner with the given ID is not found.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OwnersResponse>> GetOwner(Guid id)
        {
            var owner = await _ownerService.GetOwnerByIdAsync(id);
            return Ok(owner);
        }
        /// <summary>
        /// Creates a new owner.
        /// </summary>
        /// <param name="ownerCreationRequest">The data required to create a new owner.</param>
        /// <returns>The created owner with a 201 status code.</returns>
        /// <response code="201">If the owner was created successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPost]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerCreationRequest ownerCreationRequest)
        {
            var createdOwner = await _ownerService.CreateOwnerAsync(ownerCreationRequest);
            return CreatedAtAction(nameof(GetOwner), new { id = createdOwner.Id }, createdOwner);
        }
        /// <summary>
        /// Updates an existing owner.
        /// </summary>
        /// <param name="id">The ID of the owner to update.</param>
        /// <param name="ownerUpdateRequest">The updated data for the owner.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the owner is updated successfully.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">User is not authorized (not an admin).</response>
        /// <response code="404">If the owner is not found.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateOwner(Guid id, [FromBody] OwnerUpdateRequest ownerUpdateRequest)
        {
            await _ownerService.UpdateOwnerAsync(id, ownerUpdateRequest);
            return NoContent();
        }
    }
}
