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
            var loaduser = await contextDb.Users
                .Include(u => u.Roles)  // Navigation property won't load by default :)
                .FirstOrDefaultAsync(u => u.Username == model.Username);
            return loaduser;
        }

        public async Task<User> CreateUser(User user, List<string> roles)
        {
            await contextDb.Users.AddAsync(user);
            

            // Assign roles to the user
            foreach (var roleName in roles)
            {
                var role = await contextDb.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                if (role != null)
                {
                    user.Roles.Add(role);
                }
                else
                {
                    role = new Role { Name = roleName };
                    contextDb.Roles.Add(role);
                    user.Roles.Add(role);
                }
            }

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
            var loaduser=await contextDb.Users
                .Include(u => u.Roles)  // Navigation property won't load by default :)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            return loaduser;
        }
    }
}
