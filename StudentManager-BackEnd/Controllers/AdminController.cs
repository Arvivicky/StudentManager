using Dto;
using Microsoft.AspNetCore.Mvc;
using StudentManager_BackEnd.Service;

namespace StudentManager_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService userService;

        public AdminController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpGet("welcome")]
        public IActionResult Get()
        {
            return Ok("Admin");
        }

        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await userService.GetAllUsers();
                return Ok(users);

            } catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        [HttpPost("addNewUser")]
        public async Task<IActionResult> CreateUser(AddUserDto addUserDto)
        {
            try
            {
                var user = await userService.CreateUser(addUserDto);
                return Ok(user);

            } catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }
        [HttpPut("updateUser/{int id}")]
        public async Task<IActionResult> UpdateUser(AddUserDto addUserDto, int id)
        {
            try
            {
                var user = await userService.UpdateUser(addUserDto,id);
                return Ok(user);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }
    }
}
