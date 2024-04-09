using Microsoft.AspNetCore.Mvc;
using StudentManager_BackEnd.Dto;

namespace StudentManager_BackEnd.Service
{
    public interface IUserService
    {
        public Task<StatusMsgDto> Register(UserDto model);
        public Task<StatusMsgDto> Login(UserDto model);
        public Task<StatusMsgDto> RefreshToken(String refreshToken);
    }
}
