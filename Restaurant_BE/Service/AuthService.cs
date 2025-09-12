using Microsoft.AspNetCore.Identity;
using Restaurant_BE.Dto;
using Restaurant_BE.Model;
using Restaurant_BE.Service.Interface;

namespace Restaurant_BE.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<BaseResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return new BaseResponse("error", "Email already exists");

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

            // Create Role if not exists
            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(request.Role));
            }

            // Assign Role
            await _userManager.AddToRoleAsync(user, request.Role);

            return new BaseResponse("success", "User registered successfully", new { user.Id, user.Email, user.Role });
        }
    }
}
