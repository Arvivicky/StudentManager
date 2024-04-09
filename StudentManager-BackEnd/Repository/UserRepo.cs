using Dto;
using Entity;
using Microsoft.EntityFrameworkCore;
using StudentManager_BackEnd.Dto;
using StudentManager_BackEnd.Entity;
using System.Data;

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
                .Include(u => u.Roles)  // Include Roles navigation property
                .ThenInclude(r => r.RoleMenus)  // Include RoleMenus navigation property of each Role
                .FirstOrDefaultAsync(u => u.Username == model.Username);
            return loaduser;
        }
        public async Task<User> RegisterUser(User user, List<string> roles)
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
        public async Task<User> UpdateUser(AddUserDto updateUserDto, int Id)
        {
            var user = await contextDb.Users
                .Include(u => u.Roles)  // Include Roles navigation property
                    .ThenInclude(r => r.RoleMenus)  // Include RoleMenus navigation property of each Role
                .FirstOrDefaultAsync(u => u.Id == Id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if(updateUserDto.Username!=null)
            {
                user.Username = updateUserDto.Username;
            }
            if(updateUserDto.Password!=null)
            {
                user.Password = updateUserDto.Password;
            }
            if(updateUserDto.RefreshToken!=null)
            {
                user.RefreshToken = updateUserDto.RefreshToken;
            }
            if(updateUserDto.TokenExpires!=DateTime.MinValue)
            {
                user.TokenExpires = updateUserDto.TokenExpires;
            }

            if (updateUserDto.Roles != null && updateUserDto.Roles.Any())
            {
                foreach (var roleDto in updateUserDto.Roles)
                {
                    var role = await contextDb.Roles
                        .Include(r => r.RoleMenus)
                        .FirstOrDefaultAsync(r => r.Name == roleDto.Name);
                    if (role != null)
                    {
                        role.RoleMenus.Clear();
                        if (roleDto.RoleMenus != null && roleDto.RoleMenus.Any())
                        {
                            foreach (var roleMenuDto in roleDto.RoleMenus)
                            {
                                var roleMenu = new RoleMenu { Method = roleMenuDto.Method, Url = roleMenuDto.Url };
                                role.RoleMenus.Add(roleMenu);
                            }
                        }
                        user.Roles.Add(role);
                    }
                    else
                    {
                        role = new Role { Name = roleDto.Name };
                        if (roleDto.RoleMenus != null && roleDto.RoleMenus.Any())
                        {
                            role.RoleMenus = new List<RoleMenu>();
                            foreach (var roleMenuDto in roleDto.RoleMenus)
                            {
                                var roleMenu = new RoleMenu { Method = roleMenuDto.Method, Url = roleMenuDto.Url };
                                role.RoleMenus.Add(roleMenu);
                            }
                        }
                        contextDb.Roles.Add(role);
                        user.Roles.Add(role);
                    }
                }
            }
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
        public async Task<List<User>> GetAllUsers()
        {
            var users=await contextDb.Users
                .Include(u => u.Roles)  // Navigation property won't load by default :)
                .ThenInclude(r=>r.RoleMenus)
                .ToListAsync();
            return users;
        }
        public async Task<User> CreateUser(AddUserDto addUserDto)
        {
            var loaduser = await contextDb.Users
                .FirstOrDefaultAsync(u => u.Username == addUserDto.Username);
            if(loaduser!= null)
            {
                throw new Exception("Username already exist");
            }
            var user = new User
            {
                Username = addUserDto.Username,
                Password = addUserDto.Password,
            };
            await contextDb.Users.AddAsync(user);

            foreach (var roleDto in addUserDto.Roles)
            {
                var role = await contextDb.Roles.Include(r => r.RoleMenus)
                                                 .FirstOrDefaultAsync(r => r.Name == roleDto.Name);

                if (role == null)
                {
                    role = new Role { Name = roleDto.Name };

                    foreach (var roleMenuDto in roleDto.RoleMenus)
                    {
                        var roleMenu = new RoleMenu { Method = roleMenuDto.Method, Url = roleMenuDto.Url };
                        role.RoleMenus.Add(roleMenu);
                    }
                    contextDb.Roles.Add(role);
                }
                else
                {
                    foreach (var roleMenuDto in roleDto.RoleMenus)
                    {
                        var existingRoleMenu = role.RoleMenus.FirstOrDefault(rm => rm.Method == roleMenuDto.Method && rm.Url == roleMenuDto.Url);

                        if (existingRoleMenu == null)
                        {
                            var roleMenu = new RoleMenu { Method = roleMenuDto.Method, Url = roleMenuDto.Url };
                            role.RoleMenus.Add(roleMenu);
                        }
                    }
                }
                user.Roles.Add(role);
            }

            await contextDb.SaveChangesAsync();
            return user;
        }
    }
}
