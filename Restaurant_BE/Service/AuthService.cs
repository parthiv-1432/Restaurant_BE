using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Restaurant_BE.Dto;
using Restaurant_BE.Model;
using Restaurant_BE.Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace Restaurant_BE.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private static readonly Dictionary<string, string> _otpStore = new();

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = config;
        }

        public async Task<BaseResponse> RegisterAsync(RegisterDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return new BaseResponse("error", "Email already exists");

            if (!await _roleManager.RoleExistsAsync(request.Role))
                return new BaseResponse("error", $"Invalid role '{request.Role}'");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name,
                Mobile = request.Mobile,
                Age = request.Age,
                City = request.City,
                Role = request.Role
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return new BaseResponse("error", "Registration failed", result.Errors);

            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(request.Role));
            }

            await _userManager.AddToRoleAsync(user, request.Role);

            return new BaseResponse("success", "User registered successfully", new { user.Id, user.Email, user.Role });
        }

        public async Task<BaseResponse> LoginAsync(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new BaseResponse("error", "Invalid email or password");

            var checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!checkPassword)
                return new BaseResponse("error", "Invalid email or password");

            var roles = await _userManager.GetRolesAsync(user);

            var token = GenerateJwtToken(user, roles.FirstOrDefault());

            return new BaseResponse("success", "Login successful", new
            {
                token,
                user.Id,
                user.Email,
                Role = roles.FirstOrDefault()
            });
        }

        private string GenerateJwtToken(ApplicationUser user, string role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),   
                new Claim(ClaimTypes.Role, role ?? "User"),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<BaseResponse> ChangePasswordAsync(string userId, ChangePasswordDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new BaseResponse("error", "User not found");

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
                return new BaseResponse("error", "Password change failed", result.Errors);

            return new BaseResponse("success", "Password changed successfully");
        }

        public async Task<BaseResponse> ForgotPasswordAsync(ForgotPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new BaseResponse("error", "User not found");

            // Generate 6-digit OTP
            var otp = new Random().Next(100000, 999999).ToString();

            // Store OTP against user email (for demo purpose)
            if (_otpStore.ContainsKey(user.Email))
                _otpStore[user.Email] = otp;
            else
                _otpStore.Add(user.Email, otp);

            // Send email
            await SendEmailAsync(user.Email, "Password Reset OTP", GenerateOtpEmailBody(user.Name, otp));

            return new BaseResponse("success", "OTP sent to your email");
        }

        // ✅ Reset password using OTP
        public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new BaseResponse("error", "User not found");

            // Check OTP
            if (!_otpStore.ContainsKey(request.Email) || _otpStore[request.Email] != request.Otp)
                return new BaseResponse("error", "Invalid or expired OTP");

            // Reset password
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            if (!result.Succeeded)
                return new BaseResponse("error", "Password reset failed", result.Errors);

            // Remove OTP after successful reset
            _otpStore.Remove(request.Email);

            return new BaseResponse("success", "Password reset successful");
        }

        // ✅ Email sender
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtp = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:Port"]),
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:Username"],
                    _configuration["EmailSettings:Password"]),
                EnableSsl = true
            };

            await smtp.SendMailAsync(new MailMessage(
                from: _configuration["EmailSettings:From"],
                to: to,
                subject,
                body
            )
            { IsBodyHtml = true });
        }

        // ✅ HTML Email Body
        private string GenerateOtpEmailBody(string userName, string otp)
        {
            return $@"
        <html>
        <body style='font-family:Arial,sans-serif;background:#f5f5f5;padding:20px;'>
            <div style='max-width:600px;margin:auto;background:#fff;padding:30px;border-radius:8px;box-shadow:0 0 10px rgba(0,0,0,0.1)'>
                <h2 style='color:#333;'>Hello {userName},</h2>
                <p>You requested a password reset. Use the OTP below to reset your password. This OTP is valid for a limited time.</p>
                <div style='font-size:24px;font-weight:bold;color:#2E86C1;margin:20px 0;text-align:center;'>{otp}</div>
                <p>If you did not request this, please ignore this email.</p>
                <hr style='border:none;border-top:1px solid #eee;'/>
                <p style='font-size:12px;color:#888;'>© 2025 Restaurant_BE. All rights reserved.</p>
            </div>
        </body>
        </html>";
        }
    }
}