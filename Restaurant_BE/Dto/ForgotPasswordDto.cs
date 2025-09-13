using System.ComponentModel.DataAnnotations;

namespace Restaurant_BE.Dto
{
    public class ForgotPasswordDto
    {
        [Required, EmailAddress] public string Email { get; set; }
    }
}
