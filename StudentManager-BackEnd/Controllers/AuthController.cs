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
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService jwtService;
        private readonly IUserRepo userRepo;

        public AuthController(IJwtService jwtService,IUserRepo userRepo)
        {
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
            var roles = new List<string>{ "Admin" };
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

            user=await userRepo.CreateUser(newUser, roles);
            return Ok("User Created.!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto model)
        {
            var user = await userRepo.LoadUser(model);
            if (user == null)
                return Unauthorized("Invalid Username");
            if (!jwtService.VerifyPassword(model.Password, user.Password))
                return Unauthorized("Invalid Password");

            var accessToken = jwtService.GenerateJwtToken(user);
            var refreshToken = jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.TokenCreated;
            user.TokenExpires = refreshToken.Expires;
            await userRepo.UpdateUser(user, user.Id);

            return Ok("Login success.!");
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(String refreshToken)
        {
            var user = await userRepo.LoadRefreshToken(refreshToken);
            if (user == null)
                return BadRequest("Invalid refresh token");
            if (user.TokenExpires< DateTime.UtcNow)
                return Unauthorized("Token Expired");

            var accessToken = jwtService.GenerateJwtToken(user);
            var newRefreshToken = jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.TokenCreated;
            user.TokenExpires = newRefreshToken.Expires;
            await userRepo.UpdateUser(user, user.Id);

            return Ok(new { Token = accessToken, RefreshToken = newRefreshToken });
        }
    }
}
