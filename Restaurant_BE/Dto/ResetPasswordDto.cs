using System.ComponentModel.DataAnnotations;

namespace Restaurant_BE.Dto
{
    public class ResetPasswordDto
    {
        [Required, EmailAddress] public string Email { get; set; }
        [Required] public string Otp { get; set; }
        [Required] public string NewPassword { get; set; }
        [Required, Compare("NewPassword")] public string ConfirmPassword { get; set; }
    }
}
