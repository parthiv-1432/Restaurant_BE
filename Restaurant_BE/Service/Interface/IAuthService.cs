using Restaurant_BE.Dto;

namespace Restaurant_BE.Service.Interface
{
    public interface IAuthService
    {
        Task<BaseResponse> RegisterAsync(RegisterDto request);
        Task<BaseResponse> LoginAsync(LoginDto request);
        Task<BaseResponse> ChangePasswordAsync(string userId, ChangePasswordDto request);
        Task<BaseResponse> ForgotPasswordAsync(ForgotPasswordDto request);
        Task<BaseResponse> ResetPasswordAsync(ResetPasswordDto request);
        Task SendEmailAsync(string to, string subject, string body);
    }
}
