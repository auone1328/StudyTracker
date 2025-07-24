using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? Role { get; set; }
        public virtual ICollection<StudentCourse> StudentsCourse { get; set; }
        public virtual ICollection<StudentAssignment> StudentsAssignment { get; set; }
    }
}
