using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.EmailService;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailTestingController(IEmailService service, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly IEmailService _emailService = service;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [Authorize(Policy = "SendEmailPolicy")]
        [HttpPost]
        public async Task<IActionResult> SendTestEmail()
        {
            await _emailService.SendAsync("samsiddinarbobov@gmail.com",
            "Testing SMTP",
            "Salom, if you are reading this, it is ok!");
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("add-permission")]
        public async Task<IActionResult> AddPermissionToUser(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                await _userManager.AddClaimAsync(user,
                    new Claim("Permission", "SendEmail"));
            return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("debug/claims")]
        public IActionResult DebugClaims()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
        
            if (user == null)
                return Ok();
        
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
            var link = $"http://localhost:5264/api/MailTesting/reset-password?email={dto.Email}&token={Uri.EscapeDataString(token)}";
        
            await _emailService.SendAsync(dto.Email,
                "Reset Password",
                $"Reset link: {link}");
        
            return Ok();
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            var result = await _userManager.ResetPasswordAsync(
                user,
                dto.Token,
                dto.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }
    }
}
