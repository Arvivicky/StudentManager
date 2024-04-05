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

namespace StudentManager_FrontEnd.Pages
{
    public class StudentModel : PageModel
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;

        public StudentModel(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.httpClient = httpClient;

        }

        [BindProperty]
        public NewStudentDto NewStudentDto { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var jwtToken = httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var response = await httpClient.GetAsync("https://localhost:7089/Student/getAll");
            var responseBody = await response.Content.ReadAsStringAsync();
            ViewData["StudentList"] = responseBody;

            return Page();
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
