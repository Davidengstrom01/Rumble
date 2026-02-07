using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rumble.Api.Dtos;
using Rumble.Api.Models;
using Rumble.Api.Services;

namespace Rumble.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;
    public AuthController(UserManager<AppUser> userManager, TokenService tokenService)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            return Unauthorized();

        return new UserDto
        {
            Email = user.Email,
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new AppUser
        {
            UserName = registerDto.Username,
            Email = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        return StatusCode(201);
    }
}
