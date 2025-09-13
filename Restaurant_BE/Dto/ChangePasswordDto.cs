using System.ComponentModel.DataAnnotations;

namespace Restaurant_BE.Dto
{
    public class ChangePasswordDto
    {
        [Required] 
        public string CurrentPassword { get; set; }
        [Required] 
        public string NewPassword { get; set; }
        [Required, Compare("NewPassword")] 
        public string ConfirmPassword { get; set; }
    }
}
