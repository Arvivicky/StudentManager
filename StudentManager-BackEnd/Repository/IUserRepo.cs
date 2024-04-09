using Dto;
using StudentManager_BackEnd.Dto;
using StudentManager_BackEnd.Entity;

namespace StudentManager_BackEnd.Repository
{
    public interface IUserRepo
    {
        public Task<User> LoadUser(UserDto model);
        public Task<User> LoadRefreshToken(String refreshToken);
        public Task<User> RegisterUser(User user, List<string> roles);
        public Task<User> UpdateUser(AddUserDto addUserDto, int Id);
        public Task DeleteUser(int Id);
        public Task<List<User>> GetAllUsers();
        public Task<User> CreateUser(AddUserDto addUserDto);



    }
}
