using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManager_BackEnd.Dto;
namespace StudentManager_FrontEnd.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public UserDto UserDto { get; set; }

        public RegisterModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult>OnPostAsync()
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
            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7089/api/Auth/register", content);
            if (response.IsSuccessStatusCode)
            {

                TempData["Message"] = "Registration successful!";
                return RedirectToPage("/Index");
            }
            else
            {
                TempData["Message"] = await response.Content.ReadAsStringAsync();
                return Page();
            }

            
        }
    }

}
