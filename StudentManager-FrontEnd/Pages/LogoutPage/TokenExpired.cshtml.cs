using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using static System.Net.WebRequestMethods;
using StudentManager_FrontEnd.Service;

namespace StudentManager_FrontEnd.Pages.LogoutPage
{
    public class TokenExpiredModel : PageModel
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISetCookies setCookies;

        public TokenExpiredModel(HttpClient httpClient, IHttpContextAccessor httpContextAccessor,ISetCookies setCookies)
        {
            this.httpClient = httpClient;
            this.httpContextAccessor = httpContextAccessor;
            this.setCookies = setCookies;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostLogoutBtnAsync()
        {
            return RedirectToPage("/Index");
        }
        public async Task<IActionResult> OnPostContinueBtnAsync()
        {
            var refreshToken = httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var refreshUrl = $"https://localhost:7089/api/Auth/refresh?refreshToken={refreshToken}";
            var response = await httpClient.PostAsync(refreshUrl, null);
            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var cookies = response.Headers.GetValues("Set-Cookie");
                setCookies.SetCookies(cookies, httpContextAccessor);

                return RedirectToPage("/Student");
            }
            else
            {
                return RedirectToPage("/Index");
            }

        }
    }
}