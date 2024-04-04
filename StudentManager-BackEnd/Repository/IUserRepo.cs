using Dto;
using StudentManager_BackEnd.Dto;
using StudentManager_BackEnd.Entity;

namespace StudentManager_BackEnd.Repository
{
    public interface IUserRepo
    {
        public Task<User> LoadUser(UserDto model);
        public Task<User> LoadRefreshToken(RefreshTokenDto refreshTokenDto);
        public Task<User> CreateUser(User user);
        public Task<User> UpdateUser(User user,int Id);
        public Task DeleteUser(int Id);
        

    }
}
