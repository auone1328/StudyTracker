
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Services
{
    public interface IJwtProvider
    {
        public int AccessTokenExpirationTime { get; set; }
        public int CookiesTokenExpirationTime { get; set; }
        public string GenerateAccessToken(UserDTO user);
        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
