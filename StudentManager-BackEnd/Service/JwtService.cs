using Dto;
using Microsoft.IdentityModel.Tokens;
using StudentManager_BackEnd.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManager_BackEnd.Service
{
    public class JwtService:IJwtService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public JwtService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }
        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("sub", user.Username),
                new Claim(ClaimTypes.Name, user.Username)
            };

            // Add roles as claims
            foreach (var role in user.Roles) // Iterate over user.Roles
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(2),
                signingCredentials: creds
            );
            var jwt= new JwtSecurityTokenHandler().WriteToken(token); ;
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
            };
            httpContextAccessor.HttpContext.Response.Cookies.Append("Jwt",jwt, cookieOptions);

            return jwt;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public RefreshTokenDto GenerateRefreshToken()
        {
            var refreshToken = new RefreshTokenDto
            {
                Token=Guid.NewGuid().ToString(),
                Expires= DateTime.UtcNow.AddDays(7)
            };
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
            };
            httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken",refreshToken.Token,cookieOptions);

            return refreshToken;
        }
    }
}
