using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTOs
{
    public record LoginUserRequest(
      [Required] string Email,
      [Required] string Password);
}
