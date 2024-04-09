using Dto;
using Microsoft.AspNetCore.Mvc;
using StudentManager_BackEnd.Dto;
using StudentManager_BackEnd.Entity;

namespace StudentManager_BackEnd.Service
{
    public interface IUserService
    {
        public Task<StatusMsgDto> Register(UserDto model);
        public Task<StatusMsgDto> Login(UserDto model);
        public Task<StatusMsgDto> RefreshToken(String refreshToken);
        public Task<List<AddUserDto>> GetAllUsers();
        public Task<AddUserDto> CreateUser(AddUserDto addUserDto);
        public Task<AddUserDto> UpdateUser(AddUserDto addUserDto, int id);
    }
}
