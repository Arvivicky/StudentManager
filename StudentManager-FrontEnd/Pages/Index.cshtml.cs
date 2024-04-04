using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using StudentManager_BackEnd.Dto;

namespace StudentManager_FrontEnd.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly HttpClient _httpClient;

        [BindProperty]
        public UserDto UserDto { get; set; }

        public LoginModel(ILogger<LoginModel> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var requestData = new
            {
                Username = UserDto.Username,
                Password = UserDto.Password
            };
            var json = System.Text.Json.JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8,"application/json");
            var response = await _httpClient.PostAsync("https://localhost:7089/api/Auth/login", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Login successful!";
                string responseBody = await response.Content.ReadAsStringAsync();
                // Pass the string to the view
                ViewData["ResponseData"] = responseBody;

            }
            else
            {
                TempData["Message"] = "Login failed. Please try again.";  
            }
            return Page();


        }

    }
}
