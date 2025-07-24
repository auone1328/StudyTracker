namespace WebApplication1.Models.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? Role { get; set; }
    }
}
