using Microsoft.AspNetCore.Mvc;
using StudentManager_BackEnd.Dto;
using Microsoft.AspNetCore.Authorization;
using StudentManager_BackEnd.Service;

namespace Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("Auth")]
        public IActionResult Welcome()
        {
            return Ok("AuthController");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto model)
        {
            try
            {
                var response = await userService.Register(model);
                if (response.IsSuccess)
                {
                    return Ok(response.Message);
                }
                return Conflict(response.Message);
            }
            catch(Exception ex)
            {
                    return StatusCode(500, "An error occurred: " + ex.Message);
            }
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto model)
        {
            try
            {
                var response = await userService.Login(model);
                if (response.IsSuccess)
                {
                    return Ok(response.Message);
                }
                return Unauthorized(response.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(String refreshToken)
        {
            try
            {
                var response = await userService.RefreshToken(refreshToken);
                if (response.IsSuccess)
                {
                    return Ok(response.Message);
                }
                return BadRequest(response.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }
    }
}
