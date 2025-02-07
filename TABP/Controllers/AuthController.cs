using Application.Contracts;
using Application.DTOs.Users;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace TABP.Controllers;
/// <summary>
/// Controller responsible for user authentication and registration.
/// </summary>
[Route("api/auth")]
[ApiController]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="userService">The service handling user authentication and registration.</param>
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Processes a login request.
    /// </summary>
    /// <param name="loginRequest">The login request containing user email and password.</param>
    /// <returns>A <see cref="LoginResponse"/> with authentication details if successful.</returns>
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
    /// Processes a guest registration request.
    /// </summary>
    /// <param name="registerRequest">The registration request containing user details.</param>
    /// <returns>An HTTP 204 response if registration is successful.</returns>
    [HttpPost("register-guest")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterUser(RegisterRequest registerRequest)
    {
        try
        {
            await _userService.RegisterGuestAsync(registerRequest);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
