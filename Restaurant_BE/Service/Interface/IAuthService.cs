using Restaurant_BE.Dto;

namespace Restaurant_BE.Service.Interface
{
    public interface IAuthService
    {
        Task<BaseResponse> RegisterAsync(RegisterRequest request);
    }
}
