using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace StudentManager_FrontEnd.Pages
{
    public class UserModel : PageModel
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserModel(HttpClient httpClient,IHttpContextAccessor httpContextAccessor)
        {
            this.httpClient = httpClient;
            this.httpContextAccessor = httpContextAccessor;
        }
        public void OnGet()
        {
            var jwtToken = httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtTokenDecoded = tokenHandler.ReadToken(jwtToken) as JwtSecurityToken;
            string username = jwtTokenDecoded?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            string requestUrl = $"";
            
        }
    }
}
