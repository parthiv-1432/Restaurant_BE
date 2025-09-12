using Microsoft.AspNetCore.Identity;

namespace Restaurant_BE.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Role { get; set; }
    }
}
