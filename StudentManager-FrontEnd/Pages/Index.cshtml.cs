using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentManager_BackEnd.Dto;
using StudentManager_FrontEnd.Dto;

namespace StudentManager_FrontEnd.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;

        [BindProperty]
        public UserDto UserDto { get; set; }

        public LoginModel(ILogger<LoginModel> logger, HttpClient httpClient,IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpClient = httpClient;
            this.httpContextAccessor = httpContextAccessor;
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
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7089/api/Auth/login", content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Login successful!";
                ViewData["ResponseData"] = responseBody;

                // Extract cookies from the response headers
                var cookies = response.Headers.GetValues("Set-Cookie");

                // Set cookies in the browser's cookie storage
                foreach (var cookie in cookies)
                {
                    var cookieParts = cookie.Split(';')[0].Split('=');
                    var cookieName = cookieParts[0];
                    var cookieValue = cookieParts[1];
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax,
                    };

                    // Add cookie to the response headers
                    httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, cookieValue,cookieOptions);
                }
            }
            else
            {
                TempData["Message"] = responseBody;
            }

            return Page();
        }


        public IActionResult OnGet()
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete("Jwt");
            httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
            return Page();
        }

    }
}
