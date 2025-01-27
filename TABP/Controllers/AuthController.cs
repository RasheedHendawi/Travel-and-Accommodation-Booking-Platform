using Application.Contracts;
using Application.Users.Models;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TABP.DTOs.Users;

namespace TABP.Controllers;

[Route("api/auth")]
[ApiController]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public AuthController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    /// <summary>
    /// Processes a login request.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
    {
        try
        {
            var response = await _userService.LoginAsync(loginRequest.Email, loginRequest.Password);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    /// <summary>
    /// Processes registering a guest request.
    /// </summary>
    [HttpPost("register-guest")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterUser(RegisterRequest registerRequest)
    {
        try
        {
            var registerGuestRequest = _mapper.Map<RegisterHandler>(registerRequest);
            await _userService.RegisterGuestAsync(registerGuestRequest);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
