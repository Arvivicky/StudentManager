using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using StudentManager_FrontEnd.Dto;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using StudentManager_FrontEnd.Pages.LogoutPage;
using StudentManager_FrontEnd.Service;
using System.Net;
using System.IdentityModel.Tokens.Jwt;

namespace StudentManager_FrontEnd.Pages
{

    public class StudentModel : PageModel
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISetCookies setCookies;
        bool loggedin;

        public StudentModel(HttpClient httpClient, IHttpContextAccessor httpContextAccessor,ISetCookies setCookies)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.setCookies = setCookies;
            loggedin = false;
            this.httpClient = httpClient;
        }

        [BindProperty]
        public NewStudentDto NewStudentDto { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var jwtToken = httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtTokenDecoded = tokenHandler.ReadToken(jwtToken) as JwtSecurityToken;
            // Access the username from the JWT payload
            string username = jwtTokenDecoded?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            ViewData["User"] = username;
            var response = await httpClient.GetAsync("https://localhost:7089/Student/getAll");
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                responseBody = await response.Content.ReadAsStringAsync();
                ViewData["StudentList"] = responseBody;
                loggedin = true;
                return Page();
            }
            else
            {
                try
                {
                    JObject jsonObject = JObject.Parse(responseBody);
                    // Extract the message
                    string errorMessage = (string)jsonObject["message"];

                    if (errorMessage == "Token has expired")
                    {
                        var refreshToken = httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
                        var refreshUrl = $"https://localhost:7089/api/Auth/refresh?refreshToken={refreshToken}";
                        var responseRefresh = await httpClient.PostAsync(refreshUrl, null);
                        var responseBodyRefresh = await response.Content.ReadAsStringAsync();
                        if (responseRefresh.IsSuccessStatusCode)
                        {
                            var cookies = responseRefresh.Headers.GetValues("Set-Cookie");
                            setCookies.SetCookies(cookies, httpContextAccessor);
                        }
                        return RedirectToPage("/LogoutPage/TokenExpired");
                    }
                } catch (Exception ex)
                {

                }
                return Unauthorized();
            }
            
        }

        public async Task<IActionResult> OnPostAsync(string addStudent)
        {
            var jwtToken = httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var json = System.Text.Json.JsonSerializer.Serialize(NewStudentDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://localhost:7089/addStudent", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Added Student";
                await OnGetAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult>OnPostDeleteStudentAsync(int id)
        {
            var jwtToken = httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var deleteUrl = $"https://localhost:7089/deleteStudent/{id}?Id={id}";
            var response = await httpClient.DeleteAsync(deleteUrl);
            var responseBody = await response.Content.ReadAsStringAsync();
            if (responseBody!=null)
            {
                TempData["Message"] = responseBody;
                await OnGetAsync();
            }
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostEditStudentAsync(int id)
        {
            var jwtToken = httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var editData = new NewStudentDto
            {
                Name = Request.Form["Name"],
                RollNumber = Request.Form["RollNumber"],
                Age = int.Parse(Request.Form["Age"]),
                Cgpa = float.Parse(Request.Form["Cgpa"])
            };
            var json = System.Text.Json.JsonSerializer.Serialize(editData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var putUrl = $"https://localhost:7089/updateStudent/{id}?Id={id}";
            var response = await httpClient.PutAsync(putUrl, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Edited Student";
                await OnGetAsync();
            }
            return RedirectToPage();

        }
        
    }
}
