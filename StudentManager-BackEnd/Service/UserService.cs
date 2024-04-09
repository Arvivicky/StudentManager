using Dto;
using StudentManager_BackEnd.Dto;
using StudentManager_BackEnd.Entity;
using StudentManager_BackEnd.Repository;
using System.Linq;

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
            var roles = new List<string> {"Student"};
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


            await userRepo.RegisterUser(newUser, roles);
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
            var updateUser = new AddUserDto
            {
                Username = model.Username,
                RefreshToken = refreshToken.Token,
                TokenExpires = refreshToken.Expires,

            };
            await userRepo.UpdateUser(updateUser, user.Id);

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
            var updateUser = new AddUserDto
            {
                Username = user.Username,
                RefreshToken = newRefreshToken.Token,
                TokenExpires = newRefreshToken.Expires,

            };
            await userRepo.UpdateUser(updateUser, user.Id);

            return new StatusMsgDto { IsSuccess = true, Message = "New crds set" };
        }

        public async Task<List<AddUserDto>>GetAllUsers()
        {
            var users=await userRepo.GetAllUsers();
            var userDtoList = new List<AddUserDto>();
            foreach (var user in users)
            {
                userDtoList.Add(await MapUsertoDto(user));
            }
            return userDtoList;
        }
        public async Task<AddUserDto> CreateUser(AddUserDto addUserDto)
        {

            addUserDto.Password = jwtService.HashPassword(addUserDto.Password);
            var user=await userRepo.CreateUser(addUserDto);
            var userDto = await MapUsertoDto(user);
            return userDto;
        }
        public async Task<AddUserDto> UpdateUser(AddUserDto addUserDto,int id)
        {
            if(addUserDto.Password!=null)
            {
                addUserDto.Password = jwtService.HashPassword(addUserDto.Password);
            }
            var user = await userRepo.UpdateUser(addUserDto,id);
            
            return await MapUsertoDto(user);
        }
        public async Task<AddUserDto>MapUsertoDto(User user)
        {
            
            if (user == null)
            {
                return null;
            }

            return new AddUserDto
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                RefreshToken = user.RefreshToken,
                TokenExpires = user.TokenExpires,
                Roles = user.Roles.Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    RoleMenus = r.RoleMenus.Select(rm => new RoleMenuDto
                    {
                        Id = rm.Id,
                        Method = rm.Method,
                        Url = rm.Url
                    }).ToList()
                }).ToList()
            };
        }
    }

}
