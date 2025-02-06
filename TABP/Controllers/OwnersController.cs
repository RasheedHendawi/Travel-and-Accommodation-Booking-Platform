using Application.Contracts;
using Application.DTOs.Owners;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Utilites;

namespace TABP.Controllers
{
    [ApiController]
    [Route("api/owners")]
    //[Authorize(Roles = UserRoles.Admin)]
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
