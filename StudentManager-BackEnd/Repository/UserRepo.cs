using Dto;
using Entity;
using Microsoft.EntityFrameworkCore;
using StudentManager_BackEnd.Dto;
using StudentManager_BackEnd.Entity;

namespace StudentManager_BackEnd.Repository
{
    public class UserRepo:IUserRepo
    {
        private readonly ContextDb contextDb;

        public UserRepo(ContextDb contextDb)
        {
            this.contextDb = contextDb;
        }
        public async Task<User> LoadUser(UserDto model)
        {
            var loaduser = await contextDb.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            return loaduser;
        }

        public async Task<User> CreateUser(User user)
        {
            await contextDb.Users.AddAsync(user);
            await contextDb.SaveChangesAsync();
            return user;
        }
        public async Task<User> UpdateUser(User user, int Id)
        {
            await contextDb.SaveChangesAsync();
            return user;
        }
        public Task DeleteUser(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> LoadRefreshToken(String refreshToken)
        {
            var loaduser=await contextDb.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            return loaduser;
        }
    }
}
