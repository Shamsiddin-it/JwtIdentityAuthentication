using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.AuthService;
using WebApi.DTOs;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenService _jwt;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenService jwt)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email, // важно: логин часто завязан на UserName
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

        var token = await _jwt.CreateTokenAsync(user);
        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
            return Unauthorized(new { message = "Invalid credentials" });

        // Проверка пароля + lockoutOnFailure=true
        var check = await _signInManager.CheckPasswordSignInAsync(
            user, dto.Password, lockoutOnFailure: true);

        if (!check.Succeeded)
            return Unauthorized(new
            {
                message = check.IsLockedOut ? "Locked out (too many attempts)" : "Invalid credentials"
            });

        var token = await _jwt.CreateTokenAsync(user);
        return Ok(new { token });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
        => Ok(new { message = "JWT logout: delete token on client" });

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
        => Ok(new
        {
            name = User.Identity?.Name,
            claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
}