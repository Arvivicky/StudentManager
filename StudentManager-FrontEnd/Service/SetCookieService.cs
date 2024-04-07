
using Microsoft.AspNetCore.Http;

namespace StudentManager_FrontEnd.Service
{
    public class SetCookieService : ISetCookies
    {
        
        public Task SetCookies(IEnumerable<string> cookies, IHttpContextAccessor httpContextAccessor)
        {
            foreach (var cookie in cookies)
            {
                var cookieParts = cookie.Split(';')[0].Split('=');
                var cookieName = cookieParts[0];
                var cookieValue = cookieParts[1];
                if (cookieName == "refreshToken")
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.Now.AddDays(7),
                        SameSite = SameSiteMode.Lax,
                    };

                    // Add cookie to the response headers
                    httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, cookieValue, cookieOptions);
                }
                else
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax,
                    };

                    // Add cookie to the response headers
                    httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, cookieValue, cookieOptions);
                }
                

            }

            return null;
        }

    }
}
