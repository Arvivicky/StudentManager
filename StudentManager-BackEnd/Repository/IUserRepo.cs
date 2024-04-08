using Dto;
using StudentManager_BackEnd.Dto;
using StudentManager_BackEnd.Entity;

namespace StudentManager_BackEnd.Repository
{
    public interface IUserRepo
    {
        public Task<User> LoadUser(UserDto model);
        public Task<User> LoadRefreshToken(String refreshToken);
        public Task<User> CreateUser(User user, List<string> roles);
        public Task<User> UpdateUser(User user,int Id);
        public Task DeleteUser(int Id);
        

    }
}
