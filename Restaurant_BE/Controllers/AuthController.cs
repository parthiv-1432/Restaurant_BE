using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant_BE.Dto;
using Restaurant_BE.Model;
using Restaurant_BE.Service.Interface;
using System.Security.Claims;

namespace Restaurant_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var response = await _authService.RegisterAsync(request);
            if (response.Status == "error")
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new BaseResponse("error", "Validation failed", ModelState));

            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return BadRequest(new BaseResponse("error", "User not found"));

            var response = await _authService.ChangePasswordAsync(userId, request);

            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new BaseResponse("error", "Invalid request", ModelState));

            var response = await _authService.ForgotPasswordAsync(request);
            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new BaseResponse("error", "Invalid request", ModelState));

            var response = await _authService.ResetPasswordAsync(request);
            return Ok(response);
        }

    }
}