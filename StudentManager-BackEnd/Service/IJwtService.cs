using Dto;
using StudentManager_BackEnd.Entity;

namespace StudentManager_BackEnd.Service
{
    public interface IJwtService
    {
        public string GenerateJwtToken(User user);
        public bool VerifyPassword(string password, string hashedPassword);
        public string HashPassword(string password);
        public RefreshTokenDto GenerateRefreshToken();

    }
}
