using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Dto;
using Entity;
using StudentManager_BackEnd.Dto;
using StudentManager_BackEnd.Entity;
using StudentManager_BackEnd.Service;
using StudentManager_BackEnd.Repository;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ContextDb _context;
        private readonly IConfiguration _configuration;
        private readonly IJwtService jwtService;
        private readonly IUserRepo userRepo;

        public AuthController(ContextDb context, IConfiguration configuration,
            IJwtService jwtService,IUserRepo userRepo)
        {
            _context = context;
            _configuration = configuration;
            this.jwtService = jwtService;
            this.userRepo = userRepo;
        }

        [HttpGet("Auth")]
        public IActionResult Welcome()
        {
            return Ok("AuthController");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto model)
        {
            var user=await userRepo.LoadUser(model);
            if (user!= null)
            {
                return Conflict("Username already exists");
            }
            
            var newUser = new User
            {
                Username = model.Username,
                Password = jwtService.HashPassword(model.Password)
            };

            user=await userRepo.CreateUser(newUser);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto model)
        {
            var user = await userRepo.LoadUser(model);

            if (user == null || !jwtService.VerifyPassword(model.Password, user.Password))
                return Unauthorized("Invalid username or password");

            var accessToken = jwtService.GenerateJwtToken(user);
            var refreshToken = jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await userRepo.UpdateUser(user, user.Id);

            return Ok(new { Token = accessToken, RefreshToken = refreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var user = await userRepo.LoadRefreshToken(refreshTokenDto);
            if (user == null)
                return BadRequest("Invalid refresh token");

            var accessToken = jwtService.GenerateJwtToken(user);
            var newRefreshToken = jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await userRepo.UpdateUser(user, user.Id);

            return Ok(new { Token = accessToken, RefreshToken = newRefreshToken });
        }
    }
}
