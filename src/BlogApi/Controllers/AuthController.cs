using Amazon.CognitoIdentityProvider.Model;
using BlogApi.Models;
using BlogApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("signup")]
    public async Task<ActionResult> SignUp(Models.SignUpRequest request)
    {
        try
        {
            await _authService.SignUpAsync(request.Email, request.Password);
            return Ok(new { message = "SignUp success. Check your e-mail to continue" });
        }
        catch (InvalidPasswordException)
        {
            return BadRequest(new { message = "Password does not meet security requirements." });
        }
        catch (UsernameExistsException)
        {
            return Conflict(new { message = "An account with this email already exists." });
        }
    }

    [HttpPost("confirm")]
    public async Task<ActionResult> Confirm(ConfirmRequest request)
    {
        await _authService.ConfirmSignUpAsync(request.Email, request.Code);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request.Email, request.Password);

        return Ok(new { result.IdToken, result.AccessToken, result.RefreshToken });
    }
}




