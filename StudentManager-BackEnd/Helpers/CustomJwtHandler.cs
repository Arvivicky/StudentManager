using Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StudentManager_BackEnd.Dto;
using StudentManager_BackEnd.Repository;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace StudentManager_BackEnd.Helpers
{
    public class CustomJwtHandler : JwtBearerHandler
    {
        
        private readonly string _secret;
        private readonly IConfiguration configuration;
        private readonly IUserRepo userRepo;

        public CustomJwtHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration,IUserRepo userRepo)
            : base(options, logger, encoder, clock)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.userRepo = userRepo;
            _secret = this.configuration["Jwt:Secret"];
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = Context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                return AuthenticateResult.Fail("Authorization header missing");
            }
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out var validatedToken);

                // Check if the token is expired
                if (validatedToken.ValidTo < DateTime.UtcNow)
                {

                    throw new SecurityTokenExpiredException("Token has expired");
                }

                // Set user principal
                var claims = (validatedToken as JwtSecurityToken).Claims;
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);

                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                var IsMenuAllowed = await CheckMenu(identity.Name,Context);
                if (IsMenuAllowed)
                {
                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    throw new UnauthorizedAccessException("Forbidden");
                }
            }
            catch (SecurityTokenExpiredException ex)
            {
                throw new SecurityTokenExpiredException("Token has expired");
            }
            catch (SecurityTokenValidationException ex)
            {
                throw new AuthenticationFailureException("Token is invalid");
            }

        }
        public async Task<bool> CheckMenu(string name,HttpContext httpContext)
        {
            var userDto = new UserDto
            {
                Username = name,
            };
            var method = httpContext.Request.Method;
            var url = httpContext.Request.Path.Value;
            var user = await userRepo.LoadUser(userDto);
            if (user != null)
            {
                foreach (var role in user.Roles)
                {
                    // Check if any role menu matches the provided method and URL
                    var hasAccess = role.RoleMenus.Any(rm => rm.Method == method && rm.Url == url);
                    if (hasAccess)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
