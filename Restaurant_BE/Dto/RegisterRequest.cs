namespace Restaurant_BE.Dto
{
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Role { get; set; } 
    }
}
