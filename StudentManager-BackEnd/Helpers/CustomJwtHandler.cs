using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace StudentManager_BackEnd.Helpers
{
    public class CustomJwtHandler : JwtBearerHandler
    {
        
        private readonly string _secret;
        private readonly IConfiguration configuration;

        public CustomJwtHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
                return AuthenticateResult.Success(ticket);
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
    }
}
