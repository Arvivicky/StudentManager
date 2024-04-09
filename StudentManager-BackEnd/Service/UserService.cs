using StudentManager_BackEnd.Dto;
using StudentManager_BackEnd.Entity;
using StudentManager_BackEnd.Repository;

namespace StudentManager_BackEnd.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo userRepo;
        private readonly IJwtService jwtService;

        public UserService(IUserRepo userRepo, IJwtService jwtService)
        {
            this.userRepo = userRepo;
            this.jwtService = jwtService;
        }
        public async Task<StatusMsgDto> Register(UserDto model)
        {
            var roles = new List<string> { "Admin" };
            var user = await userRepo.LoadUser(model);
            if (user != null)
            {
                return new StatusMsgDto { IsSuccess = false, Message = "Username already exists" };
            }

            var newUser = new User
            {
                Username = model.Username,
                Password = jwtService.HashPassword(model.Password)
            };

            await userRepo.CreateUser(newUser, roles);
            return new StatusMsgDto { IsSuccess = true, Message = "User Created.!"};
        }
        public async Task<StatusMsgDto> Login(UserDto model)
        {
            var user = await userRepo.LoadUser(model);
            if (user == null)
                return new StatusMsgDto { IsSuccess = false, Message = "Invalid Username" };
            if (!jwtService.VerifyPassword(model.Password, user.Password))
                return new StatusMsgDto { IsSuccess = false, Message = "Invalid Password" };

            var accessToken = jwtService.GenerateJwtToken(user);
            var refreshToken = jwtService.GenerateRefreshToken();
            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.TokenCreated;
            user.TokenExpires = refreshToken.Expires;
            await userRepo.UpdateUser(user, user.Id);

            return new StatusMsgDto { IsSuccess = true, Message = "Login success.!"};
        }
        public async Task<StatusMsgDto> RefreshToken(String refreshToken)
        {
            var user = await userRepo.LoadRefreshToken(refreshToken);
            if (user == null)
                return new StatusMsgDto { IsSuccess = false, Message = "Invalid refresh token" };

            if (user.TokenExpires < DateTime.UtcNow)
                return new StatusMsgDto { IsSuccess = false, Message = "Token Expired" };

            var accessToken = jwtService.GenerateJwtToken(user);
            var newRefreshToken = jwtService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.TokenCreated;
            user.TokenExpires = newRefreshToken.Expires;
            await userRepo.UpdateUser(user, user.Id);

            return new StatusMsgDto { IsSuccess = true, Message = "New crds set" };
        }
    }

}
